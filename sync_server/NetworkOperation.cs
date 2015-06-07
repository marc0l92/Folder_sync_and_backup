using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace sync_server
{
    class NetworkOperation
    {


        private static ManualResetEvent receiveDone = new ManualResetEvent(false);


        public delegate void StatusDelegate(String s, int type);
        private static StatusDelegate statusDelegate;


        public void setStatusDelegateNetwork(StatusDelegate sd)
        {
            statusDelegate = sd;
        }


        public static Boolean doCommand(StateObject stateToDo)
        {
            switch (stateToDo.cmd.Type)
            {
                case SyncCommand.CommandSet.LOGIN:
                    return LoginUser(stateToDo);
                case SyncCommand.CommandSet.START:
                    return StartSession(stateToDo);
                case SyncCommand.CommandSet.GET:
                    return SendFileClient(stateToDo);
                case SyncCommand.CommandSet.RESTORE: // Todo Da implementare RESTORE
                    return false;
                case SyncCommand.CommandSet.ENDSYNC:
                    return EndSync(stateToDo);
                case SyncCommand.CommandSet.DEL:
                    return DeleteFile(stateToDo);
                case SyncCommand.CommandSet.NEW:
                    return NewFile(stateToDo);
                case SyncCommand.CommandSet.EDIT:
                    return EditFile(stateToDo);
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
                    statusDelegate("Recieved Wrong Command ", fSyncServer.LOG_INFO);
                    return true;
            }


        }
        public static Boolean LoginUser(StateObject stt)
        {
            String username = "";
            String password = "";
            String directory = "";
            int version = 0;
            //Get credential by DB
            //Check if user is just logged in
            statusDelegate("Get user data on DB ", fSyncServer.LOG_INFO);
            if (true) //compared to the sended is true
            {
                statusDelegate("User Credential Confermed ", fSyncServer.LOG_INFO);
                stt.client.usrNam = username;
                stt.client.usrPwd = password;
                stt.client.usrDir = directory;
                stt.client.vers = version;
                SyncCommand authorized = new SyncCommand(SyncCommand.CommandSet.AUTHORIZED);
                SendCommand(stt.workSocket, authorized.convertToString());
                statusDelegate("Send Back Authorized Message ", fSyncServer.LOG_INFO);
                return true;
            }
            else
            {
                statusDelegate("User Credential NOT Confirmed", fSyncServer.LOG_INFO);
                SyncCommand unauthorized = new SyncCommand(SyncCommand.CommandSet.UNAUTHORIZED);
                SendCommand(stt.workSocket, unauthorized.convertToString());
                statusDelegate("Send Back Unauthorized Message ", fSyncServer.LOG_INFO);
                return true;
            }
        }

        public static Boolean StartSession(StateObject stt)
        {

            if (string.Compare(stt.client.usrDir, stt.cmd.Directory) != 0)
            {
                statusDelegate("User Directory Change NOT Authorized", fSyncServer.LOG_INFO);
                SyncCommand unauthorized = new SyncCommand(SyncCommand.CommandSet.UNAUTHORIZED);
                SendCommand(stt.workSocket, unauthorized.convertToString());
                statusDelegate("Send Back Unauthorized Message because the user change the root directory for the connection ", fSyncServer.LOG_INFO);
                return true;
            }
            else
            {

                statusDelegate("User Directory Authorized, Start Send Check", fSyncServer.LOG_INFO);
                // todo Retreive filechecksum from the DB and save it into the state.userChecksum List to keep track of changes 
                // Assignment stt.UserChecksum=;
                foreach (FileChecksum check in stt.userChecksum)
                {
                    SyncCommand checkCommand = new SyncCommand(SyncCommand.CommandSet.CHECK, check.FileNameClient, check.Checksum);
                    SendCommand(stt.workSocket, checkCommand.convertToString());
                    statusDelegate("Send check Message ", fSyncServer.LOG_INFO);
                }

                SyncCommand endcheck = new SyncCommand(SyncCommand.CommandSet.ENDCHECK);
                SendCommand(stt.workSocket, endcheck.convertToString());
                statusDelegate("Send End check Message ", fSyncServer.LOG_INFO);
                return true;
            }
        }


        public static Boolean SendFileClient(StateObject stt)
        {
            int index = stt.userChecksum.FindIndex(x => x.FileNameClient == stt.cmd.FileName);
            String fileName = stt.userChecksum[index].FileNameServer;
            SyncCommand file = new SyncCommand(SyncCommand.CommandSet.FILE, stt.cmd.FileName);

            if (File.Exists(fileName))
            {
                SendCommand(stt.workSocket, file.convertToString());
                statusDelegate("Send File Command  ", fSyncServer.LOG_INFO);
                // Send file fileName to remote device
                stt.workSocket.SendFile(fileName);
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

        public static Boolean DeleteFile(StateObject stt)
        {
            int index = stt.userChecksum.FindIndex(x => x.FileNameClient == stt.cmd.FileName);
            stt.userChecksum.RemoveAt(index);
            statusDelegate("File Correctly Delete from the list of the files of the current Version", fSyncServer.LOG_INFO);
            return true; // Da Implementare Meglio
        }

        public static Boolean EndSync(StateObject stt)
        {
            //todo Update in the DB all file of the current version with a value equal to current version plus one 
            //todo Update user version on DB
            statusDelegate("DB Updated Correctly, Start Rename Files ", fSyncServer.LOG_INFO);
            //Get List
            stt.syncEnd = true;
            stt.wellEnd = true;
            return true;
        }

        public static Boolean NewFile(StateObject stt)
        {
            ReceiveFile(stt);
            statusDelegate("Received File correcty ", fSyncServer.LOG_INFO);
            FileChecksum file = new FileChecksum(stt.cmd.FileName + "_" + (stt.client.vers + 1), stt.cmd.FileName);
            stt.userChecksum.Add(file);
            statusDelegate("DB Updated Correctly", fSyncServer.LOG_INFO);
            return true;
        }


        public static Boolean EditFile(StateObject state)
        {
            int index = state.userChecksum.FindIndex(x => x.FileNameClient == state.cmd.FileName);
            state.userChecksum.RemoveAt(index);
            statusDelegate("File Correctly Delete from the list of the files of the current Version", fSyncServer.LOG_INFO);
            ReceiveFile(state);
            statusDelegate("Received File correcty ", fSyncServer.LOG_INFO);
            FileChecksum file = new FileChecksum(state.cmd.FileName + "_" + (state.client.vers + 1), state.cmd.FileName);
            state.userChecksum.Add(file);
            statusDelegate("DB Updated Correctly", fSyncServer.LOG_INFO);
            return true;
        }

        public static Boolean RestoreVersion(StateObject state)
        {
            //todo Get list of all file belonging to the selected version

            foreach (FileChecksum check in state.userChecksum)
            {
                if (File.Exists(check.FileNameServer))
                {
                    SyncCommand file = new SyncCommand(SyncCommand.CommandSet.FILE, check.FileNameClient);
                    SendCommand(state.workSocket, file.convertToString());
                    statusDelegate("Send File Command ", fSyncServer.LOG_INFO);
                    // Send file fileName to remote device
                    state.workSocket.SendFile(check.FileNameServer);
                    statusDelegate("File Sended Succesfully, Server Name:" + check.FileNameServer + "User Name: " + check.FileNameClient, fSyncServer.LOG_INFO);
                }
                else
                {
                    // todo FILE DOESN'T EXISTS MESSAGGE
                    statusDelegate("File doesn't exists  " + check.FileNameServer, fSyncServer.LOG_INFO);
                }
            }
            return true;
        }


        public static void ReceiveFile(StateObject state)
        {
            try
            {

                // Begin receiving the data from the remote device.
                state.workSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveFileCallback), state);
            }
            catch (Exception e)
            {
                statusDelegate("Exception:" + e.ToString(), fSyncServer.LOG_INFO);
            }
        }


        public static void ReceiveFileCallback(IAsyncResult ar)
 {
     try
     {
         // Retrieve the state object and the client socket 
         // from the asynchronous state object.
         StateObject state = (StateObject)ar.AsyncState;
         Socket client = state.workSocket;
         FileStream fs;

         string fileName = state.cmd.FileName + "_" + state.client.vers.ToString();


         // Delete the file if it exists. 
         if (File.Exists(fileName))
             fs = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
         else
             fs = File.Create(fileName);

         // Read data from the remote device.
         int bytesRead = client.EndReceive(ar);

         if (bytesRead > 0)
         {
             fs.WriteAsync(state.buffer, 0, bytesRead);
             // There might be more data, so store the data received so far.
             //state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

             // Get the rest of the data.
             client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                 new AsyncCallback(ReceiveCallback), state);
         }
         else
         {
             fs.Close();
         }
     }
     catch (Exception e)
     {
         statusDelegate("Exception:" + e.ToString(), fSyncServer.LOG_INFO);
     }
 }

        public static void SendCommand(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            statusDelegate("SendCommand Started", fSyncServer.LOG_INFO);

            // Begin sending the data to the remote device.
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        public static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                statusDelegate("Send Command Byte number: " + bytesSent, fSyncServer.LOG_INFO);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }




        public static void ReceiveCommand(Socket client)
        {
            try
            {
                // Create the state object.
                StateObject state = new StateObject();
                state.workSocket = client;

                // Begin receiving the data from the remote device.
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket 
                // from the asynchronous state object.
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    // Get the rest of the data.
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    // All the data has arrived; put it in response.
                    if (state.sb.Length > 1)
                    {
                        state.cmd = SyncCommand.convertFromString(state.sb.ToString());
                    }
                    // Signal that all bytes have been received.
                    receiveDone.Set();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }







    }
}
