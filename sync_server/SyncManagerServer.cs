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
        private String usrDir;
        private int version ;
        private bool serverStopped = false;
        private bool syncFinished = false;
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

        //Function Start Sync Button
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
        //Function Stop Sync Button
		public void stopSync()
		{
			this.serverStopped = true;
            this.syncFinished = true;
			this.listenThread.Abort();
            this.operationThread.Abort();
			statusDelegate("Server stopped", fSyncServer.LOG_INFO);
		}
        //Function Main Thread Listening for Connection, CHecking User Credential and Start the Slave Thread to serve the Client Request
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
                NetworkStream netStream = client.GetStream();
                StreamReader sr = new StreamReader(netStream);
                String usrLOGIN = sr.ReadLine();
                String[] usrCREDENTIAL = getCommand(usrLOGIN);
                if (checkCredential(usrCREDENTIAL[1], usrCREDENTIAL[2])&&usrCREDENTIAL[0]=="LOGIN")
                {
                    //Instantiate Client Slave
                    Thread clientThread = new Thread(new ParameterizedThreadStart(doSync));
                    clientThread.IsBackground = true;
                    clientThread.Start(client);

                }
                else client.Close(); //Else BAD CREDENTIAL Close Connection
                //Closing the Stream Opened for Check the Credential
                sr.Close();
                netStream.Close();
                
            }
		}


        //Client Slave Main Operation
        private void doSync(object client)
		{

            try
            {
                while (!syncFinished) { 

                TcpClient cl = (TcpClient)client;
                NetworkStream netStream = cl.GetStream();
                StreamReader sr = new StreamReader(netStream);
                StreamWriter sw = new StreamWriter(netStream);
                String cmd = createCommand("OK_START:" , "", "");
                sw.WriteLine(cmd);

                if ((cmd = sr.ReadLine())!=null)
                {
                    String[] command = this.getCommand(cmd);
                    if (doCommand(command, netStream))
                    {
                        statusDelegate("Command :"+command[0]+" Done Correctly", fSyncServer.LOG_INFO);
                    }
                    else
                    sr.Close();
                    //sw.Close();
                    netStream.Close();
                    cl.Close();
                }
                else
                {
                    sr.Close();
                    //sw.Close();
                    netStream.Close();
                }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }

				
		}

        private Boolean doCommand(String[] stringCMD, Object netStream)
        {
            
            switch (stringCMD[0])
            {
                case "START_CMD": return startSession(stringCMD[1], (NetworkStream) netStream);
                case "EDIT_CMD": return editFile();
                case "DEL_CMD": return delFile();
                case "NEW_CMD": return newFile();
                case "RESTORE_CMD": return restVers();
                case "GET_CMD": return getFile();
                case "ENDSYNC_CMD": return stopSession();
                default:
                    return false;
            }


        }
        private Boolean startSession(String usrPath, NetworkStream netStream)
        {

            this.usrDir = usrPath;
            this.generateServerChecksum(this.usrDir);
            netStream

            return true;
        }


        private Boolean editFile()
        {

            return true;
        }
        private Boolean delFile()
        {

            return true;
        }
        private Boolean newFile()
        {

            return true;
        }
        private Boolean restVers()
        {

            return true;
        }
        private Boolean getFile()
        {

            return true;
        }
        private Boolean stopSession() {

           
            return true;
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

        private Boolean checkCredential(String usr, String psw)
        {
            return true;
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

        /*

        public String createCommand(int CMD, String path, String checksum){
        
            JsonObject command;

        command  = new JsonObject(CMD.ToString);
        command["Command"] = JsonValue.CreateStringValue();
        command["Path"] = JsonValue.CreateStringValue(path);
        command["Checksum"] = JsonValue.CreateStringValue(checksum);

            return command.Stringify();
        }
        */
        //Function that return a Json String containing the command and the inserted variabels
        public String createCommand(String CMD, String path, String checksum)
        {

            JsonObject command;

            command = new JsonObject();
            command["Command"] = JsonValue.CreateStringValue(CMD);
            command["Path"] = JsonValue.CreateStringValue(path);
            command["Checksum"] = JsonValue.CreateStringValue(checksum);

            return command.Stringify();
        }
        //Function that return a Json String containing the command and the inserted list checksum
        public String createCommand(String CMD, List<FileChecksum> listChecksum)
        {

            JsonArray command = new JsonArray();

            command["Command"] = JsonValue.CreateStringValue(CMD);
            foreach (FileChecksum checksum in listChecksum)
            {
                command.Add(JsonValue.CreateStringValue(checksum.filePath));
                command.Add(JsonValue.CreateStringValue(checksum.checksum));
            }

            return command.Stringify();
        }
        //Convert Standard Command Returning a String Array of three elements
        public String[] getCommand(String jsonCMD)
        {
            String[] command = new String[3];
            JsonValue jsonOBJ = JsonValue.Parse(jsonCMD);
            command[0] = jsonOBJ.GetObject().GetNamedString("Command");
            command[1] = jsonOBJ.GetObject().GetNamedString("Path");
            command[2] = jsonOBJ.GetObject().GetNamedString("Checksum");
            return command;
        }
        


	}
}
