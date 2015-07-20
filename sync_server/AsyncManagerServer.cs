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

    public class AsyncManagerServer
    {
        // Thread signal.
        private int localport;
        //private IPAddress localAddr = IPAddress.Parse("192.168.1.130");
		//private IPAddress localAddr = IPAddress.Parse("127.0.0.1");
		//private IPAddress localAddr = IPAddress.Loopback;
        private IPAddress localAddr = IPAddress.Any;
        private String defaultDir;

        public delegate void StatusDelegate(String s, int type);
        public delegate void NumberDelegate( int nclient);
        private StatusDelegate statusDelegate;
        private static NumberDelegate numberDelegate;
		private Socket listener;
        private static int clientNumber = 0;
        private int defaultMaxVers;
        private Thread listeningThread= null;
        private List<ClientManager> clients;

        public ManualResetEvent allDone = new ManualResetEvent(false);
        private bool serverStopped = false;

        public void setDelegate(StatusDelegate sd, NumberDelegate nd)
        {
            statusDelegate = sd;
            numberDelegate = nd;
        }
        public AsyncManagerServer()
        {
        }

        //Function Start Sync Button
        public void startSync(int port, String workDir, int maxVers)
        {
            //Assign the port value 
            localport = port;
            // Check if the directory is valid
            if (!Directory.Exists(workDir))
            {
                throw new Exception("Directory not exists");
            }
            // Server start
            defaultDir = workDir;
            defaultMaxVers = maxVers;
            listeningThread = new Thread(new ThreadStart(StartListening));
            listeningThread.IsBackground = true;
            listeningThread.Start();
        }
        static public void PrintClient()
        {
            numberDelegate(clientNumber);
        }

        static public void IncreaseClient()
        {
            clientNumber++;
        }
        static public void DecreaseClient()
        {
            clientNumber--;
        }

        public void StartListening()
        {
            // Data buffer for incoming data.
            byte[] bytes = new Byte[1024];
            // Establish the local endpoint for the socket.
            // The DNS name of the computer
            // running the listener is "host.contoso.com".
            IPEndPoint localEndPoint = new IPEndPoint(localAddr, localport);

            // Create a TCP/IP socket.
            listener = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                statusDelegate("Start Listening on Port: " + localport + "; Address: " + localAddr, fSyncServer.LOG_INFO);
                while (!serverStopped)
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);
					// Wait until a connection is made before continuing.
					allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                statusDelegate("Connection Error Exception:" + e.ToString(), fSyncServer.LOG_INFO);
            }

        }

        public void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();
            // Get the socket that handles the client request.
			if (!serverStopped)
			{

                // Retrieve the socket from the state object.
                 //   Socket client = (Socket) ar.AsyncState;
				//
                Socket handler = listener.EndAccept(ar);
                //Socket handler = (Socket)ar.AsyncState;
                //handler.EndAccept(ar);
				// Create the state object.
				//StateObject state = new StateObject();
				//state.workSocket = handler;
				// handler.BeginReceive( state.buffer, 0, StateObject.BufferSize, 0,
				//   new AsyncCallback(ReadCallback), state);
                if(defaultDir[defaultDir.Length-1]=='\\')
                {
                    defaultDir=defaultDir.Substring(0, defaultDir.Length - 1);
                }
				ClientManager client = new ClientManager(handler, defaultDir, defaultMaxVers, statusDelegate); //TODO

                AsyncManagerServer.IncreaseClient();
                AsyncManagerServer.PrintClient();
                //if (endClientDelegate == null)
                //    endClientDelegate = new EndClientDelegate(client.stop);
                //else
                //    endClientDelegate += new EndClientDelegate(client.stop);

                //if (clients == null)
                //{
                //    clients = new List<ClientManager>();
                //    clients.Add(client);
                //}
                //else
                //    clients.Add(client);

				statusDelegate("Connected and Created New Thred to Serve Client", fSyncServer.LOG_INFO);
			}
        }


        //Function Stop Sync Button
        public void stopSync()
        {
            //if (endClientDelegate != null)
            //    endClientDelegate();
            //if (clients != null)
            //{
            //    foreach (ClientManager client in clients)
            //    {
            //        client.WellStop();
            //    }
            //}
			serverStopped = true;
			listener.Close();
           // listeningThread.Join();
            statusDelegate("Server stopped", fSyncServer.LOG_INFO);
        }
    }
}
