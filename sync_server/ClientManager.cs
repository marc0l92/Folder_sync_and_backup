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

        public ClientManager(Socket sock)
        {
            stateClient = new StateObject();
            stateClient.workSocket = sock;
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
                        return LoginUser();
                    case SyncCommand.CommandSet.START:
                        return StartSession();
                    case SyncCommand.CommandSet.GET:
                        return SendFileClient();
                    case SyncCommand.CommandSet.RESTORE: // Todo Da implementare RESTORE
                        return RestoreVersion();
                    case SyncCommand.CommandSet.ENDSYNC:
                        return EndSync();
                    case SyncCommand.CommandSet.DEL:
                        return DeleteFile();
                    case SyncCommand.CommandSet.NEW:
                        return NewFile();
                    case SyncCommand.CommandSet.EDIT:
                        return EditFile();
                   case SyncCommand.CommandSet.NEWUSER: 
                           return NewUser();       
                    //   	case SyncCommand.CommandSet.AUTHORIZED:
                    //           statusDelegate("Recieved Wrong Command ", fSyncServer.LOG_INFO);
                    //           return false;
                    //		case SyncCommand.CommandSet.UNAUTHORIZED:
                    //           statusDelegate("Recieved Wrong Command ", fSyncServer.LOG_INFO);
                    //			return false;
                    //		case SyncCommand.CommandSet.FILE:
                    //          return false;
                    //			break;

                    //		case SyncCommand.CommandSet.CHECK:
                    //          return false;
                    //			break;
                    //		case SyncCommand.CommandSet.ENDCHECK:
                    //          return false;
                    //			break;
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

            statusDelegate("Get user data on DB ", fSyncServer.LOG_INFO);
            if (mySQLite.authenticateUser(cmd.Username, cmd.Password)) //Call DB Authentication User
            {
                statusDelegate("User Credential Confermed", fSyncServer.LOG_INFO);
                client.usrNam = cmd.Username;
                client.usrPwd = cmd.Password;
                //client.usrDir = directory;
                //client.vers = mySQLite.getUserLastVersion();
                SyncCommand authorized = new SyncCommand(SyncCommand.CommandSet.AUTHORIZED);
                SendCommand(stateClient.workSocket, authorized.convertToString());
                statusDelegate("Send Back Authorized Message", fSyncServer.LOG_INFO);
                return true;
            }
            else
            {
                statusDelegate("User Credential NOT Confirmed", fSyncServer.LOG_INFO);
                SyncCommand unauthorized = new SyncCommand(SyncCommand.CommandSet.UNAUTHORIZED);
                SendCommand(stateClient.workSocket, unauthorized.convertToString());
                statusDelegate("Send Back Unauthorized Message", fSyncServer.LOG_INFO);
                return true;
            }
        }

        public Boolean NewUser()
        {
            Int64 userID = mySQLite.newUser(cmd.Username, cmd.Password, "/Directory");

            if (userID==-1) //Call DB New User
            {
                
                statusDelegate("Username in CONFLICT choose another one", fSyncServer.LOG_INFO);
                SyncCommand unauthorized = new SyncCommand(SyncCommand.CommandSet.UNAUTHORIZED);
                SendCommand(stateClient.workSocket, unauthorized.convertToString());
                statusDelegate("Send Back Unauthorized Message", fSyncServer.LOG_INFO);
                return true;
            }
            else
            {
                statusDelegate("User Added Succesfully", fSyncServer.LOG_INFO);
                client.usrID = userID;
                client.usrNam = cmd.Username;
                client.usrPwd = cmd.Password;
                client.usrDir = cmd.Directory;
                client.vers = 0;
                SyncCommand authorized = new SyncCommand(SyncCommand.CommandSet.AUTHORIZED);
                SendCommand(stateClient.workSocket, authorized.convertToString());
                statusDelegate("Send Back Authorized Message", fSyncServer.LOG_INFO);
                return true;
            }
        }

        public  Boolean StartSession()
        {
            Int64 userID = mySQLite.checkUserDirectory(client.usrNam, cmd.Directory); //Call DB Check Directory User

            if (userID==-1)
            {
                statusDelegate("User Directory Change NOT Authorized", fSyncServer.LOG_INFO);
                SyncCommand unauthorized = new SyncCommand(SyncCommand.CommandSet.UNAUTHORIZED);
                SendCommand(stateClient.workSocket, unauthorized.convertToString());
                statusDelegate("Send Back Unauthorized Message because the user change the root directory for the connection ", fSyncServer.LOG_INFO);
                return true;
            }
            else
            {
                client.usrID = userID;
                client.vers = mySQLite.getUserLastVersion(client.usrID); //Call DB Get Last Version
                statusDelegate("User Directory Authorized, Start Send Check", fSyncServer.LOG_INFO);
                userChecksum = mySQLite.getUserFiles(client.usrID, client.vers); //Call DB Get Users Files
               
                foreach (FileChecksum check in userChecksum)
                {
                    SyncCommand checkCommand = new SyncCommand(SyncCommand.CommandSet.CHECK, check.FileNameClient, check.Checksum);
                    SendCommand(stateClient.workSocket, checkCommand.convertToString());
                    statusDelegate("Send check Message", fSyncServer.LOG_INFO);
                }

                SyncCommand endcheck = new SyncCommand(SyncCommand.CommandSet.ENDCHECK);
                SendCommand(stateClient.workSocket, endcheck.convertToString());
                statusDelegate("Send End check Message", fSyncServer.LOG_INFO);
                return true;
            }
        }


        public  Boolean SendFileClient()
        {
            int index = userChecksum.FindIndex(x => x.FileNameClient == cmd.FileName);
            String fileName = userChecksum[index].FileNameServer;
            SyncCommand file = new SyncCommand(SyncCommand.CommandSet.FILE, cmd.FileName);

            if (File.Exists(fileName))
            {
                SendCommand(stateClient.workSocket, file.convertToString());
                statusDelegate("Send File Command  ", fSyncServer.LOG_INFO);
                // Send file fileName to remote device
                stateClient.workSocket.SendFile(fileName);
                statusDelegate("File Sended Succesfully", fSyncServer.LOG_INFO);

                return true;
            }
            else
            {
                // todo FILE DOESN'T EXISTS MESSAGGE
                statusDelegate("File doesn't exists  " + fileName, fSyncServer.LOG_INFO);
                return true;
            }

        }

        public  Boolean DeleteFile()
        {
            int index = userChecksum.FindIndex(x => x.FileNameClient == cmd.FileName);
            userChecksum.RemoveAt(index);
            statusDelegate("File Correctly Delete from the list of the files of the current Version", fSyncServer.LOG_INFO);
            return true; // Da Implementare Meglio
        }

        public  Boolean EndSync()
        {
           
            client.vers++;
            mySQLite.setUserFiles(client.usrID, client.vers, userChecksum); // Call DB Update to new Version all the Files
            statusDelegate("DB Updated Correctly, Start Rename Files ", fSyncServer.LOG_INFO);
            //Get List
            syncEnd = true;
            wellEnd = true;
            return true;
        }

        public  Boolean NewFile()
        {
            ReceiveFile();
            statusDelegate("Received File correcty ", fSyncServer.LOG_INFO);
            FileChecksum file = new FileChecksum(cmd.FileName + "_" + (client.vers + 1), cmd.FileName);
            userChecksum.Add(file);
            statusDelegate("DB Updated Correctly", fSyncServer.LOG_INFO);
            return true;
        }

        public  Boolean EditFile()
        {
            int index = userChecksum.FindIndex(x => x.FileNameClient == cmd.FileName);
            userChecksum.RemoveAt(index);
            statusDelegate("File Correctly Delete from the list of the files of the current Version", fSyncServer.LOG_INFO);
            ReceiveFile();
            statusDelegate("Received File correcty ", fSyncServer.LOG_INFO);
            FileChecksum file = new FileChecksum(cmd.FileName + "_" + (client.vers + 1), cmd.FileName);
            userChecksum.Add(file);
            statusDelegate("DB Updated Correctly", fSyncServer.LOG_INFO);
            return true;
        }

        public  Boolean RestoreVersion( )
        {
            //todo Get list of all file belonging to the selected version
            userChecksum = mySQLite.getUserFiles(client.usrID, cmd.Version); //Call DB Retrieve Version to Restore
            foreach (FileChecksum check in userChecksum)
            {
                if (File.Exists(check.FileNameServer))
                {
                    SyncCommand file = new SyncCommand(SyncCommand.CommandSet.FILE, check.FileNameClient);
                    SendCommand(stateClient.workSocket, file.convertToString());
                    statusDelegate("Send File Command ", fSyncServer.LOG_INFO);
                    // Send file fileName to remote device
                    stateClient.workSocket.SendFile(check.FileNameServer);
                    statusDelegate("File Sended Succesfully, Server Name:" + check.FileNameServer + "User Name: " + check.FileNameClient, fSyncServer.LOG_INFO);
                }
                else
                {
                    // todo FILE DOESN'T EXISTS MESSAGGE
                    statusDelegate("File doesn't exists  " + check.FileNameServer, fSyncServer.LOG_INFO);
                }
            }

            client.vers++;
            mySQLite.setUserFiles(client.usrID, client.vers, userChecksum); // Call DB Update to new Version all the Files
            return true;
        }


        public  void ReceiveFile( )
        {
            try
            {
                // Begin receiving the data from the remote device.
                stateClient.workSocket.BeginReceive(stateClient.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveFileCallback), null);
            }
            catch (Exception e)
            {
                statusDelegate("Exception:" + e.Message, fSyncServer.LOG_INFO);
            }
        }


        public  void ReceiveFileCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket 
                // from the asynchronous state object.
               // StateObject state = (StateObject)ar.AsyncState;
                //Socket client = state.workSocket;
                FileStream fs;

                string fileName = cmd.FileName + "_" + client.vers.ToString();

                fs = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                
                // Read data from the remote device.
                int bytesRead = stateClient.workSocket.EndReceive(ar);

                if (bytesRead > 0)
                {
                    fs.WriteAsync(stateClient.buffer, 0, bytesRead);
                    // There might be more data, so store the data received so far.
                    //state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    // Get the rest of the data.
                    stateClient.workSocket.BeginReceive(stateClient.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), stateClient);
                }
                else
                {
                    fs.Close();
                }
            }
            catch (Exception e)
            {
                statusDelegate("Exception:" + e.Message, fSyncServer.LOG_INFO);
            }
        }

        public  void SendCommand(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.ASCII.GetBytes(data);

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
                statusDelegate("Send Command Byte number: " + bytesSent, fSyncServer.LOG_INFO);
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
