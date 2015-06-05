using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sync_server
{
    class SyncClient
    {
        public String usrDir { set { usrDir = value; } get { return usrDir; } }
        public String usrNam { set { usrNam = value; } get { return usrNam; } }
        public String usrPwd { set { usrPwd = value; } get { return usrPwd; } }
        public List<FileChecksum> usrFileChecksum { set { usrFileChecksum = value; } get { return usrFileChecksum; } }
        public int vers { set { vers = value; } get { return vers; } }
    }
}
