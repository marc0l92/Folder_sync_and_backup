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
            this.lDir = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.bBrowse = new System.Windows.Forms.Button();
            this.lVersions = new System.Windows.Forms.ListBox();
            this.bRestore = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
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
            this.bStart.Location = new System.Drawing.Point(12, 12);
            this.bStart.Name = "bStart";
            this.bStart.Size = new System.Drawing.Size(109, 34);
            this.bStart.TabIndex = 0;
            this.bStart.Text = "Start sync";
            this.bStart.UseVisualStyleBackColor = true;
            // 
            // bStop
            // 
            this.bStop.Enabled = false;
            this.bStop.Location = new System.Drawing.Point(127, 12);
            this.bStop.Name = "bStop";
            this.bStop.Size = new System.Drawing.Size(109, 34);
            this.bStop.TabIndex = 0;
            this.bStop.Text = "Stop sync";
            this.bStop.UseVisualStyleBackColor = true;
            // 
            // lDir
            // 
            this.lDir.AutoSize = true;
            this.lDir.Location = new System.Drawing.Point(276, 23);
            this.lDir.Name = "lDir";
            this.lDir.Size = new System.Drawing.Size(52, 13);
            this.lDir.TabIndex = 1;
            this.lDir.Text = "Directory:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(334, 20);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(225, 20);
            this.textBox1.TabIndex = 2;
            // 
            // bBrowse
            // 
            this.bBrowse.Location = new System.Drawing.Point(565, 18);
            this.bBrowse.Name = "bBrowse";
            this.bBrowse.Size = new System.Drawing.Size(25, 23);
            this.bBrowse.TabIndex = 3;
            this.bBrowse.Text = "...";
            this.bBrowse.UseVisualStyleBackColor = true;
            // 
            // lVersions
            // 
            this.lVersions.FormattingEnabled = true;
            this.lVersions.Location = new System.Drawing.Point(15, 59);
            this.lVersions.Name = "lVersions";
            this.lVersions.ScrollAlwaysVisible = true;
            this.lVersions.Size = new System.Drawing.Size(577, 342);
            this.lVersions.TabIndex = 4;
            // 
            // bRestore
            // 
            this.bRestore.Location = new System.Drawing.Point(518, 412);
            this.bRestore.Name = "bRestore";
            this.bRestore.Size = new System.Drawing.Size(74, 26);
            this.bRestore.TabIndex = 5;
            this.bRestore.Text = "Restore";
            this.bRestore.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 446);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(604, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lStatus
            // 
            this.lStatus.Name = "lStatus";
            this.lStatus.Size = new System.Drawing.Size(31, 17);
            this.lStatus.Text = "Stop";
            // 
            // fSync
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 468);
            this.Controls.Add(this.lVersions);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.bRestore);
            this.Controls.Add(this.bBrowse);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lDir);
            this.Controls.Add(this.bStop);
            this.Controls.Add(this.bStart);
            this.MaximizeBox = false;
            this.Name = "fSync";
            this.Text = "Sync";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.DirectoryServices.DirectorySearcher directorySearcher1;
        private System.Windows.Forms.Button bStart;
        private System.Windows.Forms.Button bStop;
        private System.Windows.Forms.Label lDir;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button bBrowse;
        private System.Windows.Forms.ListBox lVersions;
        private System.Windows.Forms.Button bRestore;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lStatus;
    }
}

