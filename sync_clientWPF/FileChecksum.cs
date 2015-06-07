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
        private String checksum;

        public FileChecksum(String file)
        {
            // Check if the file exists
            if (!File.Exists(file)) {
                throw new Exception("ERROR: file not exists");
            }
			this.fileName = file;
            // Generate checksum
            MD5 md5 = MD5.Create();
			Stream fs = File.OpenRead(this.fileName);
            this.checksum = md5.ComputeHash(fs).ToString();
        }
		public FileChecksum(String file, String checksum)
		{
			this.fileName = file;
			this.checksum = checksum;
		}

		public String Checksum
		{
			get { return this.checksum; }
			set { this.checksum = value; }
		}
		public String FileName
		{
			get { return this.fileName; }
			set { this.fileName = value; }
		}
    }
}
