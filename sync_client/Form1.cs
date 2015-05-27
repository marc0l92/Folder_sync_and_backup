using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sync_client
{
    public partial class fSync : Form
    {
        SyncManager syncManager;

        public fSync()
        {
            InitializeComponent();

            // initialize my data structure
            syncManager = new SyncManager();
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            // start the sync manager
            try
            {
                syncManager.startSync(tDirectory.Text);
                bStart.Enabled = false;
                bStop.Enabled = true;
                lStatus.Text = "Started";
            }
            catch (Exception ex)
            {
                lStatus.Text = ex.Message;
            }
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            // stop the sync manager
            try
            {
                syncManager.stopSync();
                bStart.Enabled = true;
                bStop.Enabled = false;
                lStatus.Text = "Stop";
            }
            catch (Exception ex)
            {
                lStatus.Text = ex.Message;
            }
        }
    }
}
