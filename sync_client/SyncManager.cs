using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sync_client
{
    class SyncManager
    {
        private const int START_CMD   = 1;
        private const int EDIT_CMD    = 2;
        private const int DEL_CMD     = 3;
        private const int NEW_CMD     = 4;
        private const int SIZE_CMD    = 5;
        private const int FILE_CMD    = 6;
        private const int RESTORE_CMD = 7;
        private const int GET_CMD     = 8;
        private const int ENDSYNC_CMD = 9;

        private String directory;
        
        public void startSync(String dir);
        public void stopSync();

        private void sendCommand(int command, string param);
        private void sendFile(String path);
        private String getCheckList();
        private String getFile();
    }
}
