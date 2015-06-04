using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;

namespace sync_client
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
		private TcpClient tcpClient;
		private StreamReader streamReader;
		private StreamWriter streamWriter;

		public SyncManager()
		{
			serverFileChecksum = new List<FileChecksum>();
		}

		public void setStatusDelegate(StatusDelegate sd)
		{
			this.statusDelegate = sd;
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
			if (syncThread.IsAlive)
			{
				syncThread.Abort();
			}
		}

		private void doSync()
		{
			try
			{
				// Create the connection
				statusDelegate("Starting connection...");
				tcpClient = new TcpClient(address, port);
				NetworkStream networkStream = tcpClient.GetStream();
				streamReader = new StreamReader(networkStream);
				streamWriter = new StreamWriter(networkStream);
				statusDelegate("Connected");
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
			// TODO
		}

		private void sendCommand(SyncCommand command)
		{
			streamWriter.WriteLine(command.convertToString());
		}
		private void sendFile(String path)
		{
			SyncCommand sc = new SyncCommand(SyncCommand.CommandSet.FILE, File.ReadAllLines(path));
			this.sendCommand(sc);
		}
		private List<FileChecksum> getServerCheckList()
		{
			String line = streamReader.ReadLine();
			SyncCommand sc;
			List<FileChecksum> serverCheckList = new List<FileChecksum>();

			while ((sc = SyncCommand.convertFromString(line)).Type != SyncCommand.CommandSet.ENDCHECK)
			{
				if (sc.Type != SyncCommand.CommandSet.CHECK) break;
				serverCheckList.Add(new FileChecksum(sc.FileName, sc.Checksum));
			}

			return serverCheckList;
		}
		private String getFile()
		{
			String line = streamReader.ReadLine();
			SyncCommand sc = SyncCommand.convertFromString(line);
			if (sc.Type != SyncCommand.CommandSet.FILE) throw new Exception("File not received");
			return sc.FileContent;
		}

		private void commitChangesToServer()
		{
			serverFileChecksum = clientFileChecksum;
		}

	}
}
