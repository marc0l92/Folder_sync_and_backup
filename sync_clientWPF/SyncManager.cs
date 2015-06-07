using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace sync_clientWPF
{
	class SyncManager
	{
		private const int SYNC_SLEEPING_TIME = 2000;

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

		public SyncManager()
		{
			serverFileChecksum = new List<FileChecksum>();
			clientFileChecksum = new List<FileChecksum>();
		}

		public void setStatusDelegate(StatusDelegate sd)
		{
			this.statusDelegate = sd;
		}

		public bool login(String username, String password, bool register = false)
		{
			return true;
		}

		public void startSync(String address, int port, String username, String password, String directory)
		{
			// Check if the directory is valid
			if (!Directory.Exists(directory))
			{
				throw new Exception("ERROR: Directory not exists");
			}
			this.directory = directory;
			this.username = username;
			this.password = password;
			this.address = address;
			this.port = port;

			// Start the sync thread
			this.syncThread = new Thread(new ThreadStart(this.doSync));
			this.syncThread.IsBackground = true;
			this.syncThread.Start();
		}

		public void stopSync()
		{
			this.thread_stopped = true;
			// Release the socket.
			if (tcpClient.IsBound)
			{
				tcpClient.Shutdown(SocketShutdown.Both);
			}
			tcpClient.Close();
			if (syncThread.IsAlive)
			{
				syncThread.Abort();
			}
		}

		private void doSync()
		{
			try
			{
				statusDelegate("Starting connection...");
				// Generate the remote endpoint
				IPHostEntry ipHostInfo = Dns.GetHostEntry(address);
				IPAddress ipAddress = ipHostInfo.AddressList[0];
				IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);
				// Create a TCP/IP socket
				tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				// Connect to the remote endpoint
				tcpClient.Connect(remoteEP);
				statusDelegate("Connected to: " + tcpClient.RemoteEndPoint.ToString());

				// Do the first connection
				sendCommand(new SyncCommand(SyncCommand.CommandSet.START, directory));
				serverFileChecksum = getServerCheckList();
				scanForClientChanges(directory);
				scanForDeletedFiles();
				commitChangesToServer();

				// Do syncking
				while (!thread_stopped)
				{
					Thread.Sleep(SYNC_SLEEPING_TIME);
					scanForClientChanges(directory);
					scanForDeletedFiles();
					commitChangesToServer();
				}
			}
			catch(Exception ex)
			{
				statusDelegate(ex.Message, true);
			}
		}

		private void scanForClientChanges(String dir)
		{
			// Get directory file list
			string[] fileList = Directory.GetFiles(dir);
			
			// Scan for changes
			foreach (string filePath in fileList)
			{
				FileChecksum currentFile = new FileChecksum(filePath);
				// Search the file in the server list
				int pos = serverFileChecksum.FindIndex(x => x.Equals(currentFile));
				//int pos = serverFileChecksum.IndexOf(file);

				if (pos < 0)
				{
					// create a new file on the server
					sendCommand(new SyncCommand(SyncCommand.CommandSet.NEW, currentFile.FileName));
					this.sendFile(currentFile.FileName);
				}
				else
				{
					// the file is also on the server
					if (currentFile.Checksum != serverFileChecksum[pos].Checksum)
					{
						// on the server there is a different version of the file
						sendCommand(new SyncCommand(SyncCommand.CommandSet.EDIT, currentFile.FileName));
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
			foreach(FileChecksum currentFile in serverFileChecksum){
				sendCommand(new SyncCommand(SyncCommand.CommandSet.DEL, currentFile.FileName));
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
			sc = SyncCommand.convertFromString(receivedBuffer.Substring(0, jsonEnd));
			receivedBuffer = receivedBuffer.Substring(jsonEnd); 
			return sc;
		}

		private void sendFile(String path)
		{
			SyncCommand sc = new SyncCommand(SyncCommand.CommandSet.FILE, File.ReadAllLines(path));
			this.sendCommand(sc);
		}
		private List<FileChecksum> getServerCheckList()
		{
			SyncCommand sc;
			List<FileChecksum> serverCheckList = new List<FileChecksum>();

			while ((sc = this.receiveCommand()).Type != SyncCommand.CommandSet.ENDCHECK)
			{
				if (sc.Type != SyncCommand.CommandSet.CHECK) throw new Exception("Check list receive error");
				serverCheckList.Add(new FileChecksum(sc.FileName, sc.Checksum));
			}

			return serverCheckList;
		}
		private String getFile()
		{
			SyncCommand sc = this.receiveCommand();
			if (sc.Type != SyncCommand.CommandSet.FILE) throw new Exception("File not received");
			return sc.FileContent;
		}

		private void commitChangesToServer()
		{
			sendCommand(new SyncCommand(SyncCommand.CommandSet.ENDSYNC));
			serverFileChecksum = clientFileChecksum;
			clientFileChecksum.Clear();
		}

	}
}
