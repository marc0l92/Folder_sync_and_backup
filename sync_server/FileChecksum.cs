using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace sync_server
{
    class FileChecksum : IEquatable<FileChecksum>
    {

         private String _filePath;
        private String _checksum;

        public FileChecksum(String file)
        {
            // Check if the file exists
            if (!File.Exists(file)) {
                throw new Exception("ERROR: file not exists");
            }
            this.filePath = file;
            // Generate checksum
            MD5 md5 = MD5.Create();
            Stream fs = File.OpenRead(this.filePath);
            this.checksum = md5.ComputeHash(fs).ToString();
        }

		public String checksum
		{
			get { return this._checksum; }
			set;
		}
		public String filePath
		{
			get { return this._filePath; }
			set;
		}

		public bool EqualsPath(FileChecksum other){
			return (this.filePath == other.filePath);
		}

        public bool EqualsCheckSum(FileChecksum other)
        {
            return (this.checksum == other.checksum);
        }
    }
}
