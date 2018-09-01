namespace CanankaTest {
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
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuPorts = new System.Windows.Forms.ToolStripComboBox();
            this.sta = new System.Windows.Forms.StatusStrip();
            this.mnxFeatures = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnxPowerOn = new System.Windows.Forms.ToolStripMenuItem();
            this.mnxPowerOff = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnxTerminationOn = new System.Windows.Forms.ToolStripMenuItem();
            this.mnxTerminationOff = new System.Windows.Forms.ToolStripMenuItem();
            this.staStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.staRxQueueFull = new System.Windows.Forms.ToolStripStatusLabel();
            this.staTxQueueFull = new System.Windows.Forms.ToolStripStatusLabel();
            this.staTxRxWarning = new System.Windows.Forms.ToolStripStatusLabel();
            this.staRxOverflow = new System.Windows.Forms.ToolStripStatusLabel();
            this.staErrorPassive = new System.Windows.Forms.ToolStripStatusLabel();
            this.staArbitationLost = new System.Windows.Forms.ToolStripStatusLabel();
            this.staBusError = new System.Windows.Forms.ToolStripStatusLabel();
            this.staPower = new System.Windows.Forms.ToolStripStatusLabel();
            this.staTermination = new System.Windows.Forms.ToolStripStatusLabel();
            this.staMessagesPerSecond = new System.Windows.Forms.ToolStripStatusLabel();
            this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
            this.bwDevice = new System.ComponentModel.BackgroundWorker();
            this.lsvMessages = new CanankaTest.ListViewEx();
            this.lsvMessages_colTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lsvMessages_colID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lsvMessages_colData = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lsvMessages_colFlags = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuNew = new System.Windows.Forms.ToolStripButton();
            this.mnuCopy = new System.Windows.Forms.ToolStripButton();
            this.mnuGotoEnd = new System.Windows.Forms.ToolStripButton();
            this.mnuConnect = new System.Windows.Forms.ToolStripButton();
            this.mnuDisconnect = new System.Windows.Forms.ToolStripButton();
            this.mnuApp = new System.Windows.Forms.ToolStripDropDownButton();
            this.mnuAppFeedback = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAppUpgrade = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAppDonate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuAppAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSend = new System.Windows.Forms.ToolStripButton();
            this.mnu.SuspendLayout();
            this.sta.SuspendLayout();
            this.mnxFeatures.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnu
            // 
            this.mnu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.mnu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mnu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNew,
            this.mnuCopy,
            this.mnuGotoEnd,
            this.toolStripSeparator2,
            this.mnuPorts,
            this.mnuConnect,
            this.mnuDisconnect,
            this.mnuApp,
            this.toolStripSeparator1,
            this.mnuSend});
            this.mnu.Location = new System.Drawing.Point(0, 0);
            this.mnu.Name = "mnu";
            this.mnu.Padding = new System.Windows.Forms.Padding(1, 0, 1, 1);
            this.mnu.Size = new System.Drawing.Size(602, 29);
            this.mnu.TabIndex = 0;
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
            // sta
            // 
            this.sta.ContextMenuStrip = this.mnxFeatures;
            this.sta.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.sta.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.staStatus,
            this.staRxQueueFull,
            this.staTxQueueFull,
            this.staTxRxWarning,
            this.staRxOverflow,
            this.staErrorPassive,
            this.staArbitationLost,
            this.staBusError,
            this.staPower,
            this.staTermination,
            this.staMessagesPerSecond});
            this.sta.Location = new System.Drawing.Point(0, 328);
            this.sta.Name = "sta";
            this.sta.ShowItemToolTips = true;
            this.sta.Size = new System.Drawing.Size(602, 25);
            this.sta.TabIndex = 2;
            // 
            // mnxFeatures
            // 
            this.mnxFeatures.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mnxFeatures.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnxPowerOn,
            this.mnxPowerOff,
            this.toolStripMenuItem2,
            this.mnxTerminationOn,
            this.mnxTerminationOff});
            this.mnxFeatures.Name = "mnxPower";
            this.mnxFeatures.Size = new System.Drawing.Size(212, 106);
            this.mnxFeatures.Opening += new System.ComponentModel.CancelEventHandler(this.mnxFeatures_Opening);
            // 
            // mnxPowerOn
            // 
            this.mnxPowerOn.Name = "mnxPowerOn";
            this.mnxPowerOn.Size = new System.Drawing.Size(211, 24);
            this.mnxPowerOn.Text = "Turn power on";
            this.mnxPowerOn.Click += new System.EventHandler(this.mnxPowerOn_Click);
            // 
            // mnxPowerOff
            // 
            this.mnxPowerOff.Name = "mnxPowerOff";
            this.mnxPowerOff.Size = new System.Drawing.Size(211, 24);
            this.mnxPowerOff.Text = "Turn power off";
            this.mnxPowerOff.Click += new System.EventHandler(this.mnxPowerOff_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(208, 6);
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
            // staStatus
            // 
            this.staStatus.Name = "staStatus";
            this.staStatus.Size = new System.Drawing.Size(572, 20);
            this.staStatus.Spring = true;
            this.staStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.staStatus.ToolTipText = "Status";
            // 
            // staRxQueueFull
            // 
            this.staRxQueueFull.Name = "staRxQueueFull";
            this.staRxQueueFull.Size = new System.Drawing.Size(18, 20);
            this.staRxQueueFull.Text = "R";
            this.staRxQueueFull.ToolTipText = "Rx queue full";
            this.staRxQueueFull.Visible = false;
            // 
            // staTxQueueFull
            // 
            this.staTxQueueFull.Name = "staTxQueueFull";
            this.staTxQueueFull.Size = new System.Drawing.Size(17, 20);
            this.staTxQueueFull.Text = "T";
            this.staTxQueueFull.ToolTipText = "Tx queue full";
            this.staTxQueueFull.Visible = false;
            // 
            // staTxRxWarning
            // 
            this.staTxRxWarning.Name = "staTxRxWarning";
            this.staTxRxWarning.Size = new System.Drawing.Size(23, 20);
            this.staTxRxWarning.Text = "W";
            this.staTxRxWarning.ToolTipText = "Tx/Rx warning";
            this.staTxRxWarning.Visible = false;
            // 
            // staRxOverflow
            // 
            this.staRxOverflow.Name = "staRxOverflow";
            this.staRxOverflow.Size = new System.Drawing.Size(20, 20);
            this.staRxOverflow.Text = "O";
            this.staRxOverflow.ToolTipText = "Rx overflow";
            this.staRxOverflow.Visible = false;
            // 
            // staErrorPassive
            // 
            this.staErrorPassive.Name = "staErrorPassive";
            this.staErrorPassive.Size = new System.Drawing.Size(17, 20);
            this.staErrorPassive.Text = "P";
            this.staErrorPassive.ToolTipText = "Error-passive";
            this.staErrorPassive.Visible = false;
            // 
            // staArbitationLost
            // 
            this.staArbitationLost.Name = "staArbitationLost";
            this.staArbitationLost.Size = new System.Drawing.Size(19, 20);
            this.staArbitationLost.Text = "A";
            this.staArbitationLost.ToolTipText = "Arbitration lost";
            this.staArbitationLost.Visible = false;
            // 
            // staBusError
            // 
            this.staBusError.Name = "staBusError";
            this.staBusError.Size = new System.Drawing.Size(17, 20);
            this.staBusError.Text = "E";
            this.staBusError.ToolTipText = "Bus error";
            this.staBusError.Visible = false;
            // 
            // staPower
            // 
            this.staPower.Name = "staPower";
            this.staPower.Size = new System.Drawing.Size(18, 20);
            this.staPower.Text = "p";
            this.staPower.ToolTipText = "Power output status";
            this.staPower.Visible = false;
            // 
            // staTermination
            // 
            this.staTermination.Name = "staTermination";
            this.staTermination.Size = new System.Drawing.Size(14, 20);
            this.staTermination.Text = "t";
            this.staTermination.ToolTipText = "Termination status";
            this.staTermination.Visible = false;
            // 
            // staMessagesPerSecond
            // 
            this.staMessagesPerSecond.Name = "staMessagesPerSecond";
            this.staMessagesPerSecond.Size = new System.Drawing.Size(15, 20);
            this.staMessagesPerSecond.Text = "-";
            this.staMessagesPerSecond.ToolTipText = "Messages per second";
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
            // lsvMessages
            // 
            this.lsvMessages.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lsvMessages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lsvMessages_colTime,
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
            this.lsvMessages.Size = new System.Drawing.Size(602, 299);
            this.lsvMessages.TabIndex = 1;
            this.lsvMessages.UseCompatibleStateImageBehavior = false;
            this.lsvMessages.View = System.Windows.Forms.View.Details;
            // 
            // lsvMessages_colTime
            // 
            this.lsvMessages_colTime.Text = "Time";
            this.lsvMessages_colTime.Width = 90;
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
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 28);
            // 
            // mnuNew
            // 
            this.mnuNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuNew.Enabled = false;
            this.mnuNew.Image = global::CanankaTest.Properties.Resources.mnuNew_16;
            this.mnuNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuNew.Name = "mnuNew";
            this.mnuNew.Size = new System.Drawing.Size(24, 25);
            this.mnuNew.Text = "New";
            this.mnuNew.ToolTipText = "New (Ctrl+N)";
            this.mnuNew.Click += new System.EventHandler(this.mnuNew_Click);
            // 
            // mnuCopy
            // 
            this.mnuCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuCopy.Enabled = false;
            this.mnuCopy.Image = global::CanankaTest.Properties.Resources.mnuCopy_16;
            this.mnuCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuCopy.Name = "mnuCopy";
            this.mnuCopy.Size = new System.Drawing.Size(24, 25);
            this.mnuCopy.Text = "Copy (Ctrl+C)";
            this.mnuCopy.Click += new System.EventHandler(this.mnuCopy_Click);
            // 
            // mnuGotoEnd
            // 
            this.mnuGotoEnd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuGotoEnd.Enabled = false;
            this.mnuGotoEnd.Image = global::CanankaTest.Properties.Resources.mnuGotoEnd_16;
            this.mnuGotoEnd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuGotoEnd.Name = "mnuGotoEnd";
            this.mnuGotoEnd.Size = new System.Drawing.Size(24, 25);
            this.mnuGotoEnd.Text = "Move to end";
            this.mnuGotoEnd.Click += new System.EventHandler(this.mnuGotoEnd_Click);
            // 
            // mnuConnect
            // 
            this.mnuConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuConnect.Image = global::CanankaTest.Properties.Resources.mnuConnect_16;
            this.mnuConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuConnect.Name = "mnuConnect";
            this.mnuConnect.Size = new System.Drawing.Size(24, 25);
            this.mnuConnect.Text = "Connect";
            this.mnuConnect.ToolTipText = "Connect (F8)";
            this.mnuConnect.Click += new System.EventHandler(this.mnuConnect_Click);
            // 
            // mnuDisconnect
            // 
            this.mnuDisconnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuDisconnect.Enabled = false;
            this.mnuDisconnect.Image = global::CanankaTest.Properties.Resources.mnuDisconnect_16;
            this.mnuDisconnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuDisconnect.Name = "mnuDisconnect";
            this.mnuDisconnect.Size = new System.Drawing.Size(24, 25);
            this.mnuDisconnect.Text = "Disconnect";
            this.mnuDisconnect.Click += new System.EventHandler(this.mnuDisconnect_Click);
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
            this.mnuApp.Image = global::CanankaTest.Properties.Resources.mnuApp_16;
            this.mnuApp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuApp.Name = "mnuApp";
            this.mnuApp.Size = new System.Drawing.Size(34, 25);
            this.mnuApp.Text = "Application";
            // 
            // mnuAppFeedback
            // 
            this.mnuAppFeedback.Name = "mnuAppFeedback";
            this.mnuAppFeedback.Size = new System.Drawing.Size(212, 26);
            this.mnuAppFeedback.Text = "Send &feedback";
            this.mnuAppFeedback.Click += new System.EventHandler(this.mnuAppFeedback_Click);
            // 
            // mnuAppUpgrade
            // 
            this.mnuAppUpgrade.Name = "mnuAppUpgrade";
            this.mnuAppUpgrade.Size = new System.Drawing.Size(212, 26);
            this.mnuAppUpgrade.Text = "Check for &upgrades";
            this.mnuAppUpgrade.Click += new System.EventHandler(this.mnuAppUpgrade_Click);
            // 
            // mnuAppDonate
            // 
            this.mnuAppDonate.Name = "mnuAppDonate";
            this.mnuAppDonate.Size = new System.Drawing.Size(212, 26);
            this.mnuAppDonate.Text = "&Donate";
            this.mnuAppDonate.Click += new System.EventHandler(this.mnuAppDonate_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(209, 6);
            // 
            // mnuAppAbout
            // 
            this.mnuAppAbout.Name = "mnuAppAbout";
            this.mnuAppAbout.Size = new System.Drawing.Size(212, 26);
            this.mnuAppAbout.Text = "&About";
            this.mnuAppAbout.Click += new System.EventHandler(this.mnuAppAbout_Click);
            // 
            // mnuSend
            // 
            this.mnuSend.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuSend.Image = global::CanankaTest.Properties.Resources.mnuSend_16;
            this.mnuSend.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuSend.Name = "mnuSend";
            this.mnuSend.Size = new System.Drawing.Size(24, 25);
            this.mnuSend.Text = "Send";
            this.mnuSend.ToolTipText = "Send message";
            this.mnuSend.Click += new System.EventHandler(this.mnuSend_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 353);
            this.Controls.Add(this.lsvMessages);
            this.Controls.Add(this.sta);
            this.Controls.Add(this.mnu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(400, 320);
            this.Name = "MainForm";
            this.Text = "Cananka";
            this.mnu.ResumeLayout(false);
            this.mnu.PerformLayout();
            this.sta.ResumeLayout(false);
            this.sta.PerformLayout();
            this.mnxFeatures.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip mnu;
        private System.Windows.Forms.ToolStripComboBox mnuPorts;
        private System.Windows.Forms.ToolStripButton mnuConnect;
        private System.Windows.Forms.ToolStripButton mnuDisconnect;
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
        private System.Windows.Forms.ToolStripStatusLabel staTxQueueFull;
        private System.Windows.Forms.Timer tmrRefresh;
        private System.Windows.Forms.ToolStripMenuItem mnuAppFeedback;
        private System.Windows.Forms.ToolStripMenuItem mnuAppUpgrade;
        private System.Windows.Forms.ToolStripMenuItem mnuAppDonate;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuAppAbout;
        private System.Windows.Forms.ToolStripStatusLabel staRxQueueFull;
        private System.ComponentModel.BackgroundWorker bwDevice;
        private System.Windows.Forms.ToolStripStatusLabel staPower;
        private System.Windows.Forms.ToolStripStatusLabel staTermination;
        private System.Windows.Forms.ToolStripStatusLabel staTxRxWarning;
        private System.Windows.Forms.ContextMenuStrip mnxFeatures;
        private System.Windows.Forms.ToolStripMenuItem mnxPowerOn;
        private System.Windows.Forms.ToolStripMenuItem mnxPowerOff;
        private System.Windows.Forms.ToolStripStatusLabel staMessagesPerSecond;
        private System.Windows.Forms.ToolStripButton mnuGotoEnd;
        private System.Windows.Forms.ToolStripStatusLabel staRxOverflow;
        private System.Windows.Forms.ToolStripStatusLabel staErrorPassive;
        private System.Windows.Forms.ToolStripStatusLabel staArbitationLost;
        private System.Windows.Forms.ToolStripStatusLabel staBusError;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem mnxTerminationOn;
        private System.Windows.Forms.ToolStripMenuItem mnxTerminationOff;
        private System.Windows.Forms.ColumnHeader lsvMessages_colTime;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton mnuSend;
    }
}

