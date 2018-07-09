using Medo.Device;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CanStick {
    internal partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
            this.Font = SystemFonts.MessageBoxFont;
            Medo.Windows.Forms.State.SetupOnLoadAndClose(this, lsvMessages);

            tmrRefresh_Tick(null, null);
        }


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
                    foreach (ListViewItem item in lsvMessages.Items) {
                        item.Selected = true;
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

        private CanStickDevice Document = null;


        private void bwDevice_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e) {
            var device = (CanStickDevice)e.Argument;

            try {
                var stopwatch = Stopwatch.StartNew();
                while (true) {
                    if (stopwatch.ElapsedMilliseconds > 500) {
                        if (bwDevice.CancellationPending) { break; }
                        bwDevice.ReportProgress(-1, device.GetFlags());

                        if (bwDevice.CancellationPending) { break; }
                        bwDevice.ReportProgress(-1, device.GetStatus());

                        stopwatch.Reset();
                        stopwatch.Start();
                    }

                    for (int i = 0; i < 64; i++) {
                        if (bwDevice.CancellationPending) { break; }
                        var message = device.GetMessage();
                        if (message == null) { break; }
                        Interlocked.Increment(ref this.MessageCount);
                        if (Interlocked.CompareExchange(ref this.ProcessingPaused, 0, 0) == 0) {
                            bwDevice.ReportProgress(-1, message);
                        }
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

        private void bwDevice_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e) {
            var flags = e.UserState as CanStickFlags;
            var status = e.UserState as CanStickStatus;
            var message = e.UserState as CanStickMessage;

            if (flags != null) {
                staPower.Text = flags.IsPowerEnabled ? Strings.PowerOn : Strings.PowerOff;
                staTermination.Text = flags.IsTerminationEnabled ? Strings.TerminationOn : Strings.TerminationOff;
            } else if (status != null) {
                if (status.TxOff) {
                    staTxStatus.Text = Strings.TxOff;
                } else if (status.TxPassive) {
                    staTxStatus.Text = Strings.TxPassive;
                } else if (status.TxWarning) {
                    staTxStatus.Text = Strings.TxWarning;
                } else {
                    staTxStatus.Text = Strings.TxOK;
                }
                if (status.TxErrorCount > 0) { staTxStatus.Text += " (" + status.TxErrorCount.ToString(CultureInfo.CurrentCulture) + ")"; }

                if (status.RxPassive) {
                    staRxStatus.Text = Strings.RxPassive;
                } else if (status.RxWarning) {
                    staRxStatus.Text = Strings.RxWarning;
                } else {
                    staRxStatus.Text = Strings.RxOK;
                }
                if (status.RxErrorCount > 0) { staRxStatus.Text += " (" + status.RxErrorCount.ToString(CultureInfo.CurrentCulture) + ")"; }

                if (status.RxOverflow) {
                    staRxOverflowStatus.Text = Strings.RxOverflow;
                } else if (status.RxOverflowWarning) {
                    staRxOverflowStatus.Text = Strings.RxOverflowWarning;
                } else {
                    staRxOverflowStatus.Text = "";
                }
            } else if (message != null) {
                var lvi = new ListViewItem(message.IsExtended ? message.ID.ToString("X8") : message.ID.ToString("X3"));
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

            ProcessMenuState();
        }

        private void bwDevice_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e) {
            this.Document = null;
            ProcessMenuState();
        }

        #endregion


        #region Menu

        private void mnuNew_Click(object sender, EventArgs e) {
            lsvMessages.Items.Clear();
        }

        private void mnuCopy_Click(object sender, EventArgs e) {
            var sb = new StringBuilder();
            for (int i = lsvMessages.SelectedItems.Count - 1; i >= 0; i++) {
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
                    this.Document = new CanStickDevice(port);
                    this.Document.Open();
                    bwDevice.RunWorkerAsync(this.Document);
                } catch (Exception ex) {
                    this.Document = null;
                    Medo.MessageBox.ShowError(this, "Cannot connect to device!\n\n" + ex.Message);
                }
            }
            ProcessMenuState();
            Interlocked.Exchange(ref this.ProcessingPaused, 0);
        }

        private void mnuPause_CheckStateChanged(object sender, EventArgs e) {
            Interlocked.Exchange(ref this.ProcessingPaused, mnuPause.Checked ? 1 : 0);
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


        private void mnuListClear_Click(object sender, EventArgs e) {
            lsvMessages.BeginUpdate();
            lsvMessages.Items.Clear();
            lsvMessages.EndUpdate();
            ProcessMenuState();
        }

        private void mnuListEnd_Click(object sender, EventArgs e) {
            lsvMessages.SelectedItems.Clear();
            lsvMessages.EnsureVisible(lsvMessages.Items.Count - 1);
        }


        private void mnuAppFeedback_Click(object sender, EventArgs e) {

        }

        private void mnuAppUpgrade_Click(object sender, EventArgs e) {

        }

        private void mnuAppDonate_Click(object sender, EventArgs e) {

        }

        private void mnuAppAbout_Click(object sender, EventArgs e) {
            Medo.Windows.Forms.AboutBox.ShowDialog(this, new Uri("http://jmedved.com/canstick/"));
        }


        private void staPower_MouseDown(object sender, MouseEventArgs e) {
            mnxPower.Show(sta, staPower.Bounds.X + e.Location.X, staPower.Bounds.Y + e.Location.Y);
        }

        private void mnxPower_Opening(object sender, CancelEventArgs e) {
            mnxPowerOn.Enabled = false;
            mnxPowerOff.Enabled = false;
            try {
                var flags = (this.Document != null) ? this.Document.GetFlags() : null;
                if (flags != null) {
                    mnxPowerOn.Enabled = !flags.IsPowerEnabled;
                    mnxPowerOff.Enabled = flags.IsPowerEnabled;
                }
            } catch (Exception) { }
        }

        private void mnxPowerOn_Click(object sender, EventArgs e) {
            if (this.Document != null) {
                try {
                    this.Document.SetPower(true);
                } catch (Exception) { }
                ProcessMenuState();
            }
        }

        private void mnxPowerOff_Click(object sender, EventArgs e) {
            if (this.Document != null) {
                try {
                    this.Document.SetPower(false);
                } catch (Exception) { }
                ProcessMenuState();
            }
        }


        private void staTermination_MouseDown(object sender, MouseEventArgs e) {
            mnxTermination.Show(sta, staTermination.Bounds.X + e.Location.X, staTermination.Bounds.Y + e.Location.Y);
        }

        private void mnxTermination_Opening(object sender, CancelEventArgs e) {
            mnxTerminationOn.Enabled = false;
            mnxTerminationOff.Enabled = false;
            try {
                var flags = (this.Document != null) ? this.Document.GetFlags() : null;
                if (flags != null) {
                    mnxTerminationOn.Enabled = !flags.IsTerminationEnabled;
                    mnxTerminationOff.Enabled = flags.IsTerminationEnabled;
                }
            } catch (Exception) { }
        }

        private void mnxTerminationOn_Click(object sender, EventArgs e) {
            if (this.Document != null) {
                try {
                    this.Document.SetTermination(true);
                } catch (Exception) { }
                ProcessMenuState();
            }
        }

        private void mnxTerminationOff_Click(object sender, EventArgs e) {
            if (this.Document != null) {
                try {
                    this.Document.SetTermination(false);
                } catch (Exception) { }
                ProcessMenuState();
            }
        }

        #endregion


        private string LastPorts = "-";
        private int MessageCount = 0;
        private int ProcessingPaused = 0;
        private DateTime ResetTime = DateTime.UtcNow;

        private void tmrRefresh_Tick(object sender, EventArgs e) {
            int messageCount = Interlocked.Exchange(ref this.MessageCount, 0);
            var timePassed = (DateTime.UtcNow - this.ResetTime).TotalMilliseconds;
            this.ResetTime = DateTime.UtcNow;

            var mps = (messageCount / timePassed) * 1000;
            if (mps > 0) {
                staMessagesPerSecond.Text = mps.ToString("#,##0.0 'm/s'", CultureInfo.CurrentCulture);
            } else {
                staMessagesPerSecond.Text = "";
            }


            if (mnuPorts.DroppedDown) { return; } //don't update while user is selecting port
            if (mnuPorts.Enabled == false) { return; } //don't update when already connected

            var ports = System.IO.Ports.SerialPort.GetPortNames();
            var nextPorts = String.Join(" ", ports);
            if (nextPorts != LastPorts) {
                object lastSelected = mnuPorts.SelectedItem;
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
            var isConnected = (this.Document != null);
            mnuNew.Enabled = (lsvMessages.Items.Count > 0);
            mnuCopy.Enabled = (lsvMessages.SelectedItems.Count > 0);
            mnuPorts.Enabled = (this.Document == null);
            mnuConnect.Enabled = (mnuPorts.SelectedItem != null) && (this.Document == null) && (!bwDevice.IsBusy);
            mnuPause.Enabled = (this.Document != null);
            if (!mnuPause.Enabled) { mnuPause.Checked = false; }
            mnuDisconnect.Enabled = (this.Document != null);
            mnuListClear.Enabled = (this.Document != null) || (lsvMessages.Items.Count > 0);
            mnuListEnd.Enabled = (this.Document != null);
            mnuSend.Enabled = (this.Document != null);

            var status = isConnected ? Strings.Connected : Strings.NotConnected;
            if (staStatus.Text != status) {
                staStatus.Text = status;
                if (!isConnected) {
                    staPower.Text = "";
                    staTermination.Text = "";
                    staTxStatus.Text = "";
                    staRxStatus.Text = "";
                    staRxOverflowStatus.Text = "";
                }
            }

        }

    }
}
