using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;

namespace sync_clientWPF
{
	class SyncManager
	{
		private const int SYNC_SLEEPING_TIME = 5000;

		private String address, username, password, directory;
		private int port;
		private Thread syncThread;
		private List<FileChecksum> serverFileChecksum;
		private List<FileChecksum> clientFileChecksum;
		private bool thread_stopped = false;
		public delegate void StatusDelegate(String s, bool fatalError = false);
		private StatusDelegate statusDelegate;
		private Socket tcpClient;
		private String receivedBuffer = "";
		private bool someChanges = false;
		private Mutex connectionMutex;


		public SyncManager(String address, int port)
		{
			serverFileChecksum = new List<FileChecksum>();
			clientFileChecksum = new List<FileChecksum>();
			connectionMutex = new Mutex();
			this.address = address;
			this.port = port;
		}

		public void setStatusDelegate(StatusDelegate sd)
		{
			this.statusDelegate = sd;
		}

		public bool login(String username, String password, String directory = "", bool register = false)
		{
			serverConnect(); // todo async connection
			if (register)
			{
				this.sendCommand(new SyncCommand(SyncCommand.CommandSet.NEWUSER, username, password, directory));
			}
			else
			{
				this.sendCommand(new SyncCommand(SyncCommand.CommandSet.LOGIN, username, password));
			}

			return (this.receiveCommand().Type == SyncCommand.CommandSet.AUTHORIZED);
		}

		public void startSync(String address, int port, String username, String password, String directory)
		{
			// Check if the directory is valid
			if (!Directory.Exists(directory))
			{
				throw new Exception("ERROR: Directory not exists");
			}
			this.directory = directory;
			if (directory[directory.Length - 1] == '\\')
			{
				directory = directory.Substring(0, directory.Length - 1);
			}
			this.username = username;
			this.password = password;
			this.address = address;
			this.port = port;

			// Start the sync thread
			this.thread_stopped = false;
			this.syncThread = new Thread(new ThreadStart(this.doSync));
			this.syncThread.IsBackground = true;
			this.syncThread.Start();
		}

		public void stopSync()
		{
			this.thread_stopped = true;
			// Release the socket.

			if (tcpClient.Connected)
			{
				tcpClient.Shutdown(SocketShutdown.Both);
				tcpClient.Close();
			}
			if (syncThread.IsAlive)
			{
				syncThread.Abort(); // TODO evitare di usare Abort
			}
		}

		private void serverConnect()
		{
			IPAddress ipAddress;
			// Generate the remote endpoint
			if (Regex.IsMatch(address, "^\\d{1,3}.\\d{1,3}.\\d{1,3}.\\d{1,3}$"))
			{
				String[] parts = address.Split('.');
				ipAddress = new IPAddress(new byte[] { Byte.Parse(parts[0]), Byte.Parse(parts[1]), Byte.Parse(parts[2]), Byte.Parse(parts[3]) });
			}
			else
			{
				statusDelegate("Request address form DNS");
				IPHostEntry ipHostInfo = Dns.GetHostEntry(address);
				ipAddress = ipHostInfo.AddressList[0];
			}

			IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);
			// Create a TCP/IP socket
			tcpClient = new Socket(remoteEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			// Connect to the remote endpoint
			statusDelegate("Starting connection...");
			tcpClient.Connect(remoteEP);
			statusDelegate("Connected to: " + tcpClient.RemoteEndPoint.ToString());
		}

