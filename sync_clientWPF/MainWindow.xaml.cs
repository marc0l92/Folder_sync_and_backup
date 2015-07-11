using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
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
using System.Xml;


namespace sync_clientWPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		SyncManager syncManager;
		string username, password;
		bool loggedin = false;
		//private delegate void ResetGUI();

		public MainWindow()
		{
			InitializeComponent();

			addVersion("test1", 1, 2, 3);
			addVersion("test2", 2, 3, 4);
			addVersion("test3", 3, 4, 5);

			// load last settings
			//tAddress.Text = ConfigurationManager.AppSettings["address"].ToString();
			//tPort.Text = ConfigurationManager.AppSettings["port"].ToString();

			// initialize my data structure
			syncManager = new SyncManager(tAddress.Text, Convert.ToInt32(tPort.Text));
			syncManager.setStatusDelegate(updateStatus);
		}

		private void StartSync_Click(object sender, EventArgs e)
		{
			// start the sync manager
			try
			{
				bStart.IsEnabled = false;
				syncManager.startSync(tAddress.Text, Convert.ToInt32(tPort.Text), username, password, tDirectory.Text);
				bStop.IsEnabled = true;
				bGetVersions.IsEnabled = true;
				tDirectory.IsEnabled = false;
				bBrowse.IsEnabled = false;
				tAddress.IsEnabled = false;
				tPort.IsEnabled = false;
				updateStatus("Started");
			}
			catch (Exception ex)
			{
				bStart.IsEnabled = true;
				updateStatus(ex.Message);
			}
		}

		private void StopSync_Click(object sender, EventArgs e)
		{
			// stop the sync manager
			try
			{
				updateStatus("Stop");
				forceStop();
			}
			catch (Exception ex)
			{
				bStop.IsEnabled = true;
				updateStatus(ex.Message);
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
			this.Dispatcher.BeginInvoke((Action)(() =>
			{
				updateStatus(message);
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
			bGetVersions.IsEnabled = false;
			tDirectory.IsEnabled = true;
			bBrowse.IsEnabled = true;
			tAddress.IsEnabled = true;
			tPort.IsEnabled = true;
		}

		private void openLogin()
		{
			LoginWindow lw = new LoginWindow();
			bool loginAuthorized = false;
			while (!loginAuthorized)
			{
				lw.showLogin();
				try
				{
					switch (lw.waitResponse())
					{
						case LoginWindow.LoginResponse.CANCEL:
							//System.Windows.Application.Current.Shutdown();
							lw.Close();
							return;
						case LoginWindow.LoginResponse.LOGIN:
							loginAuthorized = syncManager.login(lw.Username, lw.Password);
							if (!loginAuthorized)
							{
								lw.ErrorMessage = "Login faild";
							}
							break;
						case LoginWindow.LoginResponse.REGISTER:
							loginAuthorized = syncManager.login(lw.Username, lw.Password, tDirectory.Text, true);
							if (!loginAuthorized)
							{
								lw.ErrorMessage = "Registration faild";
							}
							break;
						default:
							throw new Exception("Not implemented");
					}
					if (loginAuthorized)
					{
						lUsername.Content = lw.Username;
						bLogInOut.Content = "Logout";
						lw.Close();
						username = lw.Username;
						password = lw.Password;
						bStart.IsEnabled = true;
						loggedin = true;
						updateStatus("Logged in");
					}

				}
				catch (Exception ex)
				{
					lw.ErrorMessage = ex.Message;
					loginAuthorized = false;
				}
			}
		}

		private void addVersion(String version, int newFiles = 0, int editFiles = 0, int delFiles = 0)
		{
			lVersions.Items.Add(new VersionsListViewItem(version, newFiles, editFiles, delFiles));
		}

		private void LogInOut_Click(object sender, RoutedEventArgs e)
		{
			if (loggedin)
			{
				forceStop();
				lUsername.Content = "Please login";
				bLogInOut.Content = "Login";
				bStart.IsEnabled = false;
				loggedin = false;
			}
			this.openLogin();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			// Start the login procedure
			this.Dispatcher.BeginInvoke((Action)(() =>
			{
				// TODO login at startup
				// i have to create the connection in order to perform the login
				openLogin();
			}));
		}

		private void updateStatus(String newStatus)
		{
			lStatus.Content = newStatus;
			ListBoxItem lbi = new ListBoxItem();
			lbi.Content = newStatus;
			lbStatus.Items.Add(lbi);
		}

		private void GetVersions_Click(object sender, RoutedEventArgs e)
		{
			//syncManager.getVersions();
			bRestore.IsEnabled = true;
		}

	}

	class VersionsListViewItem
	{
		public String sVersion { get; set; }
		public String sNewFiles { get; set; }
		public String sEditFiles { get; set; }
		public String sDelFiles { get; set; }
		public String sDateTime { get; set; }
		public VersionsListViewItem(String version, int newFiles, int editFiles, int delFiles)
		{
			sVersion = version;
			sNewFiles = newFiles.ToString();
			sEditFiles = editFiles.ToString();
			sDelFiles = delFiles.ToString();
			sDateTime = DateTime.Now.ToString();
		}
		public VersionsListViewItem(String version, int newFiles, int editFiles, int delFiles, String dateTime)
		{
			sVersion = version;
			sNewFiles = newFiles.ToString();
			sEditFiles = editFiles.ToString();
			sDelFiles = delFiles.ToString();
			sDateTime = dateTime;
		}
	}

}
