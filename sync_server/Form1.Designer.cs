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
			this.nUDVersion = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
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
			this.bDel = new System.Windows.Forms.Button();
			this.bUsers = new System.Windows.Forms.Button();
			this.lVersions = new System.Windows.Forms.Label();
			this.lUsers = new System.Windows.Forms.Label();
			this.lvUsers = new System.Windows.Forms.ListView();
			this.cId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.cUsername = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.cVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.lvVersions = new System.Windows.Forms.ListView();
			this.cVers = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.cTotFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.cTStamp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.tabControl1.SuspendLayout();
			this.tSettings.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nUDVersion)).BeginInit();
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
			this.tabControl1.Size = new System.Drawing.Size(610, 387);
			this.tabControl1.TabIndex = 0;
			// 
			// tSettings
			// 
			this.tSettings.Controls.Add(this.nUDVersion);
			this.tSettings.Controls.Add(this.label1);
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
			this.tSettings.Size = new System.Drawing.Size(602, 361);
			this.tSettings.TabIndex = 0;
			this.tSettings.Text = "Settings";
			this.tSettings.UseVisualStyleBackColor = true;
			// 
			// nUDVersion
			// 
			this.nUDVersion.Location = new System.Drawing.Point(117, 58);
			this.nUDVersion.Margin = new System.Windows.Forms.Padding(2);
			this.nUDVersion.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.nUDVersion.Name = "nUDVersion";
			this.nUDVersion.Size = new System.Drawing.Size(80, 20);
			this.nUDVersion.TabIndex = 21;
			this.nUDVersion.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 59);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(108, 13);
			this.label1.TabIndex = 11;
			this.label1.Text = "Max Version Number:";
			// 
			// lConnectedNum
			// 
			this.lConnectedNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lConnectedNum.AutoSize = true;
			this.lConnectedNum.Location = new System.Drawing.Point(531, 327);
			this.lConnectedNum.Name = "lConnectedNum";
			this.lConnectedNum.Size = new System.Drawing.Size(13, 13);
			this.lConnectedNum.TabIndex = 9;
			this.lConnectedNum.Text = "0";
			// 
			// lConnectedUser
			// 
			this.lConnectedUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lConnectedUser.AutoSize = true;
			this.lConnectedUser.Location = new System.Drawing.Point(440, 327);
			this.lConnectedUser.Name = "lConnectedUser";
			this.lConnectedUser.Size = new System.Drawing.Size(85, 13);
			this.lConnectedUser.TabIndex = 1;
			this.lConnectedUser.Text = "Connected user:";
			// 
			// bBrowse
			// 
			this.bBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.bBrowse.Location = new System.Drawing.Point(567, 29);
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
			this.tDirectory.Size = new System.Drawing.Size(444, 20);
			this.tDirectory.TabIndex = 7;
			this.tDirectory.Text = "C:\\Users\\Andrea Ferri\\ProgettoMalnati";
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
			this.lbLog.Location = new System.Drawing.Point(9, 107);
			this.lbLog.Name = "lbLog";
			this.lbLog.Size = new System.Drawing.Size(587, 186);
			this.lbLog.TabIndex = 5;
			// 
			// lLog
			// 
			this.lLog.AutoSize = true;
			this.lLog.Location = new System.Drawing.Point(7, 84);
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
			this.bStop.Location = new System.Drawing.Point(114, 312);
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
			this.bStart.Location = new System.Drawing.Point(6, 312);
			this.bStart.Name = "bStart";
			this.bStart.Size = new System.Drawing.Size(102, 43);
			this.bStart.TabIndex = 0;
			this.bStart.Text = "Start server";
			this.bStart.UseVisualStyleBackColor = true;
			this.bStart.Click += new System.EventHandler(this.bStart_Click);
			// 
			// tVersions
			// 
			this.tVersions.Controls.Add(this.bDel);
			this.tVersions.Controls.Add(this.bUsers);
			this.tVersions.Controls.Add(this.lVersions);
			this.tVersions.Controls.Add(this.lUsers);
			this.tVersions.Controls.Add(this.lvUsers);
			this.tVersions.Controls.Add(this.lvVersions);
			this.tVersions.Location = new System.Drawing.Point(4, 22);
			this.tVersions.Name = "tVersions";
			this.tVersions.Padding = new System.Windows.Forms.Padding(3);
			this.tVersions.Size = new System.Drawing.Size(602, 361);
			this.tVersions.TabIndex = 1;
			this.tVersions.Text = "Versions";
			this.tVersions.UseVisualStyleBackColor = true;
			this.tVersions.Click += new System.EventHandler(this.bUsers_Click);
			// 
			// bDel
			// 
			this.bDel.Location = new System.Drawing.Point(419, 140);
			this.bDel.Margin = new System.Windows.Forms.Padding(2);
			this.bDel.Name = "bDel";
			this.bDel.Size = new System.Drawing.Size(72, 19);
			this.bDel.TabIndex = 5;
			this.bDel.Text = "Delete User";
			this.bDel.UseVisualStyleBackColor = true;
			this.bDel.Click += new System.EventHandler(this.bDel_Click);
			// 
			// bUsers
			// 
			this.bUsers.Location = new System.Drawing.Point(347, 140);
			this.bUsers.Margin = new System.Windows.Forms.Padding(2);
			this.bUsers.Name = "bUsers";
			this.bUsers.Size = new System.Drawing.Size(54, 19);
			this.bUsers.TabIndex = 4;
			this.bUsers.Text = "Users";
			this.bUsers.UseVisualStyleBackColor = true;
			this.bUsers.Click += new System.EventHandler(this.bUsers_Click);
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
			this.lvUsers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.cId,
            this.cUsername,
            this.cVersion});
			this.lvUsers.FullRowSelect = true;
			this.lvUsers.HideSelection = false;
			this.lvUsers.Location = new System.Drawing.Point(3, 28);
			this.lvUsers.MultiSelect = false;
			this.lvUsers.Name = "lvUsers";
			this.lvUsers.Size = new System.Drawing.Size(593, 108);
			this.lvUsers.TabIndex = 1;
			this.lvUsers.UseCompatibleStateImageBehavior = false;
			this.lvUsers.View = System.Windows.Forms.View.Details;
			this.lvUsers.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvUsers_MouseDoubleClick);
			// 
			// cId
			// 
			this.cId.Text = "ID";
			// 
			// cUsername
			// 
			this.cUsername.Tag = "";
			this.cUsername.Text = "Username";
			this.cUsername.Width = 100;
			// 
			// cVersion
			// 
			this.cVersion.Text = "Version";
			this.cVersion.Width = 80;
			// 
			// lvVersions
			// 
			this.lvVersions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lvVersions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.cVers,
            this.cTotFile,
            this.cTStamp});
			this.lvVersions.FullRowSelect = true;
			this.lvVersions.Location = new System.Drawing.Point(3, 165);
			this.lvVersions.Name = "lvVersions";
			this.lvVersions.Size = new System.Drawing.Size(593, 190);
			this.lvVersions.TabIndex = 0;
			this.lvVersions.UseCompatibleStateImageBehavior = false;
			this.lvVersions.View = System.Windows.Forms.View.Details;
			// 
			// cVers
			// 
			this.cVers.Text = "Version";
			this.cVers.Width = 100;
			// 
			// cTotFile
			// 
			this.cTotFile.Text = "Total Files";
			this.cTotFile.Width = 100;
			// 
			// cTStamp
			// 
			this.cTStamp.Text = "TimeStamp";
			this.cTStamp.Width = 100;
			// 
			// fSyncServer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(634, 411);
			this.Controls.Add(this.tabControl1);
			this.MinimumSize = new System.Drawing.Size(500, 300);
			this.Name = "fSyncServer";
			this.Text = "Sync server";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fSyncServer_FormClosing);
			this.Load += new System.EventHandler(this.fSyncServer_Load);
			this.tabControl1.ResumeLayout(false);
			this.tSettings.ResumeLayout(false);
			this.tSettings.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nUDVersion)).EndInit();
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
		private System.Windows.Forms.ListView lvVersions;
		private System.Windows.Forms.Label lConnectedNum;
		private System.Windows.Forms.Label lConnectedUser;
        private System.Windows.Forms.Button bDel;
        private System.Windows.Forms.Button bUsers;
        private System.Windows.Forms.ColumnHeader cUsername;
        private System.Windows.Forms.ColumnHeader cVersion;
        private System.Windows.Forms.ColumnHeader cId;
        private System.Windows.Forms.ColumnHeader cVers;
        private System.Windows.Forms.ColumnHeader cTotFile;
        private System.Windows.Forms.ColumnHeader cTStamp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nUDVersion;
    }
}