		private void doSync()
		{
			try
			{
				connectionMutex.WaitOne();
				if (!tcpClient.Connected)
				{
					serverConnect();
					this.sendCommand(new SyncCommand(SyncCommand.CommandSet.LOGIN, username, password));
					if (receiveCommand().Type != SyncCommand.CommandSet.AUTHORIZED)
					{
						statusDelegate("Login fail", true);
						return;
					}
				}
				// Do the first connection
				statusDelegate("Send START");
				sendCommand(new SyncCommand(SyncCommand.CommandSet.START, directory));
				if (receiveCommand().Type != SyncCommand.CommandSet.AUTHORIZED)
				{
					statusDelegate("Wrong directory", true);
					return;
				};
				statusDelegate("Waiting for CHECK list...");
				serverFileChecksum = getServerCheckList();
				statusDelegate("Scan client changes...");
				someChanges = false;
				scanForClientChanges(directory);
				scanForDeletedFiles();
				commitChangesToServer(someChanges);
				tcpClient.Close();
				statusDelegate("First sinc completed");
				// Do syncking
				while (!thread_stopped)
				{
					statusDelegate("Idle");
					connectionMutex.ReleaseMutex();
					Thread.Sleep(SYNC_SLEEPING_TIME);
					connectionMutex.WaitOne();
					// connect
					serverConnect();
					statusDelegate("Syncing...");
					// login
					this.sendCommand(new SyncCommand(SyncCommand.CommandSet.LOGIN, username, password));
					if (receiveCommand().Type != SyncCommand.CommandSet.AUTHORIZED)
					{
						statusDelegate("Wrong directory", true);
						return;
					}
					// start
					sendCommand(new SyncCommand(SyncCommand.CommandSet.START, directory));
					if (receiveCommand().Type != SyncCommand.CommandSet.AUTHORIZED)
					{
						statusDelegate("Wrong directory", true);
						return;
					}
					serverFileChecksum = getServerCheckList();
					someChanges = false;
					scanForClientChanges(directory);
					scanForDeletedFiles();
					commitChangesToServer(someChanges);
					tcpClient.Close();
				}
			}
			catch (Exception ex)
			{
				statusDelegate(ex.Message, true);
			}
			finally
			{
				try
				{
					connectionMutex.ReleaseMutex(); // TODO
				}catch(Exception e){
					statusDelegate(e.Message, true);
				}
			}
		}

		private void scanForClientChanges(String dir)
		{
			// Get directory file list
			string[] fileList = Directory.GetFiles(dir);

			// Scan for changes
			foreach (string filePath in fileList)
			{
				FileChecksum currentFile = new FileChecksum(filePath, directory);
				// Search the file in the server list
				int pos = serverFileChecksum.FindIndex(x => (x.BaseFileName == currentFile.BaseFileName));

				if (pos < 0)
				{
					// create a new file on the server
					someChanges = true;
					FileInfo fi = new FileInfo(currentFile.FileName);
					this.sendCommand(new SyncCommand(SyncCommand.CommandSet.NEW, currentFile.BaseFileName, fi.Length.ToString()));
					this.sendFile(currentFile.FileName);
				}
				else
				{
					// the file is also on the server
					if (currentFile.Checksum != serverFileChecksum[pos].Checksum)
					{
						// on the server there is a different version of the file
						someChanges = true;
						FileInfo fi = new FileInfo(currentFile.FileName);
						this.sendCommand(new SyncCommand(SyncCommand.CommandSet.EDIT, currentFile.BaseFileName, fi.Length.ToString()));
						this.sendFile(currentFile.FileName);
					}
					serverFileChecksum.RemoveAt(pos);
				}

				clientFileChecksum.Add(currentFile);
			}

			// Recurse into subdirectories of this directory.
			string[] subdirectoryList = Directory.GetDirectories(dir);
			foreach (string subdirectoryPath in subdirectoryList)
			{
				this.scanForClientChanges(subdirectoryPath);
			}
		}

		private void scanForDeletedFiles()
		{
			foreach (FileChecksum currentFile in serverFileChecksum)
			{
				someChanges = true;
				sendCommand(new SyncCommand(SyncCommand.CommandSet.DEL, currentFile.BaseFileName));
			}
		}

		public void restoreVersion(String version)
		{
			throw new Exception("Function not implemented yet\nPlease contact the server admin:\nandrea.ferri@gmail.com");
			// TODO restore
		}

