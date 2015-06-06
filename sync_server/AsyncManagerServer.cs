using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Threading;
namespace sync_server
{

// State object for reading client data asynchronously
public class StateObject {
    // Client  socket.
    public Socket workSocket = null;
    // Size of receive buffer.
    public const int BufferSize = 1024;
    // Receive buffer.
    public byte[] buffer = new byte[BufferSize];
    // Received data string.
    public StringBuilder sb = new StringBuilder();
    // Received Cmd
    public SyncCommand cmd;
    // Served Client
    public SyncClient clnt = new SyncClient();
    // Served Client
    public Boolean syncEnd = false;

}

public class AsyncManagerServer {
    // Thread signal.
    private static int localport; 
    private static IPAddress localAddr = IPAddress.Parse("127.0.0.1");

    public delegate void StatusDelegate(String s, int type);
    private static StatusDelegate statusDelegate;
    public static ManualResetEvent allDone = new ManualResetEvent(false);
    private static ManualResetEvent receiveDone = new ManualResetEvent(false);
    private static bool serverStopped = false;

    // The response from the remote device.
    private static String command = String.Empty;

    private static List<FileChecksum> UserChecksum;
    
    public AsyncManagerServer() {
    }

    public static void StartListening() {
        // Data buffer for incoming data.
        byte[] bytes = new Byte[1024];

        // Establish the local endpoint for the socket.
        // The DNS name of the computer
        // running the listener is "host.contoso.com".
        IPEndPoint localEndPoint = new IPEndPoint(localAddr, localport);

        // Create a TCP/IP socket.
        Socket listener = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp );

        // Bind the socket to the local endpoint and listen for incoming connections.
        try {
            listener.Bind(localEndPoint);
            listener.Listen(100);

            statusDelegate("Start Listening on Port: "+ localport + "Address: " + localAddr, fSyncServer.LOG_INFO);
            while (!serverStopped)
            {
                // Set the event to nonsignaled state.
                allDone.Reset();

                // Start an asynchronous socket to listen for connections.
                listener.BeginAccept( 
                    new AsyncCallback(AcceptCallback),
                    listener );

                statusDelegate("Connected and Created New Thred to Serve Client", fSyncServer.LOG_INFO);
                // Wait until a connection is made before continuing.
                allDone.WaitOne();
                statusDelegate("Main Thread Free", fSyncServer.LOG_INFO);
            }

        } catch (Exception e) {

            statusDelegate("Connection Error Exception:" + e.ToString(), fSyncServer.LOG_INFO);
        }
        
    }

    public static void AcceptCallback(IAsyncResult ar) {
        // Signal the main thread to continue.
        allDone.Set();
        statusDelegate("Slave Thread created Successfully ", fSyncServer.LOG_INFO);
        // Get the socket that handles the client request.
        Socket listener = (Socket) ar.AsyncState;
        Socket handler = listener.EndAccept(ar);

        // Create the state object.
        StateObject state = new StateObject();
        state.workSocket = handler;
       // handler.BeginReceive( state.buffer, 0, StateObject.BufferSize, 0,
         //   new AsyncCallback(ReadCallback), state);
        while(!state.syncEnd)
        {
            // Receive the response from the remote device.
            ReceiveCommand(state.workSocket);
            receiveDone.WaitOne();
            state.cmd = SyncCommand.convertFromString(command);
            if(doCommand(state))
            {

            }
            else
            {

            }
        }
        //See if necessary close connection
    }

