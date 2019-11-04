namespace Nmap_MT
{
    partial class NmapMT
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
            this.txtFrom1 = new System.Windows.Forms.TextBox();
            this.txtFrom2 = new System.Windows.Forms.TextBox();
            this.txtFrom3 = new System.Windows.Forms.TextBox();
            this.txtFrom4 = new System.Windows.Forms.TextBox();
            this.txtTo1 = new System.Windows.Forms.TextBox();
            this.txtTo2 = new System.Windows.Forms.TextBox();
            this.txtTo3 = new System.Windows.Forms.TextBox();
            this.txtTo4 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtNmapArgs = new System.Windows.Forms.TextBox();
            this.lvScans = new System.Windows.Forms.ListView();
            this.IP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Results = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnStartStop = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.numThreads = new System.Windows.Forms.NumericUpDown();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tstProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.tstStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.hostsPerThread = new System.Windows.Forms.NumericUpDown();
            this.cbShowOffline = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numThreads)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hostsPerThread)).BeginInit();
            this.SuspendLayout();
            // 
            // txtFrom1
            // 
            this.txtFrom1.Location = new System.Drawing.Point(13, 16);
            this.txtFrom1.MaxLength = 3;
            this.txtFrom1.Name = "txtFrom1";
            this.txtFrom1.Size = new System.Drawing.Size(34, 20);
            this.txtFrom1.TabIndex = 0;
            this.txtFrom1.Tag = "From Octet 1";
            this.txtFrom1.Text = "172";
            this.txtFrom1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ip_change);
            // 
            // txtFrom2
            // 
            this.txtFrom2.Location = new System.Drawing.Point(53, 16);
            this.txtFrom2.MaxLength = 3;
            this.txtFrom2.Name = "txtFrom2";
            this.txtFrom2.Size = new System.Drawing.Size(34, 20);
            this.txtFrom2.TabIndex = 1;
            this.txtFrom2.Tag = "From Octet 2";
            this.txtFrom2.Text = "31";
            this.txtFrom2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ip_change);
            // 
            // txtFrom3
            // 
            this.txtFrom3.Location = new System.Drawing.Point(93, 16);
            this.txtFrom3.MaxLength = 3;
            this.txtFrom3.Name = "txtFrom3";
            this.txtFrom3.Size = new System.Drawing.Size(34, 20);
            this.txtFrom3.TabIndex = 2;
            this.txtFrom3.Tag = "From Octet 3";
            this.txtFrom3.Text = "0";
            this.txtFrom3.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ip_change);
            // 
            // txtFrom4
            // 
            this.txtFrom4.Location = new System.Drawing.Point(133, 16);
            this.txtFrom4.MaxLength = 3;
            this.txtFrom4.Name = "txtFrom4";
            this.txtFrom4.Size = new System.Drawing.Size(34, 20);
            this.txtFrom4.TabIndex = 3;
            this.txtFrom4.Tag = "From Octet 4";
            this.txtFrom4.Text = "0";
            this.txtFrom4.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ip_change);
            // 
            // txtTo1
            // 
            this.txtTo1.Location = new System.Drawing.Point(13, 16);
            this.txtTo1.MaxLength = 3;
            this.txtTo1.Name = "txtTo1";
            this.txtTo1.Size = new System.Drawing.Size(34, 20);
            this.txtTo1.TabIndex = 4;
            this.txtTo1.Tag = "To Octet 1";
            this.txtTo1.Text = "172";
            this.txtTo1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ip_change);
            // 
            // txtTo2
            // 
            this.txtTo2.Location = new System.Drawing.Point(53, 16);
            this.txtTo2.MaxLength = 3;
            this.txtTo2.Name = "txtTo2";
            this.txtTo2.Size = new System.Drawing.Size(34, 20);
            this.txtTo2.TabIndex = 5;
            this.txtTo2.Tag = "To Octet 2";
            this.txtTo2.Text = "31";
            this.txtTo2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ip_change);
            // 
            // txtTo3
            // 
            this.txtTo3.Location = new System.Drawing.Point(93, 16);
            this.txtTo3.MaxLength = 3;
            this.txtTo3.Name = "txtTo3";
            this.txtTo3.Size = new System.Drawing.Size(34, 20);
            this.txtTo3.TabIndex = 6;
            this.txtTo3.Tag = "To Octet 3";
            this.txtTo3.Text = "0";
            this.txtTo3.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ip_change);
            // 
            // txtTo4
            // 
            this.txtTo4.Location = new System.Drawing.Point(133, 16);
            this.txtTo4.MaxLength = 3;
            this.txtTo4.Name = "txtTo4";
            this.txtTo4.Size = new System.Drawing.Size(34, 20);
            this.txtTo4.TabIndex = 7;
            this.txtTo4.Tag = "To Octet 4";
            this.txtTo4.Text = "255";
            this.txtTo4.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ip_change);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtFrom4);
            this.groupBox1.Controls.Add(this.txtFrom1);
            this.groupBox1.Controls.Add(this.txtFrom2);
            this.groupBox1.Controls.Add(this.txtFrom3);
            this.groupBox1.Location = new System.Drawing.Point(12, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(191, 42);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "From";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtTo4);
            this.groupBox2.Controls.Add(this.txtTo1);
            this.groupBox2.Controls.Add(this.txtTo2);
            this.groupBox2.Controls.Add(this.txtTo3);
            this.groupBox2.Location = new System.Drawing.Point(219, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(191, 42);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "To";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtNmapArgs);
            this.groupBox3.Location = new System.Drawing.Point(425, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(360, 42);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Nmap CmdLine Options Override (default: -sn)";
            // 
            // txtNmapArgs
            // 
            this.txtNmapArgs.Location = new System.Drawing.Point(6, 16);
            this.txtNmapArgs.Name = "txtNmapArgs";
            this.txtNmapArgs.Size = new System.Drawing.Size(348, 20);
            this.txtNmapArgs.TabIndex = 8;
            // 
            // lvScans
            // 
            this.lvScans.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvScans.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.IP,
            this.Results});
            this.lvScans.HideSelection = false;
            this.lvScans.Location = new System.Drawing.Point(12, 107);
            this.lvScans.Name = "lvScans";
            this.lvScans.Size = new System.Drawing.Size(776, 340);
            this.lvScans.TabIndex = 4;
            this.lvScans.UseCompatibleStateImageBehavior = false;
            this.lvScans.View = System.Windows.Forms.View.Details;
            // 
            // IP
            // 
            this.IP.Text = "IP";
            this.IP.Width = 100;
            // 
            // Results
            // 
            this.Results.Text = "Results";
            this.Results.Width = 600;
            // 
            // btnStartStop
            // 
            this.btnStartStop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartStop.Font = new System.Drawing.Font("Segoe UI Historic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartStop.ForeColor = System.Drawing.Color.Green;
            this.btnStartStop.Location = new System.Drawing.Point(586, 53);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(199, 48);
            this.btnStartStop.TabIndex = 12;
            this.btnStartStop.Text = "Start";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.numThreads);
            this.groupBox4.Location = new System.Drawing.Point(12, 59);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(191, 42);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Number of Threads";
            // 
            // numThreads
            // 
            this.numThreads.Location = new System.Drawing.Point(76, 16);
            this.numThreads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numThreads.Name = "numThreads";
            this.numThreads.Size = new System.Drawing.Size(91, 20);
            this.numThreads.TabIndex = 9;
            this.numThreads.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tstProgress,
            this.tstStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 471);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tstProgress
            // 
            this.tstProgress.Name = "tstProgress";
            this.tstProgress.Size = new System.Drawing.Size(500, 16);
            this.tstProgress.Step = 1;
            // 
            // tstStatus
            // 
            this.tstStatus.Name = "tstStatus";
            this.tstStatus.Size = new System.Drawing.Size(32, 17);
            this.tstStatus.Text = "Idle..";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.hostsPerThread);
            this.groupBox5.Location = new System.Drawing.Point(219, 59);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(191, 42);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Hosts Per Thread";
            // 
            // hostsPerThread
            // 
            this.hostsPerThread.Location = new System.Drawing.Point(76, 16);
            this.hostsPerThread.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.hostsPerThread.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.hostsPerThread.Name = "hostsPerThread";
            this.hostsPerThread.Size = new System.Drawing.Size(91, 20);
            this.hostsPerThread.TabIndex = 10;
            this.hostsPerThread.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // cbShowOffline
            // 
            this.cbShowOffline.AutoSize = true;
            this.cbShowOffline.Location = new System.Drawing.Point(425, 76);
            this.cbShowOffline.Name = "cbShowOffline";
            this.cbShowOffline.Size = new System.Drawing.Size(116, 17);
            this.cbShowOffline.TabIndex = 11;
            this.cbShowOffline.Text = "Show Offline Hosts";
            this.cbShowOffline.UseVisualStyleBackColor = true;
            // 
            // NmapMT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 493);
            this.Controls.Add(this.cbShowOffline);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.lvScans);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(816, 532);
            this.Name = "NmapMT";
            this.Text = "NmapMT";
            this.Load += new System.EventHandler(this.NmapMT_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numThreads)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.hostsPerThread)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFrom1;
        private System.Windows.Forms.TextBox txtFrom2;
        private System.Windows.Forms.TextBox txtFrom3;
        private System.Windows.Forms.TextBox txtFrom4;
        private System.Windows.Forms.TextBox txtTo1;
        private System.Windows.Forms.TextBox txtTo2;
        private System.Windows.Forms.TextBox txtTo3;
        private System.Windows.Forms.TextBox txtTo4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtNmapArgs;
        private System.Windows.Forms.ListView lvScans;
        private System.Windows.Forms.ColumnHeader IP;
        private System.Windows.Forms.ColumnHeader Results;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.NumericUpDown numThreads;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar tstProgress;
        private System.Windows.Forms.ToolStripStatusLabel tstStatus;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.NumericUpDown hostsPerThread;
        private System.Windows.Forms.CheckBox cbShowOffline;
    }
}

