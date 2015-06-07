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
            this.tabControl1.SuspendLayout();
            this.tSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nPort)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
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
            // bBrowse
            // 
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
            this.tDirectory.Location = new System.Drawing.Point(176, 45);
            this.tDirectory.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tDirectory.Name = "tDirectory";
            this.tDirectory.Size = new System.Drawing.Size(716, 26);
            this.tDirectory.TabIndex = 7;
            this.tDirectory.Text = "C:\\Users\\Andrea Ferri\\Desktop\\SyncFolder";
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
            this.lbLog.FormattingEnabled = true;
            this.lbLog.ItemHeight = 20;
            this.lbLog.Items.AddRange(new object[] {
            "Application started."});
            this.lbLog.Location = new System.Drawing.Point(14, 125);
            this.lbLog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(931, 404);
            this.lbLog.TabIndex = 5;
            // 
            // lLog
            // 
            this.lLog.AutoSize = true;
            this.lLog.Location = new System.Drawing.Point(9, 100);
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
            this.tVersions.Location = new System.Drawing.Point(4, 29);
            this.tVersions.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tVersions.Name = "tVersions";
            this.tVersions.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tVersions.Size = new System.Drawing.Size(960, 625);
            this.tVersions.TabIndex = 1;
            this.tVersions.Text = "Versions";
            this.tVersions.UseVisualStyleBackColor = true;
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
            ((System.ComponentModel.ISupportInitialize)(this.nPort)).EndInit();
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
    }
}

