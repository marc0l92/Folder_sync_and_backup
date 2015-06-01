namespace sync_client
{
    partial class fSync
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.directorySearcher1 = new System.DirectoryServices.DirectorySearcher();
			this.bStart = new System.Windows.Forms.Button();
			this.bStop = new System.Windows.Forms.Button();
			this.lVersions = new System.Windows.Forms.ListBox();
			this.bRestore = new System.Windows.Forms.Button();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.lStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.tDirectory = new System.Windows.Forms.TextBox();
			this.lDirectory = new System.Windows.Forms.Label();
			this.bBrowse = new System.Windows.Forms.Button();
			this.tUsername = new System.Windows.Forms.TextBox();
			this.lPassword = new System.Windows.Forms.Label();
			this.tPassword = new System.Windows.Forms.TextBox();
			this.lUsername = new System.Windows.Forms.Label();
			this.tController = new System.Windows.Forms.TabControl();
			this.tSettings = new System.Windows.Forms.TabPage();
			this.nPort = new System.Windows.Forms.NumericUpDown();
			this.lPort = new System.Windows.Forms.Label();
			this.lServer = new System.Windows.Forms.Label();
			this.tAddress = new System.Windows.Forms.TextBox();
			this.tVersions = new System.Windows.Forms.TabPage();
			this.statusStrip1.SuspendLayout();
			this.tController.SuspendLayout();
			this.tSettings.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nPort)).BeginInit();
			this.tVersions.SuspendLayout();
			this.SuspendLayout();
			// 
			// directorySearcher1
			// 
			this.directorySearcher1.ClientTimeout = System.TimeSpan.Parse("-00:00:01");
			this.directorySearcher1.ServerPageTimeLimit = System.TimeSpan.Parse("-00:00:01");
			this.directorySearcher1.ServerTimeLimit = System.TimeSpan.Parse("-00:00:01");
			// 
			// bStart
			// 
			this.bStart.Location = new System.Drawing.Point(9, 288);
			this.bStart.Name = "bStart";
			this.bStart.Size = new System.Drawing.Size(109, 34);
			this.bStart.TabIndex = 0;
			this.bStart.Text = "Start sync";
			this.bStart.UseVisualStyleBackColor = true;
			this.bStart.Click += new System.EventHandler(this.bStart_Click);
			// 
			// bStop
			// 
			this.bStop.Enabled = false;
			this.bStop.Location = new System.Drawing.Point(124, 288);
			this.bStop.Name = "bStop";
			this.bStop.Size = new System.Drawing.Size(109, 34);
			this.bStop.TabIndex = 0;
			this.bStop.Text = "Stop sync";
			this.bStop.UseVisualStyleBackColor = true;
			this.bStop.Click += new System.EventHandler(this.bStop_Click);
			// 
			// lVersions
			// 
			this.lVersions.FormattingEnabled = true;
			this.lVersions.Location = new System.Drawing.Point(6, 6);
			this.lVersions.Name = "lVersions";
			this.lVersions.ScrollAlwaysVisible = true;
			this.lVersions.Size = new System.Drawing.Size(580, 277);
			this.lVersions.TabIndex = 4;
			// 
			// bRestore
			// 
			this.bRestore.Enabled = false;
			this.bRestore.Location = new System.Drawing.Point(6, 289);
			this.bRestore.Name = "bRestore";
			this.bRestore.Size = new System.Drawing.Size(74, 26);
			this.bRestore.TabIndex = 5;
			this.bRestore.Text = "Restore";
			this.bRestore.UseVisualStyleBackColor = true;
			this.bRestore.Click += new System.EventHandler(this.bRestore_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lStatus});
			this.statusStrip1.Location = new System.Drawing.Point(0, 376);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(638, 22);
			this.statusStrip1.TabIndex = 6;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// lStatus
			// 
			this.lStatus.Name = "lStatus";
			this.lStatus.Size = new System.Drawing.Size(31, 17);
			this.lStatus.Text = "Stop";
			// 
			// tDirectory
			// 
			this.tDirectory.Location = new System.Drawing.Point(9, 87);
			this.tDirectory.Name = "tDirectory";
			this.tDirectory.Size = new System.Drawing.Size(372, 20);
			this.tDirectory.TabIndex = 2;
			this.tDirectory.Text = "D:\\ProgettoMalnati\\client";
			// 
			// lDirectory
			// 
			this.lDirectory.AutoSize = true;
			this.lDirectory.Location = new System.Drawing.Point(6, 71);
			this.lDirectory.Name = "lDirectory";
			this.lDirectory.Size = new System.Drawing.Size(52, 13);
			this.lDirectory.TabIndex = 1;
			this.lDirectory.Text = "Directory:";
			// 
			// bBrowse
			// 
			this.bBrowse.Location = new System.Drawing.Point(387, 87);
			this.bBrowse.Name = "bBrowse";
			this.bBrowse.Size = new System.Drawing.Size(25, 23);
			this.bBrowse.TabIndex = 3;
			this.bBrowse.Text = "...";
			this.bBrowse.UseVisualStyleBackColor = true;
			this.bBrowse.Click += new System.EventHandler(this.bBrowse_Click);
			// 
			// tUsername
			// 
			this.tUsername.Location = new System.Drawing.Point(9, 31);
			this.tUsername.Name = "tUsername";
			this.tUsername.Size = new System.Drawing.Size(191, 20);
			this.tUsername.TabIndex = 7;
			this.tUsername.Text = "user1";
			// 
			// lPassword
			// 
			this.lPassword.AutoSize = true;
			this.lPassword.Location = new System.Drawing.Point(203, 15);
			this.lPassword.Name = "lPassword";
			this.lPassword.Size = new System.Drawing.Size(56, 13);
			this.lPassword.TabIndex = 10;
			this.lPassword.Text = "Password:";
			// 
			// tPassword
			// 
			this.tPassword.Location = new System.Drawing.Point(206, 31);
			this.tPassword.Name = "tPassword";
			this.tPassword.PasswordChar = '*';
			this.tPassword.Size = new System.Drawing.Size(175, 20);
			this.tPassword.TabIndex = 9;
			this.tPassword.Text = "password";
			// 
			// lUsername
			// 
			this.lUsername.AutoSize = true;
			this.lUsername.Location = new System.Drawing.Point(6, 15);
			this.lUsername.Name = "lUsername";
			this.lUsername.Size = new System.Drawing.Size(58, 13);
			this.lUsername.TabIndex = 8;
			this.lUsername.Text = "Username:";
			// 
			// tController
			// 
			this.tController.Controls.Add(this.tSettings);
			this.tController.Controls.Add(this.tVersions);
			this.tController.Location = new System.Drawing.Point(12, 12);
			this.tController.Name = "tController";
			this.tController.SelectedIndex = 0;
			this.tController.Size = new System.Drawing.Size(612, 354);
			this.tController.TabIndex = 12;
			// 
			// tSettings
			// 
			this.tSettings.Controls.Add(this.nPort);
			this.tSettings.Controls.Add(this.lPort);
			this.tSettings.Controls.Add(this.lServer);
			this.tSettings.Controls.Add(this.tAddress);
			this.tSettings.Controls.Add(this.bBrowse);
			this.tSettings.Controls.Add(this.lPassword);
			this.tSettings.Controls.Add(this.tDirectory);
			this.tSettings.Controls.Add(this.lDirectory);
			this.tSettings.Controls.Add(this.bStop);
			this.tSettings.Controls.Add(this.tPassword);
			this.tSettings.Controls.Add(this.bStart);
			this.tSettings.Controls.Add(this.lUsername);
			this.tSettings.Controls.Add(this.tUsername);
			this.tSettings.Location = new System.Drawing.Point(4, 22);
			this.tSettings.Name = "tSettings";
			this.tSettings.Padding = new System.Windows.Forms.Padding(3);
			this.tSettings.Size = new System.Drawing.Size(604, 328);
			this.tSettings.TabIndex = 0;
			this.tSettings.Text = "Settings";
			this.tSettings.UseVisualStyleBackColor = true;
			// 
			// nPort
			// 
			this.nPort.Location = new System.Drawing.Point(312, 142);
			this.nPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.nPort.Name = "nPort";
			this.nPort.Size = new System.Drawing.Size(69, 20);
			this.nPort.TabIndex = 15;
			this.nPort.Value = new decimal(new int[] {
            55555,
            0,
            0,
            0});
			// 
			// lPort
			// 
			this.lPort.AutoSize = true;
			this.lPort.Location = new System.Drawing.Point(309, 125);
			this.lPort.Name = "lPort";
			this.lPort.Size = new System.Drawing.Size(29, 13);
			this.lPort.TabIndex = 14;
			this.lPort.Text = "Port:";
			// 
			// lServer
			// 
			this.lServer.AutoSize = true;
			this.lServer.Location = new System.Drawing.Point(6, 125);
			this.lServer.Name = "lServer";
			this.lServer.Size = new System.Drawing.Size(81, 13);
			this.lServer.TabIndex = 13;
			this.lServer.Text = "Server address:";
			// 
			// tAddress
			// 
			this.tAddress.Location = new System.Drawing.Point(9, 141);
			this.tAddress.Name = "tAddress";
			this.tAddress.Size = new System.Drawing.Size(295, 20);
			this.tAddress.TabIndex = 11;
			this.tAddress.Text = "127.0.0.1";
			// 
			// tVersions
			// 
			this.tVersions.Controls.Add(this.lVersions);
			this.tVersions.Controls.Add(this.bRestore);
			this.tVersions.Location = new System.Drawing.Point(4, 22);
			this.tVersions.Name = "tVersions";
			this.tVersions.Padding = new System.Windows.Forms.Padding(3);
			this.tVersions.Size = new System.Drawing.Size(604, 328);
			this.tVersions.TabIndex = 1;
			this.tVersions.Text = "Versions";
			this.tVersions.UseVisualStyleBackColor = true;
			// 
			// fSync
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(638, 398);
			this.Controls.Add(this.tController);
			this.Controls.Add(this.statusStrip1);
			this.MaximizeBox = false;
			this.Name = "fSync";
			this.Text = "Sync";
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.tController.ResumeLayout(false);
			this.tSettings.ResumeLayout(false);
			this.tSettings.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nPort)).EndInit();
			this.tVersions.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.DirectoryServices.DirectorySearcher directorySearcher1;
        private System.Windows.Forms.Button bStart;
		private System.Windows.Forms.Button bStop;
        private System.Windows.Forms.ListBox lVersions;
        private System.Windows.Forms.Button bRestore;
        private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel lStatus;
		private System.Windows.Forms.TextBox tDirectory;
		private System.Windows.Forms.Label lDirectory;
		private System.Windows.Forms.Button bBrowse;
		private System.Windows.Forms.TextBox tUsername;
		private System.Windows.Forms.Label lPassword;
		private System.Windows.Forms.TextBox tPassword;
		private System.Windows.Forms.Label lUsername;
		private System.Windows.Forms.TabControl tController;
		private System.Windows.Forms.TabPage tSettings;
		private System.Windows.Forms.TabPage tVersions;
		private System.Windows.Forms.Label lPort;
		private System.Windows.Forms.Label lServer;
		private System.Windows.Forms.TextBox tAddress;
		private System.Windows.Forms.NumericUpDown nPort;
    }
}

