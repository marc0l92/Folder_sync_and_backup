using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace sync_client
{
    class SyncManager
    {
        private const int START_CMD   = 1;
        private const int EDIT_CMD    = 2;
        private const int DEL_CMD     = 3;
        private const int NEW_CMD     = 4;
        private const int RESTORE_CMD = 7;
        private const int GET_CMD     = 8;
        private const int ENDSYNC_CMD = 9;

        private String directory;
        private Thread syncThread;
        
        public void startSync(String dir){
            this.directory = dir;
            // TODO: check directory if is valid
            syncThread = new Thread(new ThreadStart(this.doSync));
            syncThread.IsBackground = true;
            
            // Do the first connection
            
            
            
            
            syncThread.Start();
        }

        public void stopSync(){
            syncThread.Abort();
        }

        private void doSync(){
            // Do syncking
            Thread.Sleep(2000);
        }

        private void sendCommand(int command, string param){
        }
        private void sendFile(String path){
        }
        private String getCheckList(){
            return null;
        }
        private String getFile(){
            return null;
        }
    }
}
