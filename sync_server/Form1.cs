using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sync_server
{
    public partial class fSyncServer : Form
    {
		public const int LOG_NORMAL  = 0;
		public const int LOG_INFO    = 1;
		public const int LOG_WARNING = 2;
		public const int LOG_ERROR   = 3;

		private delegate void AppendItem(String s);
		private SyncManagerServer syncManager;

        public fSyncServer()
        {
            InitializeComponent();
			syncManager = new SyncManagerServer();
			syncManager.setStatusDelegate(appendStatus);
		}

		private void bBrowse_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			folderBrowserDialog.Description = "Select the server working directory";
			folderBrowserDialog.ShowNewFolderButton = true;
			//folderBrowserDialog.RootFolder = Environment.SpecialFolder.Personal;
			if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				tDirectory.Text = folderBrowserDialog.SelectedPath;
			}
		}

		private void bStart_Click(object sender, EventArgs e)
		{
			try
			{
				syncManager.startSync(Decimal.ToInt32(nPort.Value), tDirectory.Text);
				tDirectory.Enabled = false;
				nPort.Enabled = false;
				bStart.Enabled = false;
				bStop.Enabled = true;
			}
			catch (Exception ex)
			{
				this.appendStatus(ex.Message, LOG_ERROR);
			}
		}

		private void bStop_Click(object sender, EventArgs e)
		{
			syncManager.stopSync();
			tDirectory.Enabled = true;
			nPort.Enabled = true;
			bStart.Enabled = true;
			bStop.Enabled = false;
		}

		
		private void appendStatus(String s, int type = LOG_NORMAL)
		{
			switch (type)
			{
				case LOG_INFO:
					s = "INFO: "+s;
					break;
				case LOG_WARNING:
					s = "WARNING: "+s;
					break;
				case LOG_ERROR:
					s = "ERROR: " + s;
					break;
			}

			// Send the command to add a new item on the listbox on the UI thread
			lbLog.BeginInvoke(new AppendItem((String str) => { lbLog.Items.Add(str); }), new object[] { s });
		}
    }
}
