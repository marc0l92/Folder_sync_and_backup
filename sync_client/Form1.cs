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
			syncManager.setStatusDelegate((String s) => {lStatus.Text = s;});
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            // start the sync manager
            try
            {
				syncManager.startSync(tAddress.Text, Decimal.ToInt32(nPort.Value), tUsername.Text, tPassword.Text, tDirectory.Text);
                bStart.Enabled = false;
                bStop.Enabled = true;
                bRestore.Enabled = true;
                tDirectory.Enabled = false;
                bBrowse.Enabled = false;
				tUsername.Enabled = false;
				tPassword.Enabled = false;
				tAddress.Enabled = false;
				nPort.Enabled = false;
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
                bRestore.Enabled = false;
                tDirectory.Enabled = true;
                bBrowse.Enabled = true;
				tUsername.Enabled = true;
				tPassword.Enabled = true;
				tAddress.Enabled = true;
				nPort.Enabled = true;
                lStatus.Text = "Stop";
            }
            catch (Exception ex)
            {
                lStatus.Text = ex.Message;
            }
        }

        private void bBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialog.Description = "Select the folder to sync";
            folderBrowserDialog.ShowNewFolderButton = true;
            //folderBrowserDialog.RootFolder = Environment.SpecialFolder.Personal;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
                tDirectory.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void bRestore_Click(object sender, EventArgs e)
        {
            // TODO
		}
    }
}
