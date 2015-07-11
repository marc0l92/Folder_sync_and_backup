using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace sync_server
{
    public class VersCheck
    {
        private String fileNameClient;
        private String fileOperation;

        public VersCheck(String fileClient, String operation)
        {
            
            this.fileOperation = operation;
            this.fileNameClient = fileClient;
        }

        public String FileOperation
        {
            get { return this.fileOperation; }
            set { this.fileOperation = value; }
        }
        public String FileNameClient
        {
            get { return this.fileNameClient; }
            set { this.fileNameClient = value; }
        }
    }
}
