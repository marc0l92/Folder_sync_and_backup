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
		private delegate void ResetGUI();

        public fSync()
        {
            InitializeComponent();

			addVersion("test1", 1, 2, 3);
			addVersion("test2", 2, 3, 4);
			addVersion("test3", 3, 4, 5);

            // initialize my data structure
            syncManager = new SyncManager();
			syncManager.setStatusDelegate(updateStatus);
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            // start the sync manager
            try
            {
				bStart.Enabled = false;
				syncManager.startSync(tAddress.Text, Decimal.ToInt32(nPort.Value), tUsername.Text, tPassword.Text, tDirectory.Text);
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
				bStart.Enabled = true;
                lStatus.Text = ex.Message;
            }
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            // stop the sync manager
            try
            {
                lStatus.Text = "Stop";
				forceStop();
            }
            catch (Exception ex)
            {
				bStop.Enabled = true;
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
			String selVersion = lVersions.SelectedItems[0].Text;
			DialogResult res = MessageBox.Show("Do you want to restore this version?\n" + selVersion, "Restore system", MessageBoxButtons.YesNo);
			if(res == DialogResult.Yes){
				try
				{
					syncManager.restoreVersion(selVersion);
					MessageBox.Show("Restore Done!", "Restoring system");
				}
				catch (Exception ex)
				{
					MessageBox.Show("Restore failed\n"+ex.Message, "Restoring system", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void updateStatus(String message, bool fatalError)
		{
			lStatus.Text = message;
			if (fatalError)
			{
				this.BeginInvoke(new ResetGUI(forceStop));
			}
		}

		private void forceStop()
		{
			bStop.Enabled = false;
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
		}

		private void addVersion(String version, int newFiles = 0, int editFiles = 0, int delFiles = 0)
		{
			lVersions.Items.Add(new ListViewItem(new String[]{version, newFiles.ToString(), editFiles.ToString(), delFiles.ToString(), DateTime.Now.ToString()}));
		}
    }
}