    private static Boolean doCommand(StateObject stateToDo)
    {
        switch (stateToDo.cmd.Type)
			{
                case SyncCommand.CommandSet.LOGIN:
					return LoginUser(stateToDo);
				case SyncCommand.CommandSet.START:
					return StartSession(stateToDo);
				case SyncCommand.CommandSet.AUTHORIZED:
                    statusDelegate("Recieved Wrong Command ", fSyncServer.LOG_INFO);
                    return false;
				case SyncCommand.CommandSet.UNAUTHORIZED:
                    statusDelegate("Recieved Wrong Command ", fSyncServer.LOG_INFO);
					return false;
				case SyncCommand.CommandSet.EDIT:
					return false;
					break;
				case SyncCommand.CommandSet.DEL:
					return DeleteFile();
				case SyncCommand.CommandSet.NEW:
					return false;
					break;
				case SyncCommand.CommandSet.FILE:
                    return false;
					break;
				case SyncCommand.CommandSet.GET:
                    return SendFile(stateToDo);
					
				case SyncCommand.CommandSet.RESTORE:
                    return false;
					break;
				case SyncCommand.CommandSet.ENDSYNC:
                    stateToDo.syncEnd = true;
                    return true;
				case SyncCommand.CommandSet.CHECK:
                    return false;
					break;
				case SyncCommand.CommandSet.ENDCHECK:
                    return false;
					break;
				default:
					return false;
			}


    }
    private static Boolean LoginUser(StateObject stt)
    {
        String username=new String("");
        String password = new String("");
        String directory = new String("");
        int version;
        //Get credential by DB
        //Check if user is just logged in
        statusDelegate("Get user data on DB ", fSyncServer.LOG_INFO);
        if (true) //compared to the sended is true
        {
            statusDelegate("User Credential Confermed ", fSyncServer.LOG_INFO);
            stt.clnt.usrNam = username;
            stt.clnt.usrPwd = password;
            stt.clnt.usrDir = directory;
            stt.clnt.vers = version;
            SyncCommand authorized = new SyncCommand(SyncCommand.CommandSet.AUTHORIZED);
            SendCommand(stt.workSocket, authorized.ToString());
            statusDelegate("Send Back Authorized Message ", fSyncServer.LOG_INFO);
            return true;
        }
        else
        {
            statusDelegate("User Credential NOT Confirmed", fSyncServer.LOG_INFO);
            SyncCommand unauthorized = new SyncCommand(SyncCommand.CommandSet.UNAUTHORIZED);
            SendCommand(stt.workSocket, unauthorized.ToString());
            statusDelegate("Send Back Unauthorized Message ", fSyncServer.LOG_INFO);
            return false;
        }
    }

    private static  Boolean StartSession(StateObject stt)
    {

        if (string.Compare(stt.clnt.usrDir, stt.cmd.Directory) != 0)
        {
            statusDelegate("User Directory Change NOT Authorized", fSyncServer.LOG_INFO);
            SyncCommand unauthorized = new SyncCommand(SyncCommand.CommandSet.UNAUTHORIZED);
            SendCommand(stt.workSocket, unauthorized.ToString());
            statusDelegate("Send Back Unauthorized Message because the user change the root directory for the connection ", fSyncServer.LOG_INFO);
            return false;
        }
        else
        {

            statusDelegate("User Directory Authorized, Start Send Check", fSyncServer.LOG_INFO);
            generateChecksum(stt.clnt.usrDir, stt.clnt.vers);
            foreach (FileChecksum check in UserChecksum)
            {
                SyncCommand checkCommand = new SyncCommand(SyncCommand.CommandSet.CHECK, check.FileName, check.Checksum);
                SendCommand(stt.workSocket, checkCommand.ToString());
                statusDelegate("Send check Message ", fSyncServer.LOG_INFO);
            }

            SyncCommand endcheck = new SyncCommand(SyncCommand.CommandSet.ENDCHECK);
            SendCommand(stt.workSocket, endcheck.ToString());
            statusDelegate("Send End check Message ", fSyncServer.LOG_INFO);
            return true;
        }
    }


    private static Boolean SendFile(StateObject stt)
    {

        string fileName = stt.cmd.FileName + "_" + stt.clnt.vers.ToString();
        SyncCommand file = new SyncCommand(SyncCommand.CommandSet.FILE, stt.cmd.FileName);

            if (File.Exists(fileName))
            {
                SendCommand(stt.workSocket, file.ToString());
                statusDelegate("Send File Command  ", fSyncServer.LOG_INFO);
             // Send file fileName to remote device
                stt.workSocket.SendFile(fileName);
                statusDelegate("File Sended Succesfully", fSyncServer.LOG_INFO);

                return true;
            }
            else
	        {
                statusDelegate("File doesn't exists  ", fSyncServer.LOG_INFO);
                return false;
	        }
       
    }

