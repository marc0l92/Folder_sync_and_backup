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
        private Thread operationThread;
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
                //Get Connection
				statusDelegate("Waiting for connections", fSyncServer.LOG_INFO);
				TcpClient client = tcpServer.AcceptTcpClient();
				statusDelegate("Client connected", fSyncServer.LOG_INFO);


                //Check User Credential


                //Instantiate Client Slave
                Thread clientThread = new Thread(new ParameterizedThreadStart(doSync));
			    clientThread.IsBackground = true;
                clientThread.Start(client);
            }
		}


        //Client Slave Main Operation
        private void doSync(object client)
		{

                TcpClient cl = (TcpClient) client;
                NetworkStream netStream = cl.GetStream();
                StreamReader sr = new StreamReader(netStream);
                StreamWriter sw = new StreamWriter(netStream);
                String cmd =sr.ReadLine();
                String[] command = this.getCommand(cmd);
				
		}

        private void doCommand(String[] stringCMD)
        {

        }
		/*

		
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
        */
        //Funzioni per gestire la versione

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



        public String createCommand(int CMD, String path, String checksum){
        
            JsonObject command;

        command  = new JsonObject(CMD.ToString);
        command["Command"] = JsonValue.CreateStringValue();
        command["Path"] = JsonValue.CreateStringValue(path);
        command["Checksum"] = JsonValue.CreateStringValue(checksum);

            return command.Stringify();
        }

        public String[] getCommand(String jsonoCMD)
        {
            String[] command = new String[3];
            JsonValue jsonOBJ = JsonValue.Parse(jsonoCMD);
            command[0] = jsonOBJ.GetObject().GetNamedString("Command");
            command[1] = jsonOBJ.GetObject().GetNamedString("Path");
            command[2] = jsonOBJ.GetObject().GetNamedString("Checksum");

            return command;
        }
        


	}
}
