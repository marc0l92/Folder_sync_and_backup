using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace sync_server
{
    class ClientManager
    {
        private Thread clientThread;
        private StateObject stateClient;
        private SyncClient client= new SyncClient();
        private AsyncManagerServer.StatusDelegate statusDelegate;
        private ManualResetEvent receiveDone = new ManualResetEvent(false);
        private Boolean syncEnd = false;
        private Boolean wellEnd = false;
        private List<FileChecksum> userChecksum;
        private SyncCommand cmd;
        private SyncSQLite mySQLite;
        private String serverDir;

        public ClientManager(Socket sock, String workDir, AsyncManagerServer.StatusDelegate sd)
        {
            statusDelegate = sd;
            stateClient = new StateObject();
            stateClient.workSocket = sock;
            serverDir = workDir;
            mySQLite = new SyncSQLite();
            clientThread = new Thread(new ThreadStart(doClient));
            clientThread.IsBackground = true;
            clientThread.Start();
            //statusDelegate("Start ClientTheard Successfully ", fSyncServer.LOG_INFO);
        }

        public void stop() {
            // todo Cosa succede se sto sincronizzando? devo fare un restore?
            syncEnd = true;
            stateClient.workSocket.Close();
        }

        public void setStatusDelegate(AsyncManagerServer.StatusDelegate sd)
        {
            statusDelegate = sd;
        }

        private void doClient()
        {
            while (!syncEnd)
            {
                receiveDone.Reset();
                // Receive the response from the remote device.
                this.ReceiveCommand(stateClient.workSocket);
                receiveDone.WaitOne();

                if (doCommand())
                    statusDelegate("Slave Thread Done Command Successfully ", fSyncServer.LOG_INFO);
                else
                    statusDelegate("Slave Thread Done Command with no Success", fSyncServer.LOG_INFO);

            }
            if (!wellEnd)
            {
                // todo IMPLEMENTARE RESTORE A VERSIONE PRECEDENTE
            }
            // todo See if necessary close connection
        }

        public void ReceiveCommand(Socket client)
        {
            try
            {
                // Begin receiving the data from the remote device.
                client.BeginReceive(stateClient.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), null);
            }
            catch (Exception e)
            {
                statusDelegate("Exception: " + e.Message, fSyncServer.LOG_INFO);
            }
        }

        public  void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket
                // from the asynchronous state object.
               // StateObject state = (StateObject)ar.AsyncState;
                // Socket client = state.workSocket;

                // Read data from the remote device.
                int bytesRead = stateClient.workSocket.EndReceive(ar);

                if ((bytesRead > 0))
                {
                    // There might be more data, so store the data received so far.
                    stateClient.sb.Append(Encoding.ASCII.GetString(stateClient.buffer, 0, bytesRead));
                }
                if (SyncCommand.searchJsonEnd(stateClient.sb.ToString()) == -1)
                {
                    // Get the rest of the data.
                    stateClient.workSocket.BeginReceive(stateClient.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), null);
                }
                else
                {
                    // All the data has arrived; put it in response.
                    if (stateClient.sb.Length > 1)
                    {
                        cmd = SyncCommand.convertFromString(stateClient.sb.ToString());
                        stateClient.sb.Clear();
						SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.ACK));
                    }
                    // Signal that all bytes have been received.
                    receiveDone.Set();
                }
            }
            catch (Exception e)
            {
                statusDelegate("Exception: " + e.Message, fSyncServer.LOG_INFO);
            }
        }

        public  Boolean doCommand()
        {
            if (cmd != null)
            {
                switch (cmd.Type)
                {
                    case SyncCommand.CommandSet.LOGIN:
                        statusDelegate(" Command Login ", fSyncServer.LOG_INFO);
                        return LoginUser();
                    case SyncCommand.CommandSet.START:
                        statusDelegate(" Command Start ", fSyncServer.LOG_INFO);
                        return StartSession();
                    case SyncCommand.CommandSet.GET:
                        statusDelegate(" Command Get ", fSyncServer.LOG_INFO);
                        return SendFileClient();
                    case SyncCommand.CommandSet.RESTORE: // Todo Da implementare RESTORE
                        statusDelegate(" Command Restore ", fSyncServer.LOG_INFO);
                        return RestoreVersion();
                    case SyncCommand.CommandSet.ENDSYNC:
                        statusDelegate("Command EndSync ", fSyncServer.LOG_INFO);
                        return EndSync();
                    case SyncCommand.CommandSet.NOSYNC:
                        statusDelegate("Command NoSync ", fSyncServer.LOG_INFO);
                        return NoSync();
                    case SyncCommand.CommandSet.DEL:
                        statusDelegate("Command Delete ", fSyncServer.LOG_INFO);
                        return DeleteFile();
                    case SyncCommand.CommandSet.NEW:
                        statusDelegate(" Command New ", fSyncServer.LOG_INFO);
                        return NewFile();
                    case SyncCommand.CommandSet.EDIT:
                        statusDelegate("Command Edit ", fSyncServer.LOG_INFO);
                        return EditFile();
                   case SyncCommand.CommandSet.NEWUSER:
                        statusDelegate(" Command NewUser ", fSyncServer.LOG_INFO);
                        return NewUser();
                   case SyncCommand.CommandSet.GETVERSIONS:
                        statusDelegate("Command Edit ", fSyncServer.LOG_INFO);
                        return GetVersions();
                    default:
                        statusDelegate("Recieved Wrong Command", fSyncServer.LOG_INFO); //TODO return false and manage difference
                        return true;
                }
            }
            else
            { //statusDelegate("Null Command Received", fSyncServer.LOG_INFO);
                return true;
            }

        }
        public  Boolean LoginUser()
        {
    
           // String directory = "";
            // int version = 0;

            statusDelegate("Get user data on DB (LoginUser)", fSyncServer.LOG_INFO);
            if (mySQLite.authenticateUser(cmd.Username, cmd.Password)) //Call DB Authentication User
            {
                statusDelegate("User Credential Confermed (LoginUser)", fSyncServer.LOG_INFO);
                client.usrNam = cmd.Username;
                client.usrPwd = cmd.Password;
                //client.vers = mySQLite.getUserLastVersion();
                SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.AUTHORIZED));
                statusDelegate("Send Back Authorized Message (LoginUser)", fSyncServer.LOG_INFO);
                return true;
            }
            else
            {
                statusDelegate("User Credential NOT Confirmed (LoginUser)", fSyncServer.LOG_INFO);
                SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.UNAUTHORIZED));
                statusDelegate("Send Back Unauthorized Message (LoginUser)", fSyncServer.LOG_INFO);
                return true;
            }
        }

        public Boolean NewUser()
        {
            Int64 userID = mySQLite.newUser(cmd.Username, cmd.Password, cmd.Directory);

            if (userID==-1) //Call DB New User
            {

                statusDelegate("Username in CONFLICT choose another one (NewUser)", fSyncServer.LOG_INFO);
                SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.UNAUTHORIZED));
                statusDelegate("Send Back Unauthorized Message (NewUser)", fSyncServer.LOG_INFO);
                return true;
            }
            else
            {
                statusDelegate("User Added Succesfully (NewUser)", fSyncServer.LOG_INFO);
                client.usrID = userID;
                client.usrNam = cmd.Username;
                client.usrPwd = cmd.Password;
                client.usrDir = cmd.Directory;
                client.vers = 0;
                SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.AUTHORIZED));
                statusDelegate("Send Back Authorized Message (NewUser)", fSyncServer.LOG_INFO);
                return true;
            }
        }

        public  Boolean StartSession()
        {
            Int64 userID = mySQLite.checkUserDirectory(client.usrNam, cmd.Directory); //Call DB Check Directory User

            if (userID==-1)
            {
                statusDelegate("User Directory Change NOT Authorized (StartSession)", fSyncServer.LOG_INFO);
                SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.UNAUTHORIZED));
                statusDelegate("Send Back Unauthorized Message because the user change the root directory for the connection (StartSession)", fSyncServer.LOG_INFO);
                return true;
            }
            else
            {
                client.usrID = userID;
                serverDir += "\\user" + client.usrID;
                client.usrDir = cmd.Directory;
                client.vers = mySQLite.getUserLastVersion(client.usrID); //Call DB Get Last Version
                statusDelegate("User Directory Authorized, Start Send Check(StartSession)", fSyncServer.LOG_INFO);
                SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.AUTHORIZED));
                userChecksum = mySQLite.getUserFiles(client.usrID, client.vers, serverDir); //Call DB Get Users Files
               
                foreach (FileChecksum check in userChecksum)
                {
                    SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.CHECK, check.FileNameClient, check.Checksum.ToString()));
                    statusDelegate("Send check Message(StartSession)", fSyncServer.LOG_INFO);
                }

                SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.ENDCHECK));
                statusDelegate("Send End check Message (StartSession)", fSyncServer.LOG_INFO);
                return true;
            }
        }

        public Boolean GetVersions()
        {
                Int64 lastVers = mySQLite.getUserLastVersion( client.usrID);
                int currentVersion = 1;
                List<FileChecksum> userChecksumA = mySQLite.getUserFiles(client.usrID, currentVersion, serverDir); //Call DB Get Users Files;
                while (currentVersion <= lastVers )
                {
                    SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.VERSION, currentVersion.ToString(), userChecksumA.Count.ToString(), "Timestamp"));
                    statusDelegate("Send Version Message(Version Command)", fSyncServer.LOG_INFO);

                    if (currentVersion == 1)
                    {
                        foreach (FileChecksum check in userChecksumA)
                        {
                            SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.CHECKVERSION, check.FileNameClient, "NEW"));
                            statusDelegate("Send check Version Message(Version Command)", fSyncServer.LOG_INFO);
                        }
                    }
                    else { 

                    List<FileChecksum> userChecksumB =mySQLite.getUserFiles(client.usrID, currentVersion, serverDir); //Call DB Get Users Files;

                    foreach (FileChecksum checkB in userChecksumB)
                    {
                        Boolean found = false;

                        foreach (FileChecksum checkA in userChecksumA)
                        {

                            if(checkA.FileNameClient==checkB.FileNameClient)
                                if(checkA.FileNameServer==checkB.FileNameServer)
                                {
                                    SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.CHECKVERSION, checkA.FileNameClient, "NONE"));
                                    statusDelegate("Send checkVers Message(Version Command)", fSyncServer.LOG_INFO);
                                    found = true;
                                    userChecksumA.Remove(checkA);
                                    break;
                                }
                                else
                                {
                                    SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.CHECKVERSION, checkA.FileNameClient, "EDIT"));
                                    statusDelegate("Send checkVers Message(Version Command)", fSyncServer.LOG_INFO);
                                    found = true;
                                    userChecksumA.Remove(checkA);
                                    break;
                                }
                        }

                       
                        if(!found)
                        {
                            SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.CHECKVERSION, checkB.FileNameClient, "NEW"));
                            statusDelegate("Send checkVers Message(Version Command)", fSyncServer.LOG_INFO);
                        }
                    }

                    if (userChecksumA.Count != 0)
                    {
                        foreach (FileChecksum checkA in userChecksumA)
                        {
                            SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.CHECKVERSION, checkA.FileNameClient, "DEL"));
                            statusDelegate("Send check Message(Version Command)", fSyncServer.LOG_INFO);
                        }
                    }
                    userChecksumA = userChecksumB;
                    }
                    currentVersion++;
                   
                }
                SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.ENDCHECK));
                statusDelegate("Send End check Message (Version Command)", fSyncServer.LOG_INFO);
                return true;
        }



        public  Boolean DeleteFile()
        {
            int index = userChecksum.FindIndex(x => x.FileNameClient == cmd.FileName);
            userChecksum.RemoveAt(index);
            statusDelegate("File Correctly Delete from the list of the files of the current Version (DeleteFile)", fSyncServer.LOG_INFO);
            return true; // Da Implementare Meglio
        }

        public  Boolean EndSync()
        {
           
            client.vers++;
            mySQLite.setUserFiles(client.usrID, client.vers, userChecksum); // Call DB Update to new Version all the Files
            statusDelegate("DB Updated Correctly (EndSync)", fSyncServer.LOG_INFO);
            //Get List
            syncEnd = true;
            wellEnd = true;
            mySQLite.closeConnection();
            return true;
        }

        public Boolean NoSync()
        {
            //Get List
            syncEnd = true;
            wellEnd = true;

            mySQLite.closeConnection();
            return true;
        }

        public  Boolean NewFile()
        {
            string fileNameDB = Utility.FilePathWithVers(cmd.FileName, client.vers+1);
            ReceiveFile(serverDir + fileNameDB, cmd.FileSize);
            statusDelegate("Received New File correcty (NewFile)", fSyncServer.LOG_INFO);
            FileChecksum file = new FileChecksum(cmd.FileName, serverDir + fileNameDB, fileNameDB);
            userChecksum.Add(file);
            return true;
        }

        public  Boolean EditFile()
        {
            int index = userChecksum.FindIndex(x => x.FileNameClient == cmd.FileName);
            userChecksum.RemoveAt(index);
            statusDelegate("File Correctly Delete from the list of the files of the current Version (EditFile)", fSyncServer.LOG_INFO);
            string fileNameDB = Utility.FilePathWithVers(cmd.FileName, client.vers + 1);
            ReceiveFile(serverDir + fileNameDB, cmd.FileSize);
            statusDelegate("Received File to Edit correcty  (EditFile)", fSyncServer.LOG_INFO);
            FileChecksum file = new FileChecksum(cmd.FileName, serverDir + fileNameDB, fileNameDB);
            userChecksum.Add(file);
            return true;
        }

        public  Boolean RestoreVersion( )
        {
            //todo Get list of all file belonging to the selected version
            userChecksum = mySQLite.getUserFiles(client.usrID, cmd.Version, serverDir); //Call DB Retrieve Version to Restore
            foreach (FileChecksum check in userChecksum)
            {
                if (File.Exists(check.FileNameServer))
                {
                    SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.FILE, check.FileNameClient));
                    statusDelegate("Send File Command (Restore Version)", fSyncServer.LOG_INFO);
                    // Send file fileName to remote device
                    stateClient.workSocket.SendFile(check.FileNameServer);
                    statusDelegate("File Sended Succesfully, Server Name:" + check.FileNameServer + "User Name: " + check.FileNameClient + "(Restore Version)", fSyncServer.LOG_INFO);
                }
                else
                {
                    // todo FILE DOESN'T EXISTS MESSAGGE
                    statusDelegate("File doesn't exists  " + check.FileNameServer + "(Restore Version)", fSyncServer.LOG_INFO);
                }
            }

            client.vers++;
            mySQLite.setUserFiles(client.usrID, client.vers, userChecksum); // Call DB Update to new Version all the Files
            return true;
        }


        public Boolean SendFileClient()
        {
            int index = userChecksum.FindIndex(x => x.FileNameClient == cmd.FileName);
            String fileName = userChecksum[index].FileNameServer;

            if (File.Exists(fileName))
            {
                SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.FILE, cmd.FileName));
                statusDelegate("Send File Command  ", fSyncServer.LOG_INFO);
                // Send file fileName to remote device
                stateClient.workSocket.SendFile(fileName);
                statusDelegate("File Sended Succesfully", fSyncServer.LOG_INFO);
                // TODO wait for ack
                receiveDone.Reset();
                // Receive the response from the remote device.
                this.ReceiveCommand(stateClient.workSocket);
                receiveDone.WaitOne();
                if (cmd.Type != SyncCommand.CommandSet.ACK) 
                    return false;
                return true;
            }
            else
            {
                // todo FILE DOESN'T EXISTS MESSAGGE
                statusDelegate("File doesn't exists  " + fileName, fSyncServer.LOG_INFO);
                return true;
            }

        }

        public  void ReceiveFile(String fileName, Int64 fileLength)
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
                rec = stateClient.workSocket.Receive(buffer);
				fileLength -= rec;
            	bFile.Write(buffer, 0, rec);
            }
			bFile.Close();
            SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.ACK));
            


            //try
            //{
            //    // Begin receiving the data from the remote device.
            //    stateClient.workSocket.BeginReceive(stateClient.buffer, 0, StateObject.BufferSize, 0,
            //        new AsyncCallback(ReceiveFileCallback), null);
            //}
            //catch (Exception e)
            //{
            //    statusDelegate("Exception:" + e.Message, fSyncServer.LOG_INFO);
            //}
        }


        //public  void ReceiveFileCallback(IAsyncResult ar)
        //{
        //    try
        //    {
        //        // Retrieve the state object and the client socket 
        //        // from the asynchronous state object.
        //       // StateObject state = (StateObject)ar.AsyncState;
        //        //Socket client = state.workSocket;
        //        FileStream fs;
        //        string fileName = serverDir + cmd.FileName + "_" + (client.vers);

        //        if (!Directory.Exists(Path.GetDirectoryName(fileName)))
        //        {
        //            Directory.CreateDirectory( Path.GetDirectoryName(fileName));
        //        }

        //        fs = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                
        //        // Read data from the remote device.
        //        int bytesRead = stateClient.workSocket.EndReceive(ar);

        //        if (bytesRead > 0)
        //        {
        //            fs.WriteAsync(stateClient.buffer, 0, bytesRead);
        //            // There might be more data, so store the data received so far.
        //            //state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
        //            fs.Close();

        //            // Get the rest of the data.
        //            stateClient.workSocket.BeginReceive(stateClient.buffer, 0, StateObject.BufferSize, 0,
        //                new AsyncCallback(ReceiveFileCallback), stateClient);
        //        }
        //        else
        //        {
        //            fs.Close();
        //            fileDone.Set();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        statusDelegate("Exception:" + e.Message, fSyncServer.LOG_INFO);
        //    }
        //}

        public  void SendCommand(Socket handler, SyncCommand command)
        {
            // Convert the string data to byte data using ASCII encoding.
			byte[] byteData = Encoding.ASCII.GetBytes(command.convertToString());

            statusDelegate("SendCommand Started", fSyncServer.LOG_INFO);

            // Begin sending the data to the remote device.
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        public  void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
               // Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = stateClient.workSocket.EndSend(ar);
            }
            catch (Exception e)
            {
                statusDelegate("Exception: " + e.Message, fSyncServer.LOG_INFO);
            }
        }

        /*
        private void generateChecksum(String dir, int version)
        {
            String pattern = "[" + "_" + version.ToString() + "\\" + "Z" + "]";
            string[] fileList = Directory.GetFiles(dir);
            foreach (string filePath in fileList)
            {
                if (Regex.IsMatch(filePath, pattern))
                {
                    userChecksum.Add(new FileChecksum(filePath));
                }
            }

            // Recurse into subdirectories of this directory.
            string[] subdirectoryList = Directory.GetDirectories(dir);
            foreach (string subdirectoryPath in subdirectoryList)
            {
                generateChecksum(subdirectoryPath, version);
            }
        }*/
    }
}
