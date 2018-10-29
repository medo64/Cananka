using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Medo.Device;

namespace CanankaTest {
    internal partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
            this.Font = SystemFonts.MessageBoxFont;

            mnu.Renderer = Helpers.ToolStripBorderlessSystemRendererInstance;
            Helpers.ScaleToolstrip(mnu);

            Medo.Windows.Forms.State.SetupOnLoadAndClose(this, lsvMessages);

            tmrRefresh_Tick(null, null);
        }


        private const int MaxSelectAllItemsCount = 10000;

        #region Menu

        private bool SuppressMenuKey = false;

        protected override bool ProcessDialogKey(Keys keyData) {
            if (((keyData & Keys.Alt) == Keys.Alt) && (keyData != (Keys.Alt | Keys.Menu))) { this.SuppressMenuKey = true; }

            switch (keyData) {

                case Keys.F1:
                    mnuApp.ShowDropDown();
                    mnuAppAbout.Select();
                    return true;

                case Keys.F10:
                    ToggleMenu();
                    return true;

                case Keys.Control | Keys.A:
                    if (lsvMessages.Items.Count < MaxSelectAllItemsCount) {
                        lsvMessages.BeginUpdate();
                        foreach (ListViewItem item in lsvMessages.Items) {
                            item.Selected = true;
                        }
                        lsvMessages.EndUpdate();
                    }
                    return true;

                case Keys.Control | Keys.N:
                    mnuNew.PerformClick();
                    return true;

                case Keys.Control | Keys.C:
                    mnuCopy.PerformClick();
                    return true;

            }

            return base.ProcessDialogKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            if (e.KeyData == Keys.Menu) {
                if (this.SuppressMenuKey) { this.SuppressMenuKey = false; return; }
                ToggleMenu();
                e.Handled = true;
                e.SuppressKeyPress = true;
            } else {
                base.OnKeyDown(e);
            }
        }

        protected override void OnKeyUp(KeyEventArgs e) {
            if (e.KeyData == Keys.Menu) {
                if (this.SuppressMenuKey) { this.SuppressMenuKey = false; return; }
                ToggleMenu();
                e.Handled = true;
                e.SuppressKeyPress = true;
            } else {
                base.OnKeyUp(e);
            }
        }


        private void ToggleMenu() {
            if (mnu.ContainsFocus) {
                lsvMessages.Select();
            } else {
                mnu.Select();
                mnu.Items[0].Select();
            }
        }

        #endregion


        #region Document

        private Cananka Document = null;

        private CanankaVersion Version = null;
        private bool IsCananka = false;
        private bool SupportsPower = false;
        private bool SupportsTermination = false;
        private bool SupportsLoad = false;
        private bool LastPowerState = false;
        private bool LastTerminationState = false;
        private bool LastLoadState = false;

        private void bwDevice_DoWork(object sender, DoWorkEventArgs e) {
            var device = (Cananka)e.Argument;

            try {
                var stopwatch = Stopwatch.StartNew();
                var skipIndex = 0; //to interleave flag checks
                while (true) {
                    if (stopwatch.ElapsedMilliseconds > 500) {
                        if (skipIndex == 0) {
                            if (bwDevice.CancellationPending) { break; }
                            bwDevice.ReportProgress(-1, device.GetStatus());
                        } else if (skipIndex == 1) {
                            if (bwDevice.CancellationPending) { break; }
                            bwDevice.ReportProgress(-1, device.GetExtendedStatus());
                        }
                        skipIndex = (skipIndex + 1) % 2;

                        stopwatch.Reset();
                        stopwatch.Start();
                    }
                }
            } catch (Exception ex) {
                try {
                    device.Close();
                } catch (Exception) { }
                Debug.WriteLine(ex.Message);
                e.Cancel = true;
            }

            if (bwDevice.CancellationPending) { e.Cancel = true; }
        }

        private void bwDevice_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            var status = e.UserState as CanankaStatus;
            var extendedStatus = e.UserState as CanankaExtendedStatus;

            if ((extendedStatus != null) && extendedStatus.IsValid) {
                this.LastPowerState = extendedStatus.PowerEnabled;
                this.LastTerminationState = extendedStatus.TerminationEnabled;
                this.LastLoadState = extendedStatus.AnyLoad;

                var newPowerColor = this.LastPowerState ? SystemColors.WindowText : SystemColors.GrayText;
                if (staPower.ForeColor != newPowerColor) { staPower.ForeColor = newPowerColor; }

                var newTerminationColor = this.LastTerminationState ? SystemColors.WindowText : SystemColors.GrayText;
                if (staTermination.ForeColor != newTerminationColor) { staTermination.ForeColor = newTerminationColor; }

                var newLoadColor = this.LastLoadState ? SystemColors.WindowText : SystemColors.GrayText;
                if (staLoad.ForeColor != newLoadColor) { staLoad.ForeColor = newLoadColor; }
            } else if ((status != null) && status.IsValid) {
                staRxQueueFull.Visible = status.RxQueueFull;
                staTxQueueFull.Visible = status.TxQueueFull;
                staTxRxWarning.Visible = status.TxRxWarning;
                staRxOverflow.Visible = status.RxOverflow;
                staErrorPassive.Visible = status.ErrorPassive;
                staArbitationLost.Visible = status.ArbitrationLost;
                staBusError.Visible = status.BusError;
            }

            ProcessMenuState();
        }

        private void bwDevice_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            this.Document = null;
            ProcessMenuState();
        }


        private void Document_MessageArrived(object sender, CanankaMessageEventArgs e) {
            var message = e.Message;
            Interlocked.Increment(ref this.MessageCount);

            var lvi = new ListViewItem(DateTime.Now.ToLongTimeString());
            lvi.SubItems.Add(message.IsExtended ? message.Id.ToString("X8") : message.Id.ToString("X3"));
            if (!message.IsRemoteRequest) {
                lvi.SubItems.Add(System.BitConverter.ToString(message.GetData()));
                lvi.SubItems.Add("");
            } else {
                lvi.SubItems.Add("");
                lvi.SubItems.Add("R");
            }

            var isLast = (lsvMessages.SelectedIndices.Count == 0);
            lsvMessages.BeginUpdate();
            lsvMessages.Items.Add(lvi);
            if (isLast) { lsvMessages.EnsureVisible(lsvMessages.Items.Count - 1); }
            lsvMessages.EndUpdate();
        }

        #endregion


        #region Menu

        private void mnuNew_Click(object sender, EventArgs e) {
            lsvMessages.BeginUpdate();
            lsvMessages.Items.Clear();
            lsvMessages.EndUpdate();
            ProcessMenuState();
        }

        private void mnuCopy_Click(object sender, EventArgs e) {
            var sb = new StringBuilder();
            for (var i = lsvMessages.SelectedItems.Count - 1; i >= 0; i++) {
                var id = lsvMessages.SelectedItems[i].Text;
                var data = lsvMessages.SelectedItems[i].SubItems[1].Text;
                var flags = lsvMessages.SelectedItems[i].SubItems[2].Text;
                if (string.IsNullOrEmpty(lsvMessages.SelectedItems[2].Text)) {
                    sb.AppendLine(string.Format(CultureInfo.CurrentCulture, "{0}\t{1}", id, data));
                } else {
                    sb.AppendLine(string.Format(CultureInfo.CurrentCulture, "{0}\t{1}\t{2}", id, data, flags));
                }
            }

            Clipboard.SetText(sb.ToString(), TextDataFormat.UnicodeText);
        }


        private void mnuConnect_Click(object sender, EventArgs e) {
            var port = mnuPorts.Text;
            if (!string.IsNullOrEmpty(port)) {
                try {
                    if (this.Document != null) { throw new InvalidOperationException("Invalid state."); }
                    if (bwDevice.IsBusy) { throw new InvalidOperationException("Processing still in progress."); }
                    this.Document = new Cananka(port);
                    this.Document.MessageArrived += this.Document_MessageArrived;
                    this.Document.Open();

                    var version = this.Document.GetVersion();
                    var extendedStatus = this.Document.GetExtendedStatus();

                    this.Version = version;
                    this.IsCananka = extendedStatus.IsValid;
                    this.SupportsPower = extendedStatus.IsValid && (version.HardwareVersion.Minor == 2);
                    this.SupportsTermination = extendedStatus.IsValid && (version.HardwareVersion.Minor == 2);
                    this.SupportsLoad = extendedStatus.IsValid;
                    this.LastPowerState = extendedStatus.IsValid && extendedStatus.PowerEnabled;
                    this.LastTerminationState = extendedStatus.IsValid && extendedStatus.TerminationEnabled;
                    this.LastLoadState = extendedStatus.IsValid && extendedStatus.AnyLoad;

                    bwDevice_ProgressChanged(null, new ProgressChangedEventArgs(-1, extendedStatus));

                    staPower.Visible = this.SupportsPower;
                    staTermination.Visible = this.SupportsTermination;
                    staLoad.Visible = this.SupportsLoad;

                    bwDevice.RunWorkerAsync(this.Document);
                } catch (Exception ex) {
                    this.Document = null;
                    Medo.MessageBox.ShowError(this, "Cannot connect to device!\n\n" + ex.Message);
                }
            }
            ProcessMenuState();
        }

        private void mnuDisconnect_Click(object sender, EventArgs e) {
            try {
                if (this.Document == null) { throw new InvalidOperationException("Invalid state."); }
                this.Document.Close();
                this.Document = null;
                bwDevice.CancelAsync();
            } catch (Exception ex) {
                this.Document = null;
                Medo.MessageBox.ShowError(this, "Cannot disconnect device!\n\n" + ex.Message);
            }
            ProcessMenuState();
        }


        private void mnuGotoEnd_Click(object sender, EventArgs e) {
            lsvMessages.SelectedItems.Clear();
            lsvMessages.FocusedItem = null;
            if (lsvMessages.Items.Count > 0) {
                lsvMessages.EnsureVisible(lsvMessages.Items.Count - 1);
            }
        }


        private void mnuSend_Click(object sender, EventArgs e) {
            if ((this.Document != null) && this.Document.IsOpen) {
                using (var frm = new SendMessageForm(this.Document)) {
                    frm.ShowDialog(this);
                }
            }
        }

        private void mnuVersion_Click(object sender, EventArgs e) {
            if (this.Document != null) {
                var version = this.Version;
                if (version.IsValid) {
                    var sb = new StringBuilder();
                    sb.AppendFormat(CultureInfo.CurrentCulture, "Hardware: {0}.{1}", version.HardwareVersion.Major, version.HardwareVersion.Minor);
                    sb.AppendLine();
                    sb.AppendFormat(CultureInfo.CurrentCulture, "Software: {0}.{1}", version.SoftwareVersion.Major, version.SoftwareVersion.Minor);
                    Medo.MessageBox.ShowDialog(this, sb.ToString());
                } else {
                    Medo.MessageBox.ShowWarning(this, "Version not retrieved.");
                }
            }
        }


        private void mnuAppFeedback_Click(object sender, EventArgs e) {

        }

        private void mnuAppUpgrade_Click(object sender, EventArgs e) {

        }

        private void mnuAppDonate_Click(object sender, EventArgs e) {

        }

        private void mnuAppAbout_Click(object sender, EventArgs e) {
            Medo.Windows.Forms.AboutBox.ShowDialog(this, new Uri("http://medo64.com/cananka/usb/"));
        }


        private void mnxPower_Opening(object sender, CancelEventArgs e) {
            mnxPowerOn.Enabled = !this.LastPowerState;
            mnxPowerOff.Enabled = this.LastPowerState;
        }

        private void mnxPowerOff_Click(object sender, EventArgs e) {
            if (this.Document != null) {
                try {
                    this.Document.SetPower(false);
                } catch (Exception) { }
                ProcessMenuState();
            }
        }

        private void mnxPowerOn_Click(object sender, EventArgs e) {
            if (this.Document != null) {
                try {
                    this.LastPowerState = true;
                    this.Document.SetPower(true);
                } catch (Exception) { }
                ProcessMenuState();
            }
        }


        private void mnxTermination_Opening(object sender, CancelEventArgs e) {
            mnxTerminationOn.Enabled = !this.LastTerminationState;
            mnxTerminationOff.Enabled = this.LastTerminationState;
        }

        private void mnxTerminationOff_Click(object sender, EventArgs e) {
            if (this.Document != null) {
                try {
                    this.Document.SetTermination(false);
                } catch (Exception) { }
                ProcessMenuState();
            }
        }

        private void mnxTerminationOn_Click(object sender, EventArgs e) {
            if (this.Document != null) {
                try {
                    this.LastTerminationState = true;
                    this.Document.SetTermination(true);
                } catch (Exception) { }
                ProcessMenuState();
            }
        }


        private void mnxLoad_Opening(object sender, CancelEventArgs e) {
            mnxLoadOff.Enabled = this.LastLoadState;
            mnxLoadOn1.Enabled = !this.LastLoadState;
            mnxLoadOn2.Enabled = !this.LastLoadState;
            mnxLoadOn3.Enabled = !this.LastLoadState;
            mnxLoadOn4.Enabled = !this.LastLoadState;
            mnxLoadOn5.Enabled = !this.LastLoadState;
            mnxLoadOn6.Enabled = !this.LastLoadState;
            mnxLoadOn7.Enabled = !this.LastLoadState;
            mnxLoadOn8.Enabled = !this.LastLoadState;
            mnxLoadOn9.Enabled = !this.LastLoadState;

        }

        private void mnxLoadOff_Click(object sender, EventArgs e) {
            if (this.Document != null) {
                try {
                    this.Document.SetLoad(0);
                } catch (Exception) { }
                ProcessMenuState();
            }
        }

        private void mnxLoadOn1_Click(object sender, EventArgs e) {
            if (this.Document != null) {
                try {
                    this.LastLoadState = true;
                    this.Document.SetLoad(1);
                } catch (Exception) { }
                ProcessMenuState();
            }
        }

        private void mnxLoadOn2_Click(object sender, EventArgs e) {
            if (this.Document != null) {
                try {
                    this.LastLoadState = true;
                    this.Document.SetLoad(2);
                } catch (Exception) { }
                ProcessMenuState();
            }
        }

        private void mnxLoadOn3_Click(object sender, EventArgs e) {
            if (this.Document != null) {
                try {
                    this.LastLoadState = true;
                    this.Document.SetLoad(3);
                } catch (Exception) { }
                ProcessMenuState();
            }
        }

        private void mnxLoadOn4_Click(object sender, EventArgs e) {
            if (this.Document != null) {
                try {
                    this.LastLoadState = true;
                    this.Document.SetLoad(4);
                } catch (Exception) { }
                ProcessMenuState();
            }
        }

        private void mnxLoadOn5_Click(object sender, EventArgs e) {
            if (this.Document != null) {
                try {
                    this.LastLoadState = true;
                    this.Document.SetLoad(5);
                } catch (Exception) { }
                ProcessMenuState();
            }
        }

        private void mnxLoadOn6_Click(object sender, EventArgs e) {
            if (this.Document != null) {
                try {
                    this.LastLoadState = true;
                    this.Document.SetLoad(6);
                } catch (Exception) { }
                ProcessMenuState();
            }
        }

        private void mnxLoadOn7_Click(object sender, EventArgs e) {
            if (this.Document != null) {
                try {
                    this.Document.SetLoad(7);
                    this.LastLoadState = true;
                } catch (Exception) { }
                ProcessMenuState();
            }
        }

        private void mnxLoadOn8_Click(object sender, EventArgs e) {
            if (this.Document != null) {
                try {
                    this.LastLoadState = true;
                    this.Document.SetLoad(8);
                } catch (Exception) { }
                ProcessMenuState();
            }
        }

        private void mnxLoadOn9_Click(object sender, EventArgs e) {
            if (this.Document != null) {
                try {
                    this.LastLoadState = true;
                    this.Document.SetLoad(9);
                } catch (Exception) { }
                ProcessMenuState();
            }
        }

        #endregion


        private void staPower_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) { mnxPower.Show(Cursor.Position); }
        }

        private void staTermination_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) { mnxTermination.Show(Cursor.Position); }
        }

        private void staLoad_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) { mnxLoad.Show(Cursor.Position); }
        }


        private string LastPorts = "-";
        private int MessageCount = 0;
        private DateTime ResetTime = DateTime.UtcNow;

        private void tmrRefresh_Tick(object sender, EventArgs e) {
            var messageCount = Interlocked.Exchange(ref this.MessageCount, 0);
            var timePassed = (DateTime.UtcNow - this.ResetTime).TotalMilliseconds;
            this.ResetTime = DateTime.UtcNow;

            var mps = ((double)messageCount / timePassed) * 1000;
            if (mps > 0) {
                staMessagesPerSecond.Text = mps.ToString("#,##0.0", CultureInfo.CurrentCulture);
            } else {
                staMessagesPerSecond.Text = "";
            }


            if (mnuPorts.DroppedDown) { return; } //don't update while user is selecting port
            if (mnuPorts.Enabled == false) { return; } //don't update when already connected

            var ports = System.IO.Ports.SerialPort.GetPortNames();
            Array.Sort(ports, delegate (string text1, string text2) {
                var value1 = 0;
                foreach (var ch in text1) {
                    if ((ch >= '0') && (ch <= '9')) { value1 = (value1 * 10) + (ch - 0x30); }
                }

                var value2 = 0;
                foreach (var ch in text2) {
                    if ((ch >= '0') && (ch <= '9')) { value2 = (value2 * 10) + (ch - 0x30); }
                }

                if ((value1 > 0) && (value2 > 0)) {
                    return value1 > value2 ? +1 : value1 < value2 ? -1 : 0;
                } else {
                    return string.Compare(text1, text2);
                }
            });
            var nextPorts = String.Join(" ", ports);
            if (nextPorts != LastPorts) {
                var lastSelected = mnuPorts.SelectedItem;
                mnuPorts.BeginUpdate();
                mnuPorts.Items.Clear();
                foreach (var port in ports) {
                    mnuPorts.Items.Add(port);
                }
                mnuPorts.SelectedItem = lastSelected;
                if ((mnuPorts.SelectedItem == null) && (mnuPorts.Items.Count > 0)) { mnuPorts.SelectedItem = mnuPorts.Items[0]; }
                mnuPorts.EndUpdate();

                LastPorts = nextPorts;

                ProcessMenuState();
            }
        }


        private void ProcessMenuState() {
            try {
                var isConnected = (this.Document != null);
                mnuNew.Enabled = (lsvMessages.Items.Count > 0);
                mnuCopy.Enabled = (lsvMessages.SelectedItems.Count > 0);
                mnuPorts.Enabled = (this.Document == null);
                mnuConnect.Enabled = (mnuPorts.SelectedItem != null) && (this.Document == null) && (!bwDevice.IsBusy);
                mnuDisconnect.Enabled = (this.Document != null);
                mnuGotoEnd.Enabled = (this.Document != null);
                mnuSend.Enabled = (this.Document != null);
                mnuVersion.Enabled = (this.Document != null);

                var status = isConnected ? Strings.Connected : Strings.NotConnected;
                if (staStatus.Text != status) {
                    staStatus.Text = status;
                    if (!isConnected) {
                        staPower.Visible = false;
                        staTermination.Visible = false;
                        staLoad.Visible = false;
                        staRxQueueFull.Visible = false;
                        staTxQueueFull.Visible = false;
                        staTxRxWarning.Visible = false;
                        staRxOverflow.Visible = false;
                        staErrorPassive.Visible = false;
                        staArbitationLost.Visible = false;
                        staBusError.Visible = false;
                    }
                }
            } catch (NullReferenceException) { } //when closing form, semi-disposed object sometime do this
        }
    }
}
