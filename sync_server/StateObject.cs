using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace sync_server
{
    public class StateObject
    {
        // Connection info
        public Socket workSocket = null;
        public const int BUFFER_SIZE = 1024;
		public byte[] buffer = new byte[BUFFER_SIZE];
        public StringBuilder sb = new StringBuilder();

		// Client info
		public String directory = "";
		public String username = "NOACTVIVE";
		public String password = "";
		public Int64 userID = -1;
		public Int64 version = -1;
    }
}
