using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;



namespace sync_server
{
	class SyncManagerServer
	{

        private const int START_CMD = 1;
        private const int EDIT_CMD = 2;
        private const int DEL_CMD = 3;
        private const int NEW_CMD = 4;
        private const int RESTORE_CMD = 7;
        private const int GET_CMD = 8;
        private const int ENDSYNC_CMD = 9;

		public delegate void StatusDelegate(String s, int type);
		private StatusDelegate statusDelegate;
		private Thread listenThread;
		private String workDir;
        private int version ;
		private bool serverStopped = false;
		private TcpListener tcpServer;   

		private List<FileChecksum> serverFileChecksum;
		private NetworkStream networkStream;
        
        // Inizializza un nuovo Thread che compiera' listenForConnection e set del flag IsBackGround to True
		public SyncManagerServer()
		{
			this.listenThread = new Thread(new ThreadStart(this.listenForConnections));
			this.listenThread.IsBackground = true;
		}

		public void setStatusDelegate(StatusDelegate sd)
		{
			this.statusDelegate = sd;
		}

		public void startSync(int port, String workDir)
		{
			// Check if the directory is valid
			if (!Directory.Exists(workDir))
			{
				throw new Exception("Directory not exists");
			}
			this.workDir = workDir;

			// Server start
			IPAddress localAddr = IPAddress.Parse("127.0.0.1");
			tcpServer = new TcpListener(localAddr, port);
			tcpServer.Start();
			statusDelegate("Server started", fSyncServer.LOG_INFO);

			// Start the sync thread
			this.listenThread.Start();
		}

		public void stopSync()
		{
			this.serverStopped = true;
			this.listenThread.Abort();
			statusDelegate("Server stopped", fSyncServer.LOG_INFO);
		}

		private void listenForConnections()
		{
			// thread that waiting for connections
			while (!serverStopped)
			{
				statusDelegate("Waiting for connections", fSyncServer.LOG_INFO);
				TcpClient client = tcpServer.AcceptTcpClient();
				statusDelegate("Client connected", fSyncServer.LOG_INFO);
				NetworkStream networkStream = client.GetStream();
				//byte[] data = new Byte[256];
				
                //CREAZIONE NUOVO THREAD SERVIZIO CLIENT
                String message;
				int i;
				while(client.Connected){
					//i = networkStream.Read(data, 0, data.Length);
					//message = System.Text.Encoding.ASCII.GetString(data, 0, i);
					//statusDelegate(message, fSyncServer.LOG_NORMAL);
                    message=this.getClientCommand();
				}
			}
		}
        //Genera il file checksum delle cartelle e ricorsivamente dei file interni

        private void generateServerChecksum(String dir)
        {
            String pattern= "["+ "_" + version.ToString() + "\\" + "Z" + "]" ;
            string[] fileList = Directory.GetFiles(dir);
            foreach (string filePath in fileList)
            {
                if(Regex.IsMatch(filePath, pattern))
                {
                    serverFileChecksum.Add(new FileChecksum(filePath));
                }
            }

            // Recurse into subdirectories of this directory.
            string[] subdirectoryList = Directory.GetDirectories(dir);
            foreach (string subdirectoryPath in subdirectoryList)
            {
                this.generateServerChecksum(subdirectoryPath);
            }
        }

		




		private void doSync()
		{
            //Recieve Client Dir
            String clientDir = getClientDir();
            //Check if Client Dir is Equal to Work Dir esle Esare all and Start New Sync Session
            if(clientDir!=workDir)
            {
                Directory.Delete(workDir, true);
                workDir = clientDir;
                Directory.CreateDirectory(workDir);

                //CANCEL TO DB MUST BE ADDED
            }

			
			this.sendCheckList(serverFileChecksum);

			

			// confronta checksum
			int pos;
			foreach(FileChecksum file in serverFileChecksum){
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

		private Boolean sendCheckList(List<FileChecksum>)
		{
			return true;
		}

		private String getFile()
		{
			return "";
		}


        private String getClientDir()
        {

            // Receive the TcpClient Directory Path

            // Buffer to store the response bytes.
            Byte[] data = new Byte[256];

            // String to store the response ASCII representation.
            String clientDir = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = this.networkStream.Read(data, 0, data.Length);
            clientDir = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            statusDelegate("Recived Directory: " + clientDir , fSyncServer.LOG_NORMAL);        
            return clientDir;
        }



         private String getClientCommand()
        {

            // Receive the TcpClient Directory Path

            // Buffer to store the response bytes.
            Byte[] data = new Byte[256];

            // String to store the response ASCII representation.
            String Command = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = this.networkStream.Read(data, 0, data.Length);
            Command = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            statusDelegate("Recived Command: " + Command , fSyncServer.LOG_NORMAL);        
            return Command;
        }

        private String[] getClientLogin()
        {

            // Receive the TcpClient Directory Path

            // Buffer to store the response bytes.
            Byte[] data = new Byte[256];

            // String to store the response ASCII representation.
            String login = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = this.networkStream.Read(data, 0, data.Length);
            login = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            statusDelegate("Recived Command: " + login , fSyncServer.LOG_NORMAL);  
            string [] split = login.Split(new Char [] {: , ;} );
            return split;
        }

        private void resetVersion()
        {
            this.version = 0;
        }

        private void setVersion(int version)
        {
            if(version>0)
                this.version = version;
        }

        private void incVersion()
        {
                this.version++;
        }

        private int getVersion()
        {
            return this.version;
        }


	}
}
