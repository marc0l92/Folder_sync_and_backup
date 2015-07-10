﻿namespace sync_server
{
    partial class fSyncServer
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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tSettings = new System.Windows.Forms.TabPage();
			this.lConnectedNum = new System.Windows.Forms.Label();
			this.lConnectedUser = new System.Windows.Forms.Label();
			this.bBrowse = new System.Windows.Forms.Button();
			this.tDirectory = new System.Windows.Forms.TextBox();
			this.lDirectory = new System.Windows.Forms.Label();
			this.lbLog = new System.Windows.Forms.ListBox();
			this.lLog = new System.Windows.Forms.Label();
			this.lPort = new System.Windows.Forms.Label();
			this.nPort = new System.Windows.Forms.NumericUpDown();
			this.bStop = new System.Windows.Forms.Button();
			this.bStart = new System.Windows.Forms.Button();
			this.tVersions = new System.Windows.Forms.TabPage();
			this.lVersions = new System.Windows.Forms.Label();
			this.lUsers = new System.Windows.Forms.Label();
			this.lvUsers = new System.Windows.Forms.ListView();
			this.listView1 = new System.Windows.Forms.ListView();
			this.tabControl1.SuspendLayout();
			this.tSettings.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nPort)).BeginInit();
			this.tVersions.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tSettings);
			this.tabControl1.Controls.Add(this.tVersions);
			this.tabControl1.Location = new System.Drawing.Point(12, 12);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(645, 428);
			this.tabControl1.TabIndex = 0;
			// 
			// tSettings
			// 
			this.tSettings.Controls.Add(this.lConnectedNum);
			this.tSettings.Controls.Add(this.lConnectedUser);
			this.tSettings.Controls.Add(this.bBrowse);
			this.tSettings.Controls.Add(this.tDirectory);
			this.tSettings.Controls.Add(this.lDirectory);
			this.tSettings.Controls.Add(this.lbLog);
			this.tSettings.Controls.Add(this.lLog);
			this.tSettings.Controls.Add(this.lPort);
			this.tSettings.Controls.Add(this.nPort);
			this.tSettings.Controls.Add(this.bStop);
			this.tSettings.Controls.Add(this.bStart);
			this.tSettings.Location = new System.Drawing.Point(4, 22);
			this.tSettings.Name = "tSettings";
			this.tSettings.Padding = new System.Windows.Forms.Padding(3);
			this.tSettings.Size = new System.Drawing.Size(637, 402);
			this.tSettings.TabIndex = 0;
			this.tSettings.Text = "Settings";
			this.tSettings.UseVisualStyleBackColor = true;
			// 
			// lConnectedNum
			// 
			this.lConnectedNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lConnectedNum.AutoSize = true;
			this.lConnectedNum.Location = new System.Drawing.Point(566, 368);
			this.lConnectedNum.Name = "lConnectedNum";
			this.lConnectedNum.Size = new System.Drawing.Size(13, 13);
			this.lConnectedNum.TabIndex = 9;
			this.lConnectedNum.Text = "0";
			// 
			// lConnectedUser
			// 
			this.lConnectedUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lConnectedUser.AutoSize = true;
			this.lConnectedUser.Location = new System.Drawing.Point(475, 368);
			this.lConnectedUser.Name = "lConnectedUser";
			this.lConnectedUser.Size = new System.Drawing.Size(85, 13);
			this.lConnectedUser.TabIndex = 1;
			this.lConnectedUser.Text = "Connected user:";
			// 
			// bBrowse
			// 
			this.bBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.bBrowse.Location = new System.Drawing.Point(602, 29);
			this.bBrowse.Name = "bBrowse";
			this.bBrowse.Size = new System.Drawing.Size(29, 23);
			this.bBrowse.TabIndex = 8;
			this.bBrowse.Text = "...";
			this.bBrowse.UseVisualStyleBackColor = true;
			this.bBrowse.Click += new System.EventHandler(this.bBrowse_Click);
			// 
			// tDirectory
			// 
			this.tDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tDirectory.Location = new System.Drawing.Point(117, 29);
			this.tDirectory.Name = "tDirectory";
			this.tDirectory.Size = new System.Drawing.Size(479, 20);
			this.tDirectory.TabIndex = 7;
			this.tDirectory.Text = "D:\\ProgettoMalnati\\server";
			// 
			// lDirectory
			// 
			this.lDirectory.AutoSize = true;
			this.lDirectory.Location = new System.Drawing.Point(114, 13);
			this.lDirectory.Name = "lDirectory";
			this.lDirectory.Size = new System.Drawing.Size(93, 13);
			this.lDirectory.TabIndex = 6;
			this.lDirectory.Text = "Working directory:";
			// 
			// lbLog
			// 
			this.lbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbLog.FormattingEnabled = true;
			this.lbLog.Items.AddRange(new object[] {
            "Application started."});
			this.lbLog.Location = new System.Drawing.Point(9, 81);
			this.lbLog.Name = "lbLog";
			this.lbLog.Size = new System.Drawing.Size(622, 264);
			this.lbLog.TabIndex = 5;
			// 
			// lLog
			// 
			this.lLog.AutoSize = true;
			this.lLog.Location = new System.Drawing.Point(6, 65);
			this.lLog.Name = "lLog";
			this.lLog.Size = new System.Drawing.Size(75, 13);
			this.lLog.TabIndex = 4;
			this.lLog.Text = "Messages log:";
			// 
			// lPort
			// 
			this.lPort.AutoSize = true;
			this.lPort.Location = new System.Drawing.Point(6, 13);
			this.lPort.Name = "lPort";
			this.lPort.Size = new System.Drawing.Size(62, 13);
			this.lPort.TabIndex = 3;
			this.lPort.Text = "Server port:";
			// 
			// nPort
			// 
			this.nPort.Location = new System.Drawing.Point(9, 29);
			this.nPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.nPort.Name = "nPort";
			this.nPort.Size = new System.Drawing.Size(99, 20);
			this.nPort.TabIndex = 2;
			this.nPort.Value = new decimal(new int[] {
            55555,
            0,
            0,
            0});
			// 
			// bStop
			// 
			this.bStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bStop.Enabled = false;
			this.bStop.Location = new System.Drawing.Point(114, 353);
			this.bStop.Name = "bStop";
			this.bStop.Size = new System.Drawing.Size(102, 43);
			this.bStop.TabIndex = 1;
			this.bStop.Text = "Stop server";
			this.bStop.UseVisualStyleBackColor = true;
			this.bStop.Click += new System.EventHandler(this.bStop_Click);
			// 
			// bStart
			// 
			this.bStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bStart.Location = new System.Drawing.Point(6, 353);
			this.bStart.Name = "bStart";
			this.bStart.Size = new System.Drawing.Size(102, 43);
			this.bStart.TabIndex = 0;
			this.bStart.Text = "Start server";
			this.bStart.UseVisualStyleBackColor = true;
			this.bStart.Click += new System.EventHandler(this.bStart_Click);
			// 
			// tVersions
			// 
			this.tVersions.Controls.Add(this.lVersions);
			this.tVersions.Controls.Add(this.lUsers);
			this.tVersions.Controls.Add(this.lvUsers);
			this.tVersions.Controls.Add(this.listView1);
			this.tVersions.Location = new System.Drawing.Point(4, 22);
			this.tVersions.Name = "tVersions";
			this.tVersions.Padding = new System.Windows.Forms.Padding(3);
			this.tVersions.Size = new System.Drawing.Size(637, 402);
			this.tVersions.TabIndex = 1;
			this.tVersions.Text = "Versions";
			this.tVersions.UseVisualStyleBackColor = true;
			// 
			// lVersions
			// 
			this.lVersions.AutoSize = true;
			this.lVersions.Location = new System.Drawing.Point(6, 149);
			this.lVersions.Name = "lVersions";
			this.lVersions.Size = new System.Drawing.Size(47, 13);
			this.lVersions.TabIndex = 3;
			this.lVersions.Text = "Versions";
			// 
			// lUsers
			// 
			this.lUsers.AutoSize = true;
			this.lUsers.Location = new System.Drawing.Point(6, 12);
			this.lUsers.Name = "lUsers";
			this.lUsers.Size = new System.Drawing.Size(37, 13);
			this.lUsers.TabIndex = 2;
			this.lUsers.Text = "Users:";
			// 
			// lvUsers
			// 
			this.lvUsers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lvUsers.Location = new System.Drawing.Point(3, 28);
			this.lvUsers.Name = "lvUsers";
			this.lvUsers.Size = new System.Drawing.Size(628, 108);
			this.lvUsers.TabIndex = 1;
			this.lvUsers.UseCompatibleStateImageBehavior = false;
			// 
			// listView1
			// 
			this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listView1.Location = new System.Drawing.Point(3, 165);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(628, 231);
			this.listView1.TabIndex = 0;
			this.listView1.UseCompatibleStateImageBehavior = false;
			// 
			// fSyncServer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(669, 452);
			this.Controls.Add(this.tabControl1);
			this.Name = "fSyncServer";
			this.Text = "Sync server";
			this.tabControl1.ResumeLayout(false);
			this.tSettings.ResumeLayout(false);
			this.tSettings.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nPort)).EndInit();
			this.tVersions.ResumeLayout(false);
			this.tVersions.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tSettings;
		private System.Windows.Forms.Label lPort;
		private System.Windows.Forms.NumericUpDown nPort;
		private System.Windows.Forms.Button bStop;
		private System.Windows.Forms.Button bStart;
		private System.Windows.Forms.TabPage tVersions;
		private System.Windows.Forms.ListBox lbLog;
		private System.Windows.Forms.Label lLog;
		private System.Windows.Forms.TextBox tDirectory;
		private System.Windows.Forms.Label lDirectory;
		private System.Windows.Forms.Button bBrowse;
		private System.Windows.Forms.Label lVersions;
		private System.Windows.Forms.Label lUsers;
		private System.Windows.Forms.ListView lvUsers;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.Label lConnectedNum;
		private System.Windows.Forms.Label lConnectedUser;
    }
}