		private void sendCommand(SyncCommand command)
		{
			int bytesSent;
			// Get the command string
			String sCommand = command.convertToString();
			// Send the data through the socket
			while (sCommand.Length > 0)
			{
				bytesSent = tcpClient.Send(Encoding.ASCII.GetBytes(sCommand));
				sCommand = sCommand.Substring(bytesSent); // cat the message part already sent
			}
			if (receiveCommand().Type != SyncCommand.CommandSet.ACK)
			{
				statusDelegate("Protocol error", true);
			}
		}
		private SyncCommand receiveCommand()
		{
			byte[] data = new byte[1024];
			int dataRec, jsonEnd;
			SyncCommand sc;
			while ((jsonEnd = SyncCommand.searchJsonEnd(receivedBuffer)) == -1)
			{
				// Receive data from the server
				dataRec = tcpClient.Receive(data);
				receivedBuffer += Encoding.ASCII.GetString(data, 0, dataRec);
			}
			sc = SyncCommand.convertFromString(receivedBuffer.Substring(0, jsonEnd + 1));
			receivedBuffer = receivedBuffer.Substring(jsonEnd + 1);
			return sc;
		}

		private void sendFile(String path)
		{
			tcpClient.SendFile(path);
			if (receiveCommand().Type != SyncCommand.CommandSet.ACK)
			{
				statusDelegate("Error during file trasmission", true);
			}

		}

		private List<FileChecksum> getServerCheckList()
		{
			SyncCommand sc;
			List<FileChecksum> serverCheckList = new List<FileChecksum>();

			while ((sc = this.receiveCommand()).Type != SyncCommand.CommandSet.ENDCHECK)
			{
				if (sc.Type != SyncCommand.CommandSet.CHECK) throw new Exception("Check list receive error");
				serverCheckList.Add(new FileChecksum(sc.FileName, System.Text.Encoding.ASCII.GetBytes(sc.Checksum)));
			}

			return serverCheckList;
		}
		private void getFile(String fileName, Int64 fileLength)
		{
			byte[] buffer = new byte[1024];
			int rec = 0;

			if (!Directory.Exists(Path.GetDirectoryName(fileName)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(fileName));
			}
			BinaryWriter bFile = new BinaryWriter(File.Open(fileName, FileMode.Create));

			// Receive data from the server
			while (fileLength > 0)
			{
				rec = tcpClient.Receive(buffer);
				fileLength -= rec;
				bFile.Write(buffer, 0, rec);
			}
			bFile.Close();
			this.sendCommand(new SyncCommand(SyncCommand.CommandSet.ACK));
		}

		private void commitChangesToServer(bool changes)
		{
			if (changes)
				sendCommand(new SyncCommand(SyncCommand.CommandSet.ENDSYNC));
			else
				sendCommand(new SyncCommand(SyncCommand.CommandSet.NOSYNC));
			serverFileChecksum = clientFileChecksum;
			clientFileChecksum.Clear();
		}

		public List<Version> getVersions()
		{
			List<Version> versions = new List<Version>();
			Version version = null;
			SyncCommand sc;
			try
			{
				connectionMutex.WaitOne();
				serverConnect();
				// login
				this.sendCommand(new SyncCommand(SyncCommand.CommandSet.LOGIN, username, password));
				if (receiveCommand().Type == SyncCommand.CommandSet.AUTHORIZED)
				{
					statusDelegate("Retrieve version list...");
					sendCommand(new SyncCommand(SyncCommand.CommandSet.GETVERSIONS));
					while ((sc = this.receiveCommand()).Type != SyncCommand.CommandSet.ENDCHECK)
					{
						switch (sc.Type)
						{
							case SyncCommand.CommandSet.VERSION:
								version = new Version(sc.Version);
								versions.Add(version);
								break;
							case SyncCommand.CommandSet.CHECKVERSION:
								version.append(new VersionFile(sc.FileName, sc.Operation));
								break;
							default:
								throw new Exception("Version receive error");
						}
					}
					statusDelegate("Versions retrieved");
				}
				else
				{
					statusDelegate("Login fail");
				}

			}
			finally
			{
				tcpClient.Close();
				connectionMutex.ReleaseMutex();
			}
			return versions;
		}

	}
}
