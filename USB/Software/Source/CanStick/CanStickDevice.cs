using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Medo.Device {

    public class CanStickDevice {

        private readonly object SyncRoot = new object();

        public CanStickDevice(string portName) {
            this.Port = new Medo.IO.UartPort(portName, 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
            this.Port.ReadTimeout = 100;
            this.Port.WriteTimeout = 100;
            this.Port.NewLine = "\n";
        }

        private Medo.IO.UartPort Port = null;


        public void Open() {
            this.Port.Open();
        }

        public bool IsOpen {
            get { return this.Port.IsOpen; }
        }

        public void Close() {
            this.Port.Close();
        }


        public CanStickFlags GetFlags() {
            return new CanStickFlags(SendCommand(':'));
        }

        public CanStickStatus GetStatus() {
            return new CanStickStatus(SendCommand('?'));
        }

        public CanStickMessage GetMessage() {
            var line = SendCommand();
            if (!string.IsNullOrEmpty(line)) {
                var message = new CanStickMessage(line);
                if (message.ID >= 0) { return message; }
            }
            return null;
        }


        public bool SetPower(bool status) {
            var state = SendCommand(':', status ? "P" : "p");
            return !state.StartsWith("!", StringComparison.InvariantCulture);
        }

        public bool SetTermination(bool status) {
            var state = SendCommand(':', status ? "T" : "t");
            return !state.StartsWith("!", StringComparison.InvariantCulture);
        }


        private string SendCommand(char command = '\0', params string[] data) {
            try {
                var sb = new StringBuilder();
                if (command != '\0') { sb.Append(command); }
                if (data != null) {
                    foreach (var datum in data) {
                        sb.Append(datum);
                    }
                }
                lock (this.SyncRoot) {
                    var cmd = sb.ToString();
                    Debug.WriteLine("UART W: " + cmd);
                    this.Port.WriteLine(cmd);

                    var result = this.Port.ReadLine();
                    Debug.WriteLine("UART R: " + result);
                    return result;
                }
            } catch (TimeoutException) {
                Debug.WriteLine("UART PURGE");
                this.Port.Purge();
                throw;
            }
        }

    }


    public class CanStickFlags {

        internal CanStickFlags(string line) {
            this.IsPowerEnabled = (line.IndexOf("P", StringComparison.Ordinal) >= 0);
            this.IsTerminationEnabled = (line.IndexOf("T", StringComparison.Ordinal) >= 0);
            this.IsEchoEnabled = (line.IndexOf("O", StringComparison.Ordinal) >= 0);
            this.IsAutoReportingEnabled = (line.IndexOf("Q", StringComparison.Ordinal) >= 0);
        }


        public bool IsPowerEnabled { get; private set; }
        public bool IsTerminationEnabled { get; private set; }
        public bool IsEchoEnabled { get; private set; }
        public bool IsAutoReportingEnabled { get; private set; }


        public override string ToString() {
            var sb = new StringBuilder();
            if (this.IsPowerEnabled) { sb.Append("P"); }
            if (this.IsTerminationEnabled) { sb.Append("T"); }
            if (this.IsEchoEnabled) { sb.Append("O"); }
            if (this.IsAutoReportingEnabled) { sb.Append("Q"); }
            if (sb.Length == 0) { sb.Append("-"); }
            return sb.ToString();
        }

    }


    public class CanStickStatus {

        internal CanStickStatus(string line) {
            var parts = line.Split(new char[] { ':' }, 2);
            var part1 = parts[0];
            var part2 = (parts.Length > 1) ? parts[1] : null;

            this.TxOff = (part1.IndexOf("X", StringComparison.Ordinal) >= 0);
            this.TxPassive = (part1.IndexOf("T", StringComparison.Ordinal) >= 0);
            this.TxWarning = (part1.IndexOf("t", StringComparison.Ordinal) >= 0);
            this.RxPassive = (part1.IndexOf("R", StringComparison.Ordinal) >= 0);
            this.RxWarning = (part1.IndexOf("r", StringComparison.Ordinal) >= 0);
            this.RxOverflow = (part1.IndexOf("O", StringComparison.Ordinal) >= 0);
            this.RxOverflowWarning = (part1.IndexOf("o", StringComparison.Ordinal) > 0);

            if (part2 != null) {
                var hexTx = part2.Length >= 2 ? part2.Substring(0, 2) : null;
                var hexRx = part2.Length >= 4 ? part2.Substring(2, 2) : null;
                int txErrorCount, rxErrorCount;
                if (int.TryParse(hexTx, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out txErrorCount)) {
                    this.TxErrorCount = txErrorCount;
                }
                if (int.TryParse(hexRx, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out rxErrorCount)) {
                    this.RxErrorCount = rxErrorCount;
                }
            }
        }


        public bool TxOff { get; private set; }
        public bool TxPassive { get; private set; }
        public bool TxWarning { get; private set; }
        public bool RxPassive { get; private set; }
        public bool RxWarning { get; private set; }
        public bool RxOverflow { get; private set; }
        public bool RxOverflowWarning { get; private set; }

        public int TxErrorCount { get; private set; }
        public int RxErrorCount { get; private set; }


        public override string ToString() {
            var sb = new StringBuilder();
            if (this.TxOff) {
                sb.Append("X");
            } else if (this.TxPassive) {
                sb.Append("T");
            } else if (this.TxWarning) {
                sb.Append("t");
            }
            if (this.RxPassive) {
                sb.Append("R");
            } else if (this.RxWarning) {
                sb.Append("r");
            }
            if (this.RxOverflow) {
                sb.Append("O");
            } else if (this.RxOverflowWarning) {
                sb.Append("o");
            }
            if (sb.Length == 0) { sb.Append("OK"); }
            return sb.ToString();
        }

    }


    public class CanStickMessage {

        internal CanStickMessage(string line) {
            var parts = line.Split(new char[] { ':' }, 2);
            var partID = parts[0];
            var partData = (parts.Length > 1) ? parts[1] : null;

            this.ID = int.Parse(partID, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            this.IsExtended = (partID.Length > 3);

            if (partData != null) {
                var len = partData.Length / 2;
                this.Data = new byte[len];
                for (int i = 0; i < partData.Length; i += 2) {
                    this.Data[i / 2] = byte.Parse(partData.Substring(i, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                }
            } else {
                this.IsRemoteRequest = true;
            }
        }


        public int ID { get; private set; }
        public bool IsExtended { get; private set; }
        public bool IsRemoteRequest { get; private set; }

        private byte[] Data;
        public byte[] GetData() {
            if (this.Data != null) {
                var buffer = new byte[this.Data.Length];
                Buffer.BlockCopy(this.Data, 0, buffer, 0, this.Data.Length);
                return buffer;
            }
            return null;
        }


        public override string ToString() {
            var sb = new StringBuilder();
            if (this.IsExtended || (this.ID > 0x7FF)) {
                sb.AppendFormat(CultureInfo.InvariantCulture, "X8", this.Data);
            } else {
                sb.AppendFormat(CultureInfo.InvariantCulture, "X3", this.Data);
            }
            if (!this.IsRemoteRequest) {
                sb.Append(":");
                foreach (var b in this.Data) {
                    sb.Append(b.ToString("X2", CultureInfo.InvariantCulture));
                }
            }
            return sb.ToString();
        }
    }

}
