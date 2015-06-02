using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using System.Net;


namespace sync_server
{
	class SyncManagerServer
	{
		public delegate void StatusDelegate(String s, int type);
		private StatusDelegate statusDelegate;
		private Thread listenThread;
		private String workDir;
		private bool serverStopped = false;
		private TcpListener tcpServer;   

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
				statusDelegate("Waiting for connections", fSyncServer.LOG_INFO);
				TcpClient client = tcpServer.AcceptTcpClient();
				statusDelegate("Client connected", fSyncServer.LOG_INFO);
				NetworkStream networkStream = client.GetStream();
				byte[] data = new Byte[256];
				String message;
				int i;
				while(client.Connected){
					i = networkStream.Read(data, 0, data.Length);
					message = System.Text.Encoding.ASCII.GetString(data, 0, i);
					statusDelegate(message, fSyncServer.LOG_NORMAL);
				}
			}
		}

	}
}
