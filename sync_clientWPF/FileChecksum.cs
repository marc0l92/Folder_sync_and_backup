using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace sync_clientWPF
{
	class FileChecksum //: IEquatable<FileChecksum>
    {
        private String fileName;
		private String baseFileName;
        private byte[] checksum;

        public FileChecksum(String file, String basePath)
        {
            // Check if the file exists
            if (!File.Exists(file)) {
                throw new Exception("ERROR: file not exists");
            }
			this.fileName = file;
			this.baseFileName = file.Substring(basePath.Length);
            // Generate checksum
            MD5 md5 = MD5.Create();
			Stream fs = File.OpenRead(this.fileName);
            this.checksum = md5.ComputeHash(fs);
        }
		public FileChecksum(String baseFile, byte[] checksum)
		{
			this.baseFileName = baseFile;
			this.checksum = checksum;
		}

		public String Checksum
		{
			get { return System.Text.Encoding.ASCII.GetString(this.checksum); }
			set { this.checksum = System.Text.Encoding.ASCII.GetBytes(value); }
		}
		public byte[] ChecksumBytes
		{
			get { return this.checksum; }
			set { this.checksum = value; }
		}
		public String FileName
		{
			get { return this.fileName; }
			set { this.fileName = value; }
		}
		public String BaseFileName
		{
			get { return this.baseFileName; }
			set { this.baseFileName = value; }
		}
    }
}
