using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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
		private SyncManager syncManager;
		List<Version> versions = null;
		private bool loggedin = false;
		private NotifyIcon notifyIcon;
		private System.Windows.Forms.ContextMenu notifyIconMenu;
		private SettingsManager settingsManager;


		public MainWindow()
		{
			InitializeComponent();

			// initialize my data structure
			syncManager = new SyncManager();
			syncManager.setStatusDelegate(updateStatus, updateStatusBar);

			//initialize tray icon
			notifyIcon = new NotifyIcon();
			Stream iconStream = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/sync_clientWPF;component/Synchronize.ico")).Stream;
			notifyIcon.Icon = new System.Drawing.Icon(iconStream);
			notifyIconMenu = new System.Windows.Forms.ContextMenu();
			System.Windows.Forms.MenuItem mnuItemSyncNow = new System.Windows.Forms.MenuItem();
			System.Windows.Forms.MenuItem mnuItemShow = new System.Windows.Forms.MenuItem();
			mnuItemShow.Text = "Show";
			mnuItemShow.Click += new System.EventHandler(notifyIcon_Click);
			notifyIconMenu.MenuItems.Add(mnuItemShow);
			mnuItemSyncNow.Text = "SyncNow";
			mnuItemSyncNow.Click += new System.EventHandler(syncMenuItem);
			notifyIconMenu.MenuItems.Add(mnuItemSyncNow);
			notifyIcon.Text = "SyncClient";
			notifyIcon.ContextMenu = notifyIconMenu;
			notifyIcon.BalloonTipTitle = "App minimized to tray";
			notifyIcon.BalloonTipText = "Sync sill running.";
			//notifyIcon.Visible = true;

			// Settings manager
			settingsManager = new SettingsManager();
			tAddress.Text = settingsManager.readSetting("connection", "address");
			tPort.Text = settingsManager.readSetting("connection", "port");
			tDirectory.Text = settingsManager.readSetting("connection", "directory");
			tTimeout.Text = settingsManager.readSetting("connection", "syncTime");
		}
		private void syncMenuItem(object sender, System.EventArgs e)
		{
			syncManager.forceSync();
			//this.Hide();
		}
		private void StartSync_Click(object sender, EventArgs e)
		{
			// start the sync manager
			try
			{
				bStart.IsEnabled = false;
				lVersions.Items.Clear();
				syncManager.startSync(tAddress.Text, Int32.Parse(tPort.Text), tDirectory.Text, Int32.Parse(tTimeout.Text) * 1000);
				bStop.IsEnabled = true;
				bSyncNow.IsEnabled = true;
				bGetVersions.IsEnabled = true;
				tDirectory.IsEnabled = false;
				tTimeout.IsEnabled = false;
				bBrowse.IsEnabled = false;
				tAddress.IsEnabled = false;
				tPort.IsEnabled = false;
				updateStatus("Started");
				// Save settings
				settingsManager.writeSetting("connection", "address", tAddress.Text);
				settingsManager.writeSetting("connection", "port", tPort.Text);
				settingsManager.writeSetting("connection", "directory", tDirectory.Text);
				settingsManager.writeSetting("connection", "syncTime", tTimeout.Text);
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
				lVersions.Items.Clear();
				updateStatus("Stop");
				forceStop();
			}
			catch (Exception ex)
			{
				bStop.IsEnabled = true;
				bSyncNow.IsEnabled = true;
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
		private void updateStatusBar(int percentage)
		{
			this.Dispatcher.BeginInvoke((Action)(() =>
			{
				lStatusBar.Value = percentage;
			}));
		}

		private void forceStop()
		{
			bStop.IsEnabled = false;
			bSyncNow.IsEnabled = false;
			bRestore.IsEnabled = false;
			bGetVersions.IsEnabled = false;
			syncManager.stopSync();
			bStart.IsEnabled = true;
			tDirectory.IsEnabled = true;
			tTimeout.IsEnabled = true;
			bBrowse.IsEnabled = true;
			tAddress.IsEnabled = true;
			tPort.IsEnabled = true;
		}

		private async void openLogin()
		{
			LoginWindow lw = new LoginWindow();
			bool loginAuthorized = false;
			bLogInOut.IsEnabled = false;
			lw.Username = settingsManager.readSetting("account", "username");
			lw.Username = settingsManager.readSetting("account", "password");
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
							bLogInOut.IsEnabled = true;
							return;
						case LoginWindow.LoginResponse.LOGIN:
							loginAuthorized = await syncManager.login(tAddress.Text, Convert.ToInt32(tPort.Text), lw.Username, lw.Password);
							if (!loginAuthorized)
							{
								lw.ErrorMessage = "Login faild";
							}
							break;
						case LoginWindow.LoginResponse.REGISTER:
							loginAuthorized = await syncManager.login(tAddress.Text, Convert.ToInt32(tPort.Text), lw.Username, lw.Password, tDirectory.Text, true);
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
						settingsManager.writeSetting("account", "username", lw.Username);
						if(lw.KeepLoggedIn)
							settingsManager.writeSetting("account", "password", lw.Username);
						else
							settingsManager.writeSetting("account", "password", "");
						bStart.IsEnabled = true;
						loggedin = true;
						updateStatus("Logged in");
						StartSync_Click(null, null); // start sync
					}
				}
				catch (Exception ex)
				{
					lw.ErrorMessage = ex.Message;
					loginAuthorized = false;
				}
			}
			bLogInOut.IsEnabled = true;
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
			lVersions.Items.Clear();
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

		private async void GetVersions_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				bGetVersions.IsEnabled = false;
				lVersions.Items.Clear();
				versions = await syncManager.getVersions();
				foreach (Version version in versions)
				{
					lVersions.Items.Add(new VersionsListViewItem(version.VersionNum, version.NewFiles, version.EditFiles, version.DelFiles, version.Timestamp));
				}
				lVersions.SelectedIndex = 0;

				bGetVersions.IsEnabled = true;
				bRestore.IsEnabled = true;
			}
			catch (Exception ex)
			{
				bGetVersions.IsEnabled = true;
				updateStatus(ex.Message);
			}
		}

		private void lVersions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			DependencyObject obj = (DependencyObject)e.OriginalSource;
			while (obj != null && obj != lVersions)
			{
				if (obj.GetType() == typeof(System.Windows.Controls.ListViewItem))
				{
					VersionDetailsWindow vdw = new VersionDetailsWindow(versions[lVersions.SelectedIndex], syncManager);
					vdw.Show();
					break;
				}
				obj = VisualTreeHelper.GetParent(obj);
			}
		}

		private async void Restore_Click(object sender, EventArgs e)
		{
			bRestore.IsEnabled = false;
			Int64 selVersion = versions[lVersions.SelectedIndex].VersionNum;
			MessageBoxResult res = System.Windows.MessageBox.Show("Do you want to restore version number " + selVersion + " ?", "Restore system", System.Windows.MessageBoxButton.YesNo);
			if (res == MessageBoxResult.Yes)
			{
				try
				{
					await syncManager.restoreVersion(selVersion);
					System.Windows.MessageBox.Show("Restore Done!", "Restoring system");
				}
				catch (Exception ex)
				{
					System.Windows.MessageBox.Show("Restore failed:\n" + ex.Message, "Restoring system", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
			bRestore.IsEnabled = true;
		}

		private void bSyncNow_Click(object sender, RoutedEventArgs e)
		{
			syncManager.forceSync();
		}

		private void Window_StateChanged(object sender, EventArgs e)
		{
			switch (this.WindowState)
			{
				case WindowState.Maximized:
					break;
				case WindowState.Minimized:
					// Do your stuff
					notifyIcon.Visible = true;
					notifyIcon.ShowBalloonTip(1000);
					this.Hide();
					break;
				case WindowState.Normal:

					break;
			}
		}

		private void notifyIcon_Click(object o, EventArgs ea)
		{
			this.Show();

			// this.Activate();
			//this.Focus(); // todo focus

			this.WindowState = WindowState.Normal;
			this.Activate();
			notifyIcon.Visible = false;
		}

	}

	class VersionsListViewItem
	{
		public String sVersion { get; set; }
		public String sNewFiles { get; set; }
		public String sEditFiles { get; set; }
		public String sDelFiles { get; set; }
		public String sDateTime { get; set; }
		public VersionsListViewItem(Int64 version, int newFiles, int editFiles, int delFiles, String dateTime)
		{
			sVersion = version.ToString();
			sNewFiles = newFiles.ToString();
			sEditFiles = editFiles.ToString();
			sDelFiles = delFiles.ToString();
			sDateTime = dateTime;
		}
	}

}