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
		private const int SOCKET_QUEUE_LENGTH = 100;
		public delegate void StatusDelegate(String s, int type);
		public delegate void NumberDelegate(int nclient);
		private int localport;
		private IPAddress localAddr;
		private String defaultDir;
		private StatusDelegate statusDelegate;
		private static NumberDelegate numberDelegate;
		private static int clientNumber = 0;
		private Socket listener;
		private int defaultMaxVers;
		private Thread listeningThread;
		private List<ClientManager> clients;
		private bool serverStopped;

		public AsyncManagerServer(StatusDelegate sd, NumberDelegate nd)
		{
			statusDelegate = sd;
			numberDelegate = nd;
			clients = new List<ClientManager>();
			localAddr = IPAddress.Any;
		}

		// Function Start Sync Button
		public void startSync(int port, String workDir, int maxVers)
		{
			// Check if the directory is valid
			if (!Directory.Exists(workDir))
			{
				throw new Exception("Directory not exists");
			}
			defaultDir = workDir;
			if (defaultDir[defaultDir.Length - 1] == '\\')
			{
				defaultDir = defaultDir.Substring(0, defaultDir.Length - 1);
			}
			defaultMaxVers = maxVers;
			localport = port;
			// Server start
			serverStopped = false;
			listeningThread = new Thread(new ThreadStart(StartListening));
			listeningThread.IsBackground = true;
			listeningThread.Start();
		}

		public void StartListening()
		{
			try
			{
				// Establish the local endpoint for the socket.
				IPEndPoint localEndPoint = new IPEndPoint(localAddr, localport);
				// Create a TCP/IP socket.
				listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				// Bind the socket to the local endpoint and listen for incoming connections.
				listener.Bind(localEndPoint);
				listener.Listen(SOCKET_QUEUE_LENGTH);

				statusDelegate("Start Listening on Port: " + localport + "; Address: " + localAddr, fSyncServer.LOG_INFO);
				while (!serverStopped)
				{
					// Start an synchronous socket to listen for connections.
					Socket handler = listener.Accept();
					ClientManager client = new ClientManager(handler, defaultDir, defaultMaxVers, statusDelegate);
					AsyncManagerServer.IncreaseClient();
					clients.Add(client);

					statusDelegate("Connected and Created New Thred to Serve Client", fSyncServer.LOG_INFO);
				}
			}
			catch (Exception e)
			{
				statusDelegate("Connection Error Exception:" + e.ToString(), fSyncServer.LOG_INFO);
			}
			finally
			{
				// Close socket and clients
				if (listener.Connected) listener.Close();
				foreach (ClientManager client in clients)
				{
					client.WellStop();
				}
			}
		}

		// Function Stop Sync Button
		public void stopSync()
		{
			serverStopped = true;
			listener.Close();
			listeningThread.Join();
			statusDelegate("Server stopped", fSyncServer.LOG_INFO);
		}

		// Manage connected user count
		static public void IncreaseClient()
		{
			clientNumber++;
			numberDelegate(clientNumber);
		}
		static public void DecreaseClient()
		{
			clientNumber--;
			numberDelegate(clientNumber);
		}
	}
}
