using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Medo.Device;

namespace CanankaTest {
    internal partial class SendMessageForm : Form {
        public SendMessageForm(Cananka device) {
            InitializeComponent();
            this.Font = SystemFonts.MessageBoxFont;

            this.Device = device;

            foreach (var control in new Control[] { txtID, txtLength, chbRemoteRequest, txtData }) {
                ErrorProvider.SetIconAlignment(control, ErrorIconAlignment.MiddleRight);
                ErrorProvider.SetIconPadding(control, -ErrorProvider.Icon.Width - SystemInformation.Border3DSize.Width);
            }
        }

        private readonly Cananka Device;

        protected override bool ProcessDialogKey(Keys keyData) {
            switch (keyData) {
                case Keys.Escape:
                    this.Close();
                    return true;
            }

            return base.ProcessDialogKey(keyData);
        }


        private void Form_Load(object sender, EventArgs e) {
            txtID.Text = (Settings.Current.LastID <= 0x3FF) ? Settings.Current.LastID.ToString("X3") : Settings.Current.LastID.ToString("X8");
            txtLength.Text = Settings.Current.LastLength.ToString();
            chbRemoteRequest.Checked = Settings.Current.LastRemoteRequest;
            txtData.Text = Settings.Current.LastData;

            txt_TextChanged(null, null);
        }


        private void txt_TextChanged(object sender, EventArgs e) {
            btnSend.Enabled = ParseAll() != null;
        }


        private CanankaMessage ParseAll() {
            txtLength.Enabled = chbRemoteRequest.Checked;
            txtData.Enabled = !chbRemoteRequest.Checked;

            var id = int.MinValue;
            if (int.TryParse(txtID.Text, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out id) && (id >= 0) && (id <= 0x1FFFFFFF)) {
                ErrorProvider.SetError(txtID, null);
            } else {
                ErrorProvider.SetError(txtID, "ID must be a hexadecimal number in 0x00000000 to 0x1FFFFFFF range.");
            }

            var remoteRequest = chbRemoteRequest.Checked;
            if (remoteRequest) {
                var length = int.MinValue;
                if (int.TryParse(txtLength.Text, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out length) && (length >= 0) && (length <= 8)) {
                    ErrorProvider.SetError(txtLength, null);
                } else {
                    ErrorProvider.SetError(txtLength, "Length must be a number in 0 to 8 range.");
                }
                if ((id > int.MinValue) && (length > int.MinValue)) {
                    return new CanankaMessage(id, length);
                }
            } else {
                var dataText = txtData.Text.Trim().Replace(" ", "");
                if (dataText.Length % 2 == 1) { dataText = "0" + dataText; }

                ErrorProvider.SetError(txtLength, null);
                var data = new List<byte>();
                for (var i = 0; i < dataText.Length; i += 2) {
                    if (byte.TryParse(dataText.Substring(i, 2), NumberStyles.HexNumber, CultureInfo.CurrentCulture, out var oneByte)) {
                        data.Add(oneByte);
                    } else {
                        ErrorProvider.SetError(txtLength, "Cannot parse hexadecimal data.");
                        return null;
                    }
                }
                if (data.Count <= 8) {
                    if (id > int.MinValue) { return new CanankaMessage(id, data.ToArray()); }
                } else {
                    ErrorProvider.SetError(txtLength, "Data can be only between 0 and 8 bytes long.");
                }
            }

            return null;
        }

        private void btnSend_Click(object sender, EventArgs e) {
            var message = ParseAll();
            if (message != null) {
                this.Device.SendMessage(message);
            }
        }
    }
}
