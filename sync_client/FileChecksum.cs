using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace sync_client
{
    class FileChecksum
    {
        private String filePath;
        private String checksum;

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
    }
}
