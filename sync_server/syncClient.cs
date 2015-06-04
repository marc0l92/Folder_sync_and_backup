using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sync_server
{
    class syncClient
    {
        private String usrDir { set; get; }
        private String usrNam { set; get; }
        private String usrPwd { set; get; }
        private List<FileChecksum> usrFileChecksum { set; get; }
        private int vers { set; get; }
        private Boolean syncEnd { set; get; }
    }
}
