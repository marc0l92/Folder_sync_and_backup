namespace sync_server
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
            this.tabControl1.Location = new System.Drawing.Point(18, 18);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(968, 658);
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
            this.tSettings.Location = new System.Drawing.Point(4, 29);
            this.tSettings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tSettings.Name = "tSettings";
            this.tSettings.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tSettings.Size = new System.Drawing.Size(960, 625);
            this.tSettings.TabIndex = 0;
            this.tSettings.Text = "Settings";
            this.tSettings.UseVisualStyleBackColor = true;
            // 
            // nUDVersion
            // 
            this.nUDVersion.Location = new System.Drawing.Point(176, 89);
            this.nUDVersion.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nUDVersion.Name = "nUDVersion";
            this.nUDVersion.Size = new System.Drawing.Size(120, 26);
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
            this.label1.Location = new System.Drawing.Point(10, 91);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 20);
            this.label1.TabIndex = 11;
            this.label1.Text = "Max Version Number:";
            // 
            // lConnectedNum
            // 
            this.lConnectedNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lConnectedNum.AutoSize = true;
            this.lConnectedNum.Location = new System.Drawing.Point(849, 566);
            this.lConnectedNum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lConnectedNum.Name = "lConnectedNum";
            this.lConnectedNum.Size = new System.Drawing.Size(18, 20);
            this.lConnectedNum.TabIndex = 9;
            this.lConnectedNum.Text = "0";
            // 
            // lConnectedUser
            // 
            this.lConnectedUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lConnectedUser.AutoSize = true;
            this.lConnectedUser.Location = new System.Drawing.Point(712, 566);
            this.lConnectedUser.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lConnectedUser.Name = "lConnectedUser";
            this.lConnectedUser.Size = new System.Drawing.Size(126, 20);
            this.lConnectedUser.TabIndex = 1;
            this.lConnectedUser.Text = "Connected user:";
            // 
            // bBrowse
            // 
            this.bBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bBrowse.Location = new System.Drawing.Point(903, 45);
            this.bBrowse.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bBrowse.Name = "bBrowse";
            this.bBrowse.Size = new System.Drawing.Size(44, 35);
            this.bBrowse.TabIndex = 8;
            this.bBrowse.Text = "...";
            this.bBrowse.UseVisualStyleBackColor = true;
            this.bBrowse.Click += new System.EventHandler(this.bBrowse_Click);
            // 
            // tDirectory
            // 
            this.tDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tDirectory.Location = new System.Drawing.Point(176, 45);
            this.tDirectory.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tDirectory.Name = "tDirectory";
            this.tDirectory.Size = new System.Drawing.Size(716, 26);
            this.tDirectory.TabIndex = 7;
            this.tDirectory.Text = "C:\\Users\\Andrea Ferri\\ProgettoMalnati";
            // 
            // lDirectory
            // 
            this.lDirectory.AutoSize = true;
            this.lDirectory.Location = new System.Drawing.Point(171, 20);
            this.lDirectory.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lDirectory.Name = "lDirectory";
            this.lDirectory.Size = new System.Drawing.Size(135, 20);
            this.lDirectory.TabIndex = 6;
            this.lDirectory.Text = "Working directory:";
            // 
            // lbLog
            // 
            this.lbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLog.FormattingEnabled = true;
            this.lbLog.ItemHeight = 20;
            this.lbLog.Items.AddRange(new object[] {
            "Application started."});
            this.lbLog.Location = new System.Drawing.Point(14, 165);
            this.lbLog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(931, 364);
            this.lbLog.TabIndex = 5;
            // 
            // lLog
            // 
            this.lLog.AutoSize = true;
            this.lLog.Location = new System.Drawing.Point(10, 130);
            this.lLog.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lLog.Name = "lLog";
            this.lLog.Size = new System.Drawing.Size(111, 20);
            this.lLog.TabIndex = 4;
            this.lLog.Text = "Messages log:";
            // 
            // lPort
            // 
            this.lPort.AutoSize = true;
            this.lPort.Location = new System.Drawing.Point(9, 20);
            this.lPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lPort.Name = "lPort";
            this.lPort.Size = new System.Drawing.Size(91, 20);
            this.lPort.TabIndex = 3;
            this.lPort.Text = "Server port:";
            // 
            // nPort
            // 
            this.nPort.Location = new System.Drawing.Point(14, 45);
            this.nPort.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.nPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nPort.Name = "nPort";
            this.nPort.Size = new System.Drawing.Size(148, 26);
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
            this.bStop.Location = new System.Drawing.Point(171, 543);
            this.bStop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bStop.Name = "bStop";
            this.bStop.Size = new System.Drawing.Size(153, 66);
            this.bStop.TabIndex = 1;
            this.bStop.Text = "Stop server";
            this.bStop.UseVisualStyleBackColor = true;
            this.bStop.Click += new System.EventHandler(this.bStop_Click);
            // 
            // bStart
            // 
            this.bStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bStart.Location = new System.Drawing.Point(9, 543);
            this.bStart.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bStart.Name = "bStart";
            this.bStart.Size = new System.Drawing.Size(153, 66);
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
            this.tVersions.Location = new System.Drawing.Point(4, 29);
            this.tVersions.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tVersions.Name = "tVersions";
            this.tVersions.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tVersions.Size = new System.Drawing.Size(960, 625);
            this.tVersions.TabIndex = 1;
            this.tVersions.Text = "Versions";
            this.tVersions.UseVisualStyleBackColor = true;
            this.tVersions.Click += new System.EventHandler(this.bUsers_Click);
            // 
            // bDel
            // 
            this.bDel.Location = new System.Drawing.Point(628, 215);
            this.bDel.Name = "bDel";
            this.bDel.Size = new System.Drawing.Size(108, 30);
            this.bDel.TabIndex = 5;
            this.bDel.Text = "Delete User";
            this.bDel.UseVisualStyleBackColor = true;
            this.bDel.Click += new System.EventHandler(this.bDel_Click);
            // 
            // bUsers
            // 
            this.bUsers.Location = new System.Drawing.Point(520, 215);
            this.bUsers.Name = "bUsers";
            this.bUsers.Size = new System.Drawing.Size(81, 30);
            this.bUsers.TabIndex = 4;
            this.bUsers.Text = "Users";
            this.bUsers.UseVisualStyleBackColor = true;
            this.bUsers.Click += new System.EventHandler(this.bUsers_Click);
            // 
            // lVersions
            // 
            this.lVersions.AutoSize = true;
            this.lVersions.Location = new System.Drawing.Point(9, 229);
            this.lVersions.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lVersions.Name = "lVersions";
            this.lVersions.Size = new System.Drawing.Size(71, 20);
            this.lVersions.TabIndex = 3;
            this.lVersions.Text = "Versions";
            // 
            // lUsers
            // 
            this.lUsers.AutoSize = true;
            this.lUsers.Location = new System.Drawing.Point(9, 18);
            this.lUsers.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lUsers.Name = "lUsers";
            this.lUsers.Size = new System.Drawing.Size(55, 20);
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
            this.lvUsers.Location = new System.Drawing.Point(4, 43);
            this.lvUsers.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lvUsers.MultiSelect = false;
            this.lvUsers.Name = "lvUsers";
            this.lvUsers.Size = new System.Drawing.Size(940, 164);
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
            this.lvVersions.Location = new System.Drawing.Point(4, 254);
            this.lvVersions.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lvVersions.Name = "lvVersions";
            this.lvVersions.Size = new System.Drawing.Size(940, 353);
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 695);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "fSyncServer";
            this.Text = "Sync server";
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

