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
		private IPAddress localAddr = IPAddress.Loopback;
        private String defaultDir;

        public delegate void StatusDelegate(String s, int type);
        private delegate void EndClientDelegate();
        private EndClientDelegate endClientDelegate = null;
        private StatusDelegate statusDelegate;
		private Socket listener;

        public ManualResetEvent allDone = new ManualResetEvent(false);
        private bool serverStopped = false;

        public void setStatusDelegate(StatusDelegate sd)
        {
            statusDelegate = sd;
        }

        public AsyncManagerServer()
        {
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
            defaultDir = workDir;
            Thread listeningThread = new Thread(new ThreadStart(StartListening));
            listeningThread.IsBackground = true;
            listeningThread.Start();
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
				ClientManager client = new ClientManager(handler, defaultDir);
				client.setStatusDelegate(statusDelegate);
				if (endClientDelegate == null)
					endClientDelegate = new EndClientDelegate(client.stop);
				else
					endClientDelegate += new EndClientDelegate(client.stop);

				statusDelegate("Connected and Created New Thred to Serve Client", fSyncServer.LOG_INFO);
			}
        }

        //Function Stop Sync Button
        public void stopSync()
        {
			if (endClientDelegate != null)
				endClientDelegate();
			serverStopped = true;
			listener.Close();
            statusDelegate("Server stopped", fSyncServer.LOG_INFO);
        }
    }
}
