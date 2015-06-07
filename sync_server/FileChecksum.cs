using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace sync_server
{
	class FileChecksum
	{
		private String fileNameClient;
        private String fileNameServer;
		private String checksum;

		public FileChecksum(String fileClient , String fileServer)
		{
			// Check if the file exists
			if (!File.Exists(fileServer))
			{
				throw new Exception("ERROR: file not exists");
			}
            this.fileNameServer = fileServer;
            this.fileNameClient = fileClient;
			// Generate checksum
			MD5 md5 = MD5.Create();
			Stream fs = File.OpenRead(this.fileNameServer);
			this.checksum = md5.ComputeHash(fs).ToString();
		}
        public FileChecksum(String fileS, String fileC, String checksum)
		{
			this.fileNameServer = fileS;
            this.fileNameClient = fileC;
			this.checksum = checksum;
		}

		public String Checksum
		{
			get { return this.checksum; }
			set { this.checksum = value; }
		}
		public String FileNameServer
		{
			get { return this.fileNameServer; }
			set { this.fileNameServer = value; }
		}
        public String FileNameClient
        {
            get { return this.fileNameClient; }
            set { this.fileNameClient = value; }
        }
	}
}