    private static Boolean DeleteFile(StateObject stt)
    {
        //Update in the DB all file of the current version with a value equal to current version plus one 
        statusDelegate("DB Updated Correctly, Start Rename Files ", fSyncServer.LOG_INFO);
        generateChecksum(stt.clnt.usrDir, stt.clnt.vers);
        stt.clnt.vers++;
        foreach (FileChecksum check in UserChecksum)
        {
            
            if (File.Exists(check.FileName))
            {
                String file = check.FileName.Remove(check.FileName.Length-3);
                file = file + "_" + stt.clnt.vers;
                File.Move(check.FileName,file);
                statusDelegate("File: "+ check.FileName + "Renamed", fSyncServer.LOG_INFO);
            }
            else statusDelegate("File: " + check.FileName + "Doesn't Exists", fSyncServer.LOG_INFO);
            
        }

        SyncCommand endcheck = new SyncCommand(SyncCommand.CommandSet.ENDCHECK);
        SendCommand(stt.workSocket, endcheck.ToString());
        statusDelegate("Send End check Message ", fSyncServer.LOG_INFO);
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
    private Boolean stopSession()
    {
        return true;
        
    }
    /*
    public static void ReadCallback(IAsyncResult ar) {
        
        // Retrieve the state object and the handler socket
        // from the asynchronous state object.
        StateObject state = (StateObject) ar.AsyncState;
        Socket handler = state.workSocket;

        // Read data from the client socket. 
        int bytesRead = handler.EndReceive(ar);

        if (bytesRead > 0) {
            // There  might be more data, so store the data received so far.
            state.sb.Append(Encoding.ASCII.GetString(
                state.buffer,0,bytesRead));

            // Check for end-of-file tag. If it is not there, read 
            // more data.
            state.commandString = state.sb.ToString();
            if (state.commandString.IndexOf("<EOF>") > -1)
            {
                // All the data has been read from the 
                // client. Display it on the console.
                statusDelegate("Read " + state.commandString.Length + " bytes from socket. \n Data :" + state.commandString, fSyncServer.LOG_INFO);
                // Echo the data back to the client.
               // Send(handler, content);
            } else {
                // Not all data received. Get more.
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
            }
        }
    }
    */

    /*
    private static void Receive(Socket client)
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
    */




 private static void ReceiveFile(Socket client)
    {
        try
        {
            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = client;

            // Begin receiving the data from the remote device.
            client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReceiveFileCallback), state);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }


 private static void ReceiveFileCallback(IAsyncResult ar)
 {
     try
     {
         // Retrieve the state object and the client socket 
         // from the asynchronous state object.
         StateObject state = (StateObject)ar.AsyncState;
         Socket client = state.workSocket;

         string fileName = state.cmd.FileName + "_" + state.clnt.vers.ToString();


         // Delete the file if it exists. 
         if (File.Exists(fileName))
             File.Delete(fileName);

         FileStream fs = File.Create(fileName);

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
             // Signal that all bytes have been received.
             receiveDone.Set();
         }
     }
     catch (Exception e)
     {
         Console.WriteLine(e.ToString());
     }
 }

 private static void SendCommand(Socket handler, String data)
 {
     // Convert the string data to byte data using ASCII encoding.
     byte[] byteData = Encoding.ASCII.GetBytes(data);

     statusDelegate("SendCommand Started", fSyncServer.LOG_INFO);

     // Begin sending the data to the remote device.
     handler.BeginSend(byteData, 0, byteData.Length, 0,
         new AsyncCallback(SendCallback), handler);
 }

    private static void SendCallback(IAsyncResult ar)
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




    private static void ReceiveCommand(Socket client)
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
    private static void ReceiveCallback(IAsyncResult ar)
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
                    command = state.sb.ToString();
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
    
        //Genera il file checksum delle cartelle e ricorsivamente dei file interni
        
        private static void generateChecksum(String dir, int version)
        {
            String pattern= "["+ "_" + version.ToString() + "\\" + "Z" + "]" ;
            string[] fileList = Directory.GetFiles(dir);
            foreach (string filePath in fileList)
            {
                if(Regex.IsMatch(filePath, pattern))
                {
                    UserChecksum.Add(new FileChecksum(filePath));
                }
            }

            // Recurse into subdirectories of this directory.
            string[] subdirectoryList = Directory.GetDirectories(dir);
            foreach (string subdirectoryPath in subdirectoryList)
            {
                generateChecksum(subdirectoryPath, version);
            }
        }

        //Function Start Sync Button
        public void startSync(int port, String workDir)
        {
            //Assign the port value 
            localport = port;
            // Check if the directory is valid
            if (!Directory.Exists(workDir))
            {
                throw new Exception("Directory not exists");
            }
            // Server start
            StartListening();
        }
        //Function Stop Sync Button
        public void stopSync()
        {
            serverStopped = true;
            statusDelegate("Server stopped", fSyncServer.LOG_INFO);
        }
}

}





  