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
    public SyncClient client = new SyncClient();
    // Served Client
    public Boolean syncEnd = false;

    public Boolean wellEnd = false;

    public List<FileChecksum> userChecksum;

}

public class AsyncManagerServer {
    // Thread signal.
    private static int localport; 
    private static IPAddress localAddr = IPAddress.Parse("127.0.0.1");

    public delegate void StatusDelegate(String s, int type);
    private static StatusDelegate statusDelegate;

    public static ManualResetEvent allDone = new ManualResetEvent(false);
    private static bool serverStopped = false;

    // The response from the remote device.
    private static String command = String.Empty;

    public void setStatusDelegate(StatusDelegate sd)
    {
        statusDelegate = sd;
    }
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
            NetworkOperation.ReceiveCommand(state.workSocket);
           // receiveDone.WaitOne();
            state.cmd = SyncCommand.convertFromString(command);
            if (NetworkOperation.doCommand(state))
                statusDelegate("Slave Thread Done Command Successfully ", fSyncServer.LOG_INFO);
            else
                statusDelegate("Slave Thread Done Command with no Success", fSyncServer.LOG_INFO);

        }
        if(!state.wellEnd)
        { 
            // todo IMPLEMENTARE RESTORE A VERSIONE PRECEDENTE
        }
        //See if necessary close connection
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
    
        //Genera il file checksum delle cartelle e ricorsivamente dei file interni
        // todo funzione che ritorna la lista di checksum

    /*
    private static Boolean RenameFiles(String dir, String file, int version)
    {
        foreach (FileChecksum check in UserChecksum)
        {
            if (String.Compare(check.FileName, file + "_" + (version - 1)) != 0)
            {
                if (File.Exists(check.FileName))
                {
                    String fil = check.FileName.Remove(check.FileName.Length - 3);
                    fil = fil + "_" + version;
                    File.Move(check.FileName, file);
                    statusDelegate("File: " + check.FileName + "Renamed", fSyncServer.LOG_INFO);
                }
                else statusDelegate("File: " + check.FileName + "Doesn't Exists", fSyncServer.LOG_INFO);
            }
        }
        return true;

    }*/
       /* private static void generateChecksum(String dir, int version)
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
        }*/

}

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