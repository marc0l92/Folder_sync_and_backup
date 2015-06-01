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
		private const int START_CMD = 1;
		private const int EDIT_CMD = 2;
		private const int DEL_CMD = 3;
		private const int NEW_CMD = 4;
		private const int RESTORE_CMD = 7;
		private const int GET_CMD = 8;
		private const int ENDSYNC_CMD = 9;

		private String username, password, directory;
		private Thread syncThread;
		private List<FileChecksum> clientFileChecksum;
		private bool thread_stopped = false;
		public delegate void StatusDelegate(String s);
		private StatusDelegate statusDelegate;
		private TcpClient tcpClient;
		private NetworkStream networkStream;

		public SyncManager()
		{
			clientFileChecksum = new List<FileChecksum>();
			this.syncThread = new Thread(new ThreadStart(this.doSync));
			this.syncThread.IsBackground = true;
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

			statusDelegate("Starting connection...");
			tcpClient = new TcpClient(address, port);
			networkStream = tcpClient.GetStream();
			statusDelegate("Connected");

			// Start the sync thread
			this.syncThread.Start();
		}

		public void stopSync()
		{
			this.thread_stopped = true;
			//syncThread.Abort();
		}

		private void doSync()
		{
			// Do the first connection
			this.sendCommand(START_CMD, this.directory);
			List<FileChecksum> serverFileChecksum = getServerCheckList();
			clientFileChecksum.Clear();
			this.generateClientChecksum(this.directory);

			// confronta checksum
			int pos;
			foreach(FileChecksum file in clientFileChecksum){
				pos = serverFileChecksum.IndexOf(file);
				
				if (pos < 0)
				{
					// create a new file on the server
					this.sendCommand(NEW_CMD, file.filePath);
					this.sendFile(file.filePath);
				}
				else
				{
					// the file is also on the server
					if (file.checksum != serverFileChecksum.ElementAt(pos).checksum)
					{
						// on the server there is a different version of the file
						this.sendCommand(EDIT_CMD, file.filePath);
						this.sendFile(file.filePath);
					}
					serverFileChecksum.RemoveAt(pos);
				}
			}
			foreach (FileChecksum serverFile in serverFileChecksum)
			{
				// delete extra file on the server
				this.sendCommand(DEL_CMD, serverFile.filePath);
			}

			// Do syncking
			while (!thread_stopped)
			{
				Thread.Sleep(2000);
			}
		}

		private void generateClientChecksum(String dir)
		{
			string[] fileList = Directory.GetFiles(dir);
			foreach (string filePath in fileList)
			{
				clientFileChecksum.Add(new FileChecksum(filePath));
			}

			// Recurse into subdirectories of this directory.
			string[] subdirectoryList = Directory.GetDirectories(dir);
			foreach (string subdirectoryPath in subdirectoryList)
			{
				this.generateClientChecksum(subdirectoryPath);
			}
		}

		private void sendCommand(int command, string param1 = "", string param2 = "")
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
	}
}
