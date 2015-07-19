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
		private Version version;
		public VersionDetailsWindow(Version v)
		{
			InitializeComponent();
			this.version = v;
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
					//lDetails.SelectedIndex;
					
					break;
				}
				obj = VisualTreeHelper.GetParent(obj);
			}
		}
	}

	class VersionListViewItem
	{
		public String sFilename { get; set; }
		public String sOperation { get; set; }
		public VersionListViewItem(String filename, String operation)
		{
			sFilename = filename;
			sOperation = operation;
		}
	}
}
