namespace CanStick {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mnu = new System.Windows.Forms.ToolStrip();
            this.mnuNew = new System.Windows.Forms.ToolStripButton();
            this.mnuCopy = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuPorts = new System.Windows.Forms.ToolStripComboBox();
            this.mnuConnect = new System.Windows.Forms.ToolStripButton();
            this.mnuPause = new System.Windows.Forms.ToolStripButton();
            this.mnuDisconnect = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuListClear = new System.Windows.Forms.ToolStripButton();
            this.mnuListEnd = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSend = new System.Windows.Forms.ToolStripButton();
            this.mnuApp = new System.Windows.Forms.ToolStripDropDownButton();
            this.mnuAppFeedback = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAppUpgrade = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAppDonate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuAppAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.lsvMessages = new CanStick.ListViewEx();
            this.lsvMessages_colID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lsvMessages_colData = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lsvMessages_colFlags = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sta = new System.Windows.Forms.StatusStrip();
            this.staStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.staPower = new System.Windows.Forms.ToolStripStatusLabel();
            this.staTermination = new System.Windows.Forms.ToolStripStatusLabel();
            this.staTxStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.staRxStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.staRxOverflowStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.staMessagesPerSecond = new System.Windows.Forms.ToolStripStatusLabel();
            this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
            this.bwDevice = new System.ComponentModel.BackgroundWorker();
            this.mnxPower = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnxPowerOn = new System.Windows.Forms.ToolStripMenuItem();
            this.mnxPowerOff = new System.Windows.Forms.ToolStripMenuItem();
            this.mnxTermination = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnxTerminationOn = new System.Windows.Forms.ToolStripMenuItem();
            this.mnxTerminationOff = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu.SuspendLayout();
            this.sta.SuspendLayout();
            this.mnxPower.SuspendLayout();
            this.mnxTermination.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnu
            // 
            this.mnu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.mnu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNew,
            this.mnuCopy,
            this.toolStripSeparator2,
            this.mnuPorts,
            this.mnuConnect,
            this.mnuPause,
            this.mnuDisconnect,
            this.toolStripSeparator1,
            this.mnuListClear,
            this.mnuListEnd,
            this.toolStripSeparator3,
            this.mnuSend,
            this.mnuApp});
            this.mnu.Location = new System.Drawing.Point(0, 0);
            this.mnu.Name = "mnu";
            this.mnu.Padding = new System.Windows.Forms.Padding(1, 0, 1, 1);
            this.mnu.Size = new System.Drawing.Size(462, 29);
            this.mnu.TabIndex = 0;
            // 
            // mnuNew
            // 
            this.mnuNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuNew.Enabled = false;
            this.mnuNew.Image = ((System.Drawing.Image)(resources.GetObject("mnuNew.Image")));
            this.mnuNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuNew.Name = "mnuNew";
            this.mnuNew.Size = new System.Drawing.Size(23, 25);
            this.mnuNew.Text = "New";
            this.mnuNew.ToolTipText = "New (Ctrl+N)";
            this.mnuNew.Click += new System.EventHandler(this.mnuNew_Click);
            // 
            // mnuCopy
            // 
            this.mnuCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuCopy.Enabled = false;
            this.mnuCopy.Image = ((System.Drawing.Image)(resources.GetObject("mnuCopy.Image")));
            this.mnuCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuCopy.Name = "mnuCopy";
            this.mnuCopy.Size = new System.Drawing.Size(23, 25);
            this.mnuCopy.Text = "Copy (Ctrl+C)";
            this.mnuCopy.Click += new System.EventHandler(this.mnuCopy_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 28);
            // 
            // mnuPorts
            // 
            this.mnuPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mnuPorts.Name = "mnuPorts";
            this.mnuPorts.Size = new System.Drawing.Size(121, 28);
            // 
            // mnuConnect
            // 
            this.mnuConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuConnect.Image = ((System.Drawing.Image)(resources.GetObject("mnuConnect.Image")));
            this.mnuConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuConnect.Name = "mnuConnect";
            this.mnuConnect.Size = new System.Drawing.Size(23, 25);
            this.mnuConnect.Text = "Connect";
            this.mnuConnect.ToolTipText = "Connect (F8)";
            this.mnuConnect.Click += new System.EventHandler(this.mnuConnect_Click);
            // 
            // mnuPause
            // 
            this.mnuPause.CheckOnClick = true;
            this.mnuPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuPause.Enabled = false;
            this.mnuPause.Image = ((System.Drawing.Image)(resources.GetObject("mnuPause.Image")));
            this.mnuPause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuPause.Name = "mnuPause";
            this.mnuPause.Size = new System.Drawing.Size(23, 25);
            this.mnuPause.Text = "Pause";
            this.mnuPause.CheckStateChanged += new System.EventHandler(this.mnuPause_CheckStateChanged);
            // 
            // mnuDisconnect
            // 
            this.mnuDisconnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuDisconnect.Enabled = false;
            this.mnuDisconnect.Image = ((System.Drawing.Image)(resources.GetObject("mnuDisconnect.Image")));
            this.mnuDisconnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuDisconnect.Name = "mnuDisconnect";
            this.mnuDisconnect.Size = new System.Drawing.Size(23, 25);
            this.mnuDisconnect.Text = "Disconnect";
            this.mnuDisconnect.Click += new System.EventHandler(this.mnuDisconnect_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 28);
            // 
            // mnuListClear
            // 
            this.mnuListClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuListClear.Enabled = false;
            this.mnuListClear.Image = ((System.Drawing.Image)(resources.GetObject("mnuListClear.Image")));
            this.mnuListClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuListClear.Name = "mnuListClear";
            this.mnuListClear.Size = new System.Drawing.Size(23, 25);
            this.mnuListClear.Text = "Clear list";
            this.mnuListClear.Click += new System.EventHandler(this.mnuListClear_Click);
            // 
            // mnuListEnd
            // 
            this.mnuListEnd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuListEnd.Enabled = false;
            this.mnuListEnd.Image = ((System.Drawing.Image)(resources.GetObject("mnuListEnd.Image")));
            this.mnuListEnd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuListEnd.Name = "mnuListEnd";
            this.mnuListEnd.Size = new System.Drawing.Size(23, 25);
            this.mnuListEnd.Text = "Move to end";
            this.mnuListEnd.Click += new System.EventHandler(this.mnuListEnd_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 28);
            // 
            // mnuSend
            // 
            this.mnuSend.Enabled = false;
            this.mnuSend.Image = ((System.Drawing.Image)(resources.GetObject("mnuSend.Image")));
            this.mnuSend.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuSend.Name = "mnuSend";
            this.mnuSend.Size = new System.Drawing.Size(62, 25);
            this.mnuSend.Text = "Send";
            this.mnuSend.ToolTipText = "Send (F5)";
            // 
            // mnuApp
            // 
            this.mnuApp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.mnuApp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuApp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAppFeedback,
            this.mnuAppUpgrade,
            this.mnuAppDonate,
            this.toolStripMenuItem1,
            this.mnuAppAbout});
            this.mnuApp.Image = ((System.Drawing.Image)(resources.GetObject("mnuApp.Image")));
            this.mnuApp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuApp.Name = "mnuApp";
            this.mnuApp.Size = new System.Drawing.Size(29, 25);
            this.mnuApp.Text = "Application";
            // 
            // mnuAppFeedback
            // 
            this.mnuAppFeedback.Name = "mnuAppFeedback";
            this.mnuAppFeedback.Size = new System.Drawing.Size(206, 24);
            this.mnuAppFeedback.Text = "Send &feedback";
            this.mnuAppFeedback.Click += new System.EventHandler(this.mnuAppFeedback_Click);
            // 
            // mnuAppUpgrade
            // 
            this.mnuAppUpgrade.Name = "mnuAppUpgrade";
            this.mnuAppUpgrade.Size = new System.Drawing.Size(206, 24);
            this.mnuAppUpgrade.Text = "Check for &upgrades";
            this.mnuAppUpgrade.Click += new System.EventHandler(this.mnuAppUpgrade_Click);
            // 
            // mnuAppDonate
            // 
            this.mnuAppDonate.Name = "mnuAppDonate";
            this.mnuAppDonate.Size = new System.Drawing.Size(206, 24);
            this.mnuAppDonate.Text = "&Donate";
            this.mnuAppDonate.Click += new System.EventHandler(this.mnuAppDonate_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(203, 6);
            // 
            // mnuAppAbout
            // 
            this.mnuAppAbout.Name = "mnuAppAbout";
            this.mnuAppAbout.Size = new System.Drawing.Size(206, 24);
            this.mnuAppAbout.Text = "&About";
            this.mnuAppAbout.Click += new System.EventHandler(this.mnuAppAbout_Click);
            // 
            // lsvMessages
            // 
            this.lsvMessages.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lsvMessages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lsvMessages_colID,
            this.lsvMessages_colData,
            this.lsvMessages_colFlags});
            this.lsvMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvMessages.FullRowSelect = true;
            this.lsvMessages.GridLines = true;
            this.lsvMessages.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lsvMessages.HideSelection = false;
            this.lsvMessages.Location = new System.Drawing.Point(0, 29);
            this.lsvMessages.Name = "lsvMessages";
            this.lsvMessages.Size = new System.Drawing.Size(462, 299);
            this.lsvMessages.TabIndex = 1;
            this.lsvMessages.UseCompatibleStateImageBehavior = false;
            this.lsvMessages.View = System.Windows.Forms.View.Details;
            // 
            // lsvMessages_colID
            // 
            this.lsvMessages_colID.Text = "ID";
            this.lsvMessages_colID.Width = 120;
            // 
            // lsvMessages_colData
            // 
            this.lsvMessages_colData.Text = "Data";
            this.lsvMessages_colData.Width = 240;
            // 
            // lsvMessages_colFlags
            // 
            this.lsvMessages_colFlags.Text = "Flags";
            // 
            // sta
            // 
            this.sta.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.staStatus,
            this.staPower,
            this.staTermination,
            this.staTxStatus,
            this.staRxStatus,
            this.staRxOverflowStatus,
            this.staMessagesPerSecond});
            this.sta.Location = new System.Drawing.Point(0, 328);
            this.sta.Name = "sta";
            this.sta.ShowItemToolTips = true;
            this.sta.Size = new System.Drawing.Size(462, 25);
            this.sta.TabIndex = 2;
            // 
            // staStatus
            // 
            this.staStatus.Name = "staStatus";
            this.staStatus.Size = new System.Drawing.Size(357, 20);
            this.staStatus.Spring = true;
            this.staStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.staStatus.ToolTipText = "Status";
            // 
            // staPower
            // 
            this.staPower.Name = "staPower";
            this.staPower.Size = new System.Drawing.Size(15, 20);
            this.staPower.Text = "-";
            this.staPower.ToolTipText = "Power output status";
            this.staPower.MouseDown += new System.Windows.Forms.MouseEventHandler(this.staPower_MouseDown);
            // 
            // staTermination
            // 
            this.staTermination.Name = "staTermination";
            this.staTermination.Size = new System.Drawing.Size(15, 20);
            this.staTermination.Text = "-";
            this.staTermination.ToolTipText = "Termination status";
            this.staTermination.MouseDown += new System.Windows.Forms.MouseEventHandler(this.staTermination_MouseDown);
            // 
            // staTxStatus
            // 
            this.staTxStatus.Name = "staTxStatus";
            this.staTxStatus.Size = new System.Drawing.Size(15, 20);
            this.staTxStatus.Text = "-";
            this.staTxStatus.ToolTipText = "Transmitter status";
            // 
            // staRxStatus
            // 
            this.staRxStatus.Name = "staRxStatus";
            this.staRxStatus.Size = new System.Drawing.Size(15, 20);
            this.staRxStatus.Text = "-";
            this.staRxStatus.ToolTipText = "Receiver status";
            // 
            // staRxOverflowStatus
            // 
            this.staRxOverflowStatus.Name = "staRxOverflowStatus";
            this.staRxOverflowStatus.Size = new System.Drawing.Size(15, 20);
            this.staRxOverflowStatus.Text = "-";
            this.staRxOverflowStatus.ToolTipText = "Receiver overflow status";
            // 
            // staMessagesPerSecond
            // 
            this.staMessagesPerSecond.Name = "staMessagesPerSecond";
            this.staMessagesPerSecond.Size = new System.Drawing.Size(15, 20);
            this.staMessagesPerSecond.Text = "-";
            // 
            // tmrRefresh
            // 
            this.tmrRefresh.Enabled = true;
            this.tmrRefresh.Interval = 1000;
            this.tmrRefresh.Tick += new System.EventHandler(this.tmrRefresh_Tick);
            // 
            // bwDevice
            // 
            this.bwDevice.WorkerReportsProgress = true;
            this.bwDevice.WorkerSupportsCancellation = true;
            this.bwDevice.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwDevice_DoWork);
            this.bwDevice.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwDevice_ProgressChanged);
            this.bwDevice.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwDevice_RunWorkerCompleted);
            // 
            // mnxPower
            // 
            this.mnxPower.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnxPowerOn,
            this.mnxPowerOff});
            this.mnxPower.Name = "mnxPower";
            this.mnxPower.Size = new System.Drawing.Size(177, 52);
            this.mnxPower.Opening += new System.ComponentModel.CancelEventHandler(this.mnxPower_Opening);
            // 
            // mnxPowerOn
            // 
            this.mnxPowerOn.Name = "mnxPowerOn";
            this.mnxPowerOn.Size = new System.Drawing.Size(176, 24);
            this.mnxPowerOn.Text = "Turn power on";
            this.mnxPowerOn.Click += new System.EventHandler(this.mnxPowerOn_Click);
            // 
            // mnxPowerOff
            // 
            this.mnxPowerOff.Name = "mnxPowerOff";
            this.mnxPowerOff.Size = new System.Drawing.Size(176, 24);
            this.mnxPowerOff.Text = "Turn power off";
            this.mnxPowerOff.Click += new System.EventHandler(this.mnxPowerOff_Click);
            // 
            // mnxTermination
            // 
            this.mnxTermination.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnxTerminationOn,
            this.mnxTerminationOff});
            this.mnxTermination.Name = "mnxTermination";
            this.mnxTermination.Size = new System.Drawing.Size(212, 52);
            this.mnxTermination.Opening += new System.ComponentModel.CancelEventHandler(this.mnxTermination_Opening);
            // 
            // mnxTerminationOn
            // 
            this.mnxTerminationOn.Name = "mnxTerminationOn";
            this.mnxTerminationOn.Size = new System.Drawing.Size(211, 24);
            this.mnxTerminationOn.Text = "Turn termination on";
            this.mnxTerminationOn.Click += new System.EventHandler(this.mnxTerminationOn_Click);
            // 
            // mnxTerminationOff
            // 
            this.mnxTerminationOff.Name = "mnxTerminationOff";
            this.mnxTerminationOff.Size = new System.Drawing.Size(211, 24);
            this.mnxTerminationOff.Text = "Turn termination off";
            this.mnxTerminationOff.Click += new System.EventHandler(this.mnxTerminationOff_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 353);
            this.Controls.Add(this.lsvMessages);
            this.Controls.Add(this.sta);
            this.Controls.Add(this.mnu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(400, 320);
            this.Name = "MainForm";
            this.Text = "CanStick";
            this.mnu.ResumeLayout(false);
            this.mnu.PerformLayout();
            this.sta.ResumeLayout(false);
            this.sta.PerformLayout();
            this.mnxPower.ResumeLayout(false);
            this.mnxTermination.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip mnu;
        private System.Windows.Forms.ToolStripComboBox mnuPorts;
        private System.Windows.Forms.ToolStripButton mnuConnect;
        private System.Windows.Forms.ToolStripButton mnuDisconnect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton mnuSend;
        private System.Windows.Forms.ToolStripButton mnuNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton mnuCopy;
        private ListViewEx lsvMessages;
        private System.Windows.Forms.ColumnHeader lsvMessages_colID;
        private System.Windows.Forms.ColumnHeader lsvMessages_colData;
        private System.Windows.Forms.ColumnHeader lsvMessages_colFlags;
        private System.Windows.Forms.ToolStripDropDownButton mnuApp;
        private System.Windows.Forms.StatusStrip sta;
        private System.Windows.Forms.ToolStripStatusLabel staStatus;
        private System.Windows.Forms.ToolStripStatusLabel staRxStatus;
        private System.Windows.Forms.Timer tmrRefresh;
        private System.Windows.Forms.ToolStripMenuItem mnuAppFeedback;
        private System.Windows.Forms.ToolStripMenuItem mnuAppUpgrade;
        private System.Windows.Forms.ToolStripMenuItem mnuAppDonate;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuAppAbout;
        private System.Windows.Forms.ToolStripStatusLabel staTxStatus;
        private System.ComponentModel.BackgroundWorker bwDevice;
        private System.Windows.Forms.ToolStripStatusLabel staPower;
        private System.Windows.Forms.ToolStripStatusLabel staTermination;
        private System.Windows.Forms.ToolStripStatusLabel staRxOverflowStatus;
        private System.Windows.Forms.ContextMenuStrip mnxPower;
        private System.Windows.Forms.ToolStripMenuItem mnxPowerOn;
        private System.Windows.Forms.ToolStripMenuItem mnxPowerOff;
        private System.Windows.Forms.ContextMenuStrip mnxTermination;
        private System.Windows.Forms.ToolStripMenuItem mnxTerminationOn;
        private System.Windows.Forms.ToolStripMenuItem mnxTerminationOff;
        private System.Windows.Forms.ToolStripStatusLabel staMessagesPerSecond;
        private System.Windows.Forms.ToolStripButton mnuPause;
        private System.Windows.Forms.ToolStripButton mnuListClear;
        private System.Windows.Forms.ToolStripButton mnuListEnd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}

