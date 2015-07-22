using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class LoginWindow : Window
	{
		public enum LoginResponse {LOGIN, REGISTER, CANCEL};
		private LoginResponse lastResponse;
		private String username, password;

		public LoginWindow()
		{
			InitializeComponent();
			lError.Content = "";
		}

		public void showLogin(){
			this.ShowDialog();
		}

		public LoginResponse waitResponse()
		{
			return lastResponse;
		}

		public String Username
		{
			set {username = value;}
			get
			{
				return username;
			}
		}
		public String Password
		{
			private set {password = value;}
			get
			{
				SHA256Managed hashstring = new SHA256Managed();
				return Encoding.ASCII.GetString(hashstring.ComputeHash(Encoding.ASCII.GetBytes(password + username)));
			}
		}
		public String ErrorMessage
		{
			set
			{
				lError.Content = value;
			}
		}
		public bool KeepLoggedIn
		{
			get { return cbKeep.IsChecked.Value; }
		}

		private void LogIn_Click(object sender, RoutedEventArgs e)
		{
			username = tUsername.Text;
			password = tPassword.Password;
			if (username == "" || password == "")
			{
				this.ErrorMessage = "Username and passwrod cannot be empty";
				return;
			}
			lastResponse = LoginResponse.LOGIN;
			this.Hide();
		}

		private void Register_Click(object sender, RoutedEventArgs e)
		{
			username = tUsername.Text;
			password = tPassword.Password;
			if (username == "" || password == "")
			{
				this.ErrorMessage = "Username and passwrod cannot be empty";
				return;
			}
			lastResponse = LoginResponse.REGISTER;
			this.Hide();
		}
		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			lastResponse = LoginResponse.CANCEL;
			this.Hide();
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			lastResponse = LoginResponse.CANCEL;
			this.Hide();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			FocusManager.SetFocusedElement(this, tUsername);
		}
	}
}
