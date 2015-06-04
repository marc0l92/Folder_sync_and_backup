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
		private NetworkStream networkStream;

		public SyncManager()
		{
			serverFileChecksum = new List<FileChecksum>();
			//this.syncThread = new Thread(new ThreadStart(this.doSync));
			//this.syncThread.IsBackground = true;
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
				networkStream = tcpClient.GetStream();
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
					this.sendCommand(NEW_CMD, currentFile.filePath);
					this.sendFile(currentFile.filePath);
				}
				else
				{
					// the file is also on the server
					if (currentFile.checksum != serverFileChecksum[pos].checksum)
					{
						// on the server there is a different version of the file
						this.sendCommand(EDIT_CMD, currentFile.filePath);
						this.sendFile(currentFile.filePath);
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
				sendCommand(DEL_CMD, currentFile.filePath);
			}
		}

		public void restoreVersion(String version)
		{
			throw new Exception("Function not implemented yet\nPlease contact the server admin:\nandrea.ferri@gmail.com");
			
		}

		private void sendCommand(SyncCommand command)
		{
			String message;
			Byte[] data;
			switch (command)
			{
				case START_CMD:
					message = "START:" + param1;
					break;
				case EDIT_CMD:
					message = "EDIT:" + param1;
					break;
				case DEL_CMD:
					message = "DEL:" + param1;
					break;
				case NEW_CMD:
					message = "NEW:" + param1;
					break;
				case RESTORE_CMD:
					message = "RESTORE:" + param1;
					break;
				case GET_CMD:
					message = "GET:" + param1 + ";" + param2;
					break;
				case ENDSYNC_CMD:
					message = "ENDSYNC:";
					break;
				default:
					message = "";
					break;
			}

			data = System.Text.Encoding.ASCII.GetBytes(message);
			networkStream.Write(data, 0, data.Length);
		}
		private void sendFile(String path)
		{
		}
		private List<FileChecksum> getServerCheckList()
		{
			return new List<FileChecksum>();
		}
		private String getFile()
		{
			return "";
		}

		private void commitChangesToServer()
		{
			serverFileChecksum = clientFileChecksum;
		}

	}
}
