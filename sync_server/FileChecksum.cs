using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace sync_server
{
	public class FileChecksum
	{
		private String fileNameClient;
        private String fileNameServer;
        private String fileNameServerDB;
		private byte[] checksum;

		public FileChecksum(String fileClient , String fileServer, String fileServerDB)
		{
			// Check if the file exists
			if (!File.Exists(fileServer))
			{
				throw new Exception("ERROR: file not exists");
			}
            this.fileNameServer = fileServer;
            this.fileNameServerDB = fileServerDB;
            this.fileNameClient = fileClient;
			// Generate checksum
			MD5 md5 = MD5.Create();
			Stream fs = File.OpenRead(fileServer);
			this.checksum = md5.ComputeHash(fs);
            fs.Close();
		}
        public FileChecksum(String fileServer)
        {
            // Check if the file exists
            if (!File.Exists(fileServer))
            {
                throw new Exception("ERROR: file not exists");
            }
            this.fileNameServer = fileServer;
            // Generate checksum
            MD5 md5 = MD5.Create();
            Stream fs = File.OpenRead(this.fileNameServer);
            this.checksum = md5.ComputeHash(fs);
        }

        public FileChecksum(String fileServer, String fileServerDB, String fileClient, String checksum)
		{
			this.fileNameServer = fileServer;
            this.fileNameClient = fileClient;
            this.fileNameServerDB = fileServerDB;
            this.checksum = System.Text.Encoding.ASCII.GetBytes(checksum);
		}

		public String Checksum
		{
			get { return System.Text.Encoding.ASCII.GetString(this.checksum); }
			set { this.checksum = System.Text.Encoding.ASCII.GetBytes(value); }
		}
		public String FileNameServer
		{
			get { return this.fileNameServer; }
			set { this.fileNameServer = value; }
		}
        public String FileNameServerDB
        {
            get { return this.fileNameServerDB; }
            set { this.fileNameServerDB = value; }
        }
        public String FileNameClient
        {
            get { return this.fileNameClient; }
            set { this.fileNameClient = value; }
        }
	}
}
