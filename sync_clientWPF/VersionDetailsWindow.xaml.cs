using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace sync_clientWPF
{
	/// <summary>
	/// Interaction logic for VersionDetailsWindow.xaml
	/// </summary>
	partial class VersionDetailsWindow : Window
	{
		private SyncManager syncManager;
		private Version version;
		private string selectedFileName;
		public VersionDetailsWindow(Version v, SyncManager sm)
		{
			InitializeComponent();
			this.version = v;
			syncManager = sm;
			this.Title = "Version details: " + version.VersionNum;

			foreach (VersionFile vf in version.Items)
			{

				lDetails.Items.Add(new VersionListViewItem(vf.FileName, vf.FileOperation));
			}
		}

		private void lDetails_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			DependencyObject obj = (DependencyObject)e.OriginalSource;
			while (obj != null && obj != lDetails)
			{
				if (obj.GetType() == typeof(System.Windows.Controls.ListViewItem))
				{
					selectedFileName = ((VersionListViewItem)lDetails.SelectedItem).sFilename;
					List<VersionFile> versions = syncManager.getFileVersions(selectedFileName);
					foreach (VersionFile vf in versions)
					{
						lFileVersions.Items.Add(new FileVersionListViewItem(vf.VersionNum.ToString(), vf.FileOperation, vf.Timestamp));
					}
					break;
				}
				obj = VisualTreeHelper.GetParent(obj);
			}
		}

		private void lFileVersions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			DependencyObject obj = (DependencyObject)e.OriginalSource;
			while (obj != null && obj != lDetails)
			{
				if (obj.GetType() == typeof(System.Windows.Controls.ListViewItem))
				{
					Int64 selectedVersion = Int64.Parse(((FileVersionListViewItem)lFileVersions.SelectedItem).sVersion);
					MessageBoxResult res = System.Windows.MessageBox.Show("Do you want to restore file \"" + selectedFileName + "\" with version number " + selectedVersion + " ?", "Restore system", System.Windows.MessageBoxButton.YesNo);
					if (res == MessageBoxResult.Yes)
					{
						try
						{
							syncManager.restoreFileVersion(selectedFileName, selectedVersion);
							//System.Windows.MessageBox.Show("Restore Done!", "Restoring system");
						}
						catch (Exception ex)
						{
							System.Windows.MessageBox.Show("Restore failed\n" + ex.Message, "Restoring system", MessageBoxButton.OK, MessageBoxImage.Error);
						}
					}
					break;
				}
				obj = VisualTreeHelper.GetParent(obj);
			}
		}
	}

	class VersionListViewItem
	{
		public string sFilename { get; set; }
		public string sOperation { get; set; }
		public VersionListViewItem(string filename, string operation)
		{
			sFilename = filename;
			sOperation = operation;
		}
	}
	class FileVersionListViewItem
	{
		public string sVersion { get; set; }
		public string sOperation { get; set; }
		public string sTimestamp { get; set; }
		public FileVersionListViewItem(string version, string operation, string timestamp)
		{
			sVersion = version;
			sOperation = operation;
			sTimestamp = timestamp;
		}
	}
}
