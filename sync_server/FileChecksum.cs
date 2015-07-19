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
		private string fileNameClient;
		private string fileNameServer;
		private string fileNameServerDB;
		private byte[] checksum;
		private string timestamp="";

		public FileChecksum(string fileClient, string fileServer, string fileServerDB)
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
		public FileChecksum(string fileServer)
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

        public FileChecksum()
        {
        
        }

        public FileChecksum(string fileServer, string fileServerDB, string fileClient, byte[] checksum, string timestamp ="")
		{
			this.fileNameServer = fileServer;
            this.fileNameClient = fileClient;
            this.fileNameServerDB = fileServerDB;
            //this.checksum = System.Text.Encoding.ASCII.GetBytes(checksum);
			this.checksum = checksum;
			this.timestamp = timestamp;
		}

		public string Checksum
		{
			get { return System.Text.Encoding.ASCII.GetString(this.checksum); }
			set { this.checksum = System.Text.Encoding.ASCII.GetBytes(value); }
		}
		public byte[] ChecksumBytes
		{
			get { return this.checksum; }
			set { this.checksum = value; }
		}
		public string FileNameServer
		{
			get { return this.fileNameServer; }
			set { this.fileNameServer = value; }
		}
        public string FileNameServerDB
        {
            get { return this.fileNameServerDB; }
            set { this.fileNameServerDB = value; }
        }
        public string FileNameClient
        {
            get { return this.fileNameClient; }
            set { this.fileNameClient = value; }
        }
		public string Timestamp
		{
			get { return timestamp; }
			set { timestamp = value; }
		}
	}
}
