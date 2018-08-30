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

        private Cananka Document = null;


        private void bwDevice_DoWork(object sender, DoWorkEventArgs e) {
            var device = (Cananka)e.Argument;

            try {
                var stopwatch = Stopwatch.StartNew();
                while (true) {
                    var skipIndex = 0; //to interleave flag checks
                    if (stopwatch.ElapsedMilliseconds > 500) {
                        if (skipIndex == 0) {
                            if (bwDevice.CancellationPending) { break; }
                            bwDevice.ReportProgress(-1, device.GetStatus());
                        } else if (skipIndex == 1) {
                            if (bwDevice.CancellationPending) { break; }
                            bwDevice.ReportProgress(-1, device.GetExtendedStatus());
                        }
                        skipIndex = (skipIndex++ % 2);

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

            if (extendedStatus != null) {
                staPower.Visible = extendedStatus.PowerEnabled;
                staTermination.Visible = extendedStatus.TerminationEnabled;
            } else if (status != null) {
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


        private void mnuAppFeedback_Click(object sender, EventArgs e) {

        }

        private void mnuAppUpgrade_Click(object sender, EventArgs e) {

        }

        private void mnuAppDonate_Click(object sender, EventArgs e) {

        }

        private void mnuAppAbout_Click(object sender, EventArgs e) {
            Medo.Windows.Forms.AboutBox.ShowDialog(this, new Uri("http://medo64.com/cananka/usb/"));
        }


        private void mnxFeatures_Opening(object sender, CancelEventArgs e) {
            mnxPowerOn.Enabled = false;
            mnxPowerOff.Enabled = false;
            mnxTerminationOn.Enabled = false;
            mnxTerminationOff.Enabled = false;

            var flags = this.Document?.GetExtendedStatus();
            if ((flags != null) && flags.IsValid) {
                mnxPowerOn.Enabled = !flags.PowerEnabled;
                mnxPowerOff.Enabled = flags.PowerEnabled;
                mnxTerminationOn.Enabled = !flags.TerminationEnabled;
                mnxTerminationOff.Enabled = flags.TerminationEnabled;
            }
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
            Array.Sort(ports);
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

                var status = isConnected ? Strings.Connected : Strings.NotConnected;
                if (staStatus.Text != status) {
                    staStatus.Text = status;
                    if (!isConnected) {
                        staPower.Visible = false;
                        staTermination.Visible = false;
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
