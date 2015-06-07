using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace sync_clientWPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		SyncManager syncManager;
		//private delegate void ResetGUI();

		public MainWindow()
		{
			InitializeComponent();
		
			addVersion("test1", 1, 2, 3);
			addVersion("test2", 2, 3, 4);
			addVersion("test3", 3, 4, 5);

            // initialize my data structure
            syncManager = new SyncManager();
			syncManager.setStatusDelegate(updateStatus);
			
			// start the login procedure 
			// TODO
			//this.Dispatcher.BeginInvoke((Action)(() => {
			//	openLogin();
			//}));
        }

		private void StartSync_Click(object sender, EventArgs e)
        {
            // start the sync manager
            try
            {
				bStart.IsEnabled = false;
				syncManager.startSync(tAddress.Text, Convert.ToInt32(tPort.Text), tUsername.Text, tPassword.Password, tDirectory.Text);
				bStop.IsEnabled = true;
				bRestore.IsEnabled = true;
				tDirectory.IsEnabled = false;
                bBrowse.IsEnabled = false;
				tUsername.IsEnabled = false;
				tPassword.IsEnabled = false;
				tAddress.IsEnabled = false;
				tPort.IsEnabled = false;
				lStatus.Content = "Started";
            }
            catch (Exception ex)
            {
				bStart.IsEnabled = true;
				lStatus.Content = ex.Message;
            }
        }

        private void StopSync_Click(object sender, EventArgs e)
        {
            // stop the sync manager
            try
            {
				lStatus.Content = "Stop";
				forceStop();
            }
            catch (Exception ex)
            {
				bStop.IsEnabled = true;
				lStatus.Content = ex.Message;
            }
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select the folder to sync";
            folderBrowserDialog.ShowNewFolderButton = true;
            //folderBrowserDialog.RootFolder = Environment.SpecialFolder.Personal;
			if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
                tDirectory.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void Restore_Click(object sender, EventArgs e)
        {
			String selVersion = lVersions.SelectedItems[0].ToString();
			MessageBoxResult res = System.Windows.MessageBox.Show("Do you want to restore this version?\n" + selVersion, "Restore system", System.Windows.MessageBoxButton.YesNo);
			if (res == MessageBoxResult.Yes)
			{
				try
				{
					syncManager.restoreVersion(selVersion);
					System.Windows.MessageBox.Show("Restore Done!", "Restoring system");
				}
				catch (Exception ex)
				{
					System.Windows.MessageBox.Show("Restore failed\n" + ex.Message, "Restoring system", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		private void updateStatus(String message, bool fatalError)
		{
			this.Dispatcher.BeginInvoke((Action)(() => {
				lStatus.Content = message;
				if (fatalError)
				{
					forceStop();
				}
			}));
		}

		private void forceStop()
		{
			bStop.IsEnabled = false;
			syncManager.stopSync();
			bStart.IsEnabled = true;
			bStop.IsEnabled = false;
			bRestore.IsEnabled = false;
			tDirectory.IsEnabled = true;
			bBrowse.IsEnabled = true;
			tUsername.IsEnabled = true;
			tPassword.IsEnabled = true;
			tAddress.IsEnabled = true;
			tPort.IsEnabled = true;
		}

		private void openLogin()
		{
			LoginWindow lw = new LoginWindow();
			lw.showLogin();
			switch (lw.waitResponse())
			{
				case LoginWindow.LoginResponse.CANCEL:
					System.Windows.Application.Current.Shutdown();
					break;
				case LoginWindow.LoginResponse.LOGIN:
					syncManager.login(lw.Username, lw.Password);
					break;
				case LoginWindow.LoginResponse.REGISTER:
					syncManager.login(lw.Username, lw.Password, true);
					break;
				default:
					throw new Exception("Not implemented");
			}
		}

		private void addVersion(String version, int newFiles = 0, int editFiles = 0, int delFiles = 0)
		{
			// TODO add an item
			//lVersions.Items.Add(new String[]{version, newFiles.ToString(), editFiles.ToString(), delFiles.ToString(), DateTime.Now.ToString()});
			//lVersions.Items.Add(new RestoreListViewItem());
		}

		private void LogOut_Click(object sender, RoutedEventArgs e)
		{
			this.openLogin();
		}
	}
}
