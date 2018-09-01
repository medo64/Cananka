//Josip Medved <jmedved@jmedved.com>  https://www.medo64.com

//2018-08-30: New version.


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace Medo.Device {

    /// <summary>
    /// Communication with Cananka, SLCAN-compatible device.
    /// </summary>
    public class Cananka : IDisposable {

        /// <summary>
        /// Initializes a new instance of the class using the specified port name.
        /// </summary>
        /// <param name="portName">The port to use.</param>
        public Cananka(string portName)
            : this(portName, 115200, Parity.None, 8, StopBits.One) {
        }

        /// <summary>
        /// Initializes a new instance of the class using the specified port name and baud rate.
        /// </summary>
        /// <param name="portName">The port to use.</param>
        /// <param name="baudRate">The baud rate.</param>
        public Cananka(string portName, int baudRate)
            : this(portName, baudRate, Parity.None, 8, StopBits.One) {
        }

        /// <summary>
        /// Initializes a new instance of the class using the specified port name, baud rate, parity bit, data bits, and stop bit.
        /// </summary>
        /// <param name="portName">The port to use.</param>
        /// <param name="baudRate">The baud rate.</param>
        /// <param name="parity">One of the Parity values.</param>
        /// <param name="dataBits">The data bits value.</param>
        /// <param name="stopBits">The stop bits value.</param>
        /// <exception cref="ArgumentNullException">PortName cannot be null.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Object is disposed in Dispose method.")]
        public Cananka(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits) {
            if (portName == null) { throw new ArgumentNullException("portName", "PortName cannot be null."); }
            portName = portName.Trim().TrimEnd(':');

            this.Uart = new SerialPort(portName, baudRate, parity, dataBits, stopBits) {
                ReadTimeout = 100,
                WriteTimeout = 100,
                NewLine = "\0"
            };

            this.Context = SynchronizationContext.Current;
            this.Thread = new Thread(this.Run) {
                CurrentCulture = CultureInfo.InvariantCulture,
                IsBackground = true,
                Name = "Cananka:" + portName,
                Priority = ThreadPriority.AboveNormal
            };
            this.CancelEvent = new ManualResetEvent(false);
        }


        private readonly SerialPort Uart;
        private readonly object UartLock = new object();

        private readonly SynchronizationContext Context;
        private readonly ManualResetEvent CancelEvent;
        private readonly Thread Thread;


        /// <summary>
        /// Opens SLCAN device and returns true if operation was successful.
        /// If SLCAN didn't return positive response, port is immediately closed.
        /// </summary>
        public bool Open() {
            this.Uart.Open();

            WriteCommand(new byte[] { (byte)'C', CR }); //first close
            WriteCommand(new byte[] { (byte)'*', (byte)'r', CR }); //then clear debug (ignored on non-Cananka devices)

            var response = WriteCommand(new byte[] { (byte)'O', CR });
            if ((response != null) && response.IsPositive) {
                this.Thread.Start();
                return true;
            }

            this.Uart.Close();
            return false;
        }

        /// <summary>
        /// Gets if serial port is open.
        /// </summary>
        public bool IsOpen {
            get { return this.Uart.IsOpen; }
        }

        /// <summary>
        /// Closes the serial port.
        /// </summary>
        public void Close() {
            if (this.IsOpen) {
                WriteCommand(new byte[] { (byte)'C', CR });

                //stop thread
                this.CancelEvent.Set();
                var sw = Stopwatch.StartNew();
                while (this.Thread.IsAlive && (sw.ElapsedMilliseconds < 500)) { Thread.Sleep(10); }
                if (Thread.IsAlive) {
                    try {
                        this.Thread.Abort();
                        while (this.Thread.IsAlive) { Thread.Sleep(10); }
                    } catch (ThreadStateException) { }
                }
                this.CancelEvent.Reset();

                this.Uart.Close();
            }
        }


        /// <summary>
        /// Returns device flags.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Method is more appropriate than property as it does more than simple field access.")]
        public CanankaStatus GetStatus() {
            var response = WriteCommand(new byte[] { (byte)'F', CR });
            return new CanankaStatus(response?.ToBytes());
        }


        /// <summary>
        /// Returns if CAN message is successfully sent.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <returns></returns>
        public bool SendMessage(CanankaMessage message) {
            if (message == null) { throw new ArgumentNullException(nameof(message), "Message cannot be null."); }

            var bytes = new List<byte>();
            if (message.IsRemoteRequest) {
                bytes.Add(message.IsExtended ? (byte)'R' : (byte)'r');
            } else {
                bytes.Add(message.IsExtended ? (byte)'T' : (byte)'t');
            }
            bytes.AddRange(Encoding.ASCII.GetBytes(message.Id.ToString(message.IsExtended ? "X8" : "X3", CultureInfo.InvariantCulture)));
            bytes.Add((byte)(0x30 + message.Length));
            if (!message.IsRemoteRequest) {
                foreach (var b in message.GetData()) {
                    bytes.AddRange(Encoding.ASCII.GetBytes(b.ToString("X2", CultureInfo.InvariantCulture)));
                }
            }
            bytes.Add(CR);

            var response = WriteCommand(bytes.ToArray());
            return response?.IsPositive ?? false;
        }



        /// <summary>
        /// Event triggered on every message arrival.
        /// If thread used to create object had synchronization context (e.g. main window loop), that context will be used for event processing.
        /// </summary>
        public event EventHandler<CanankaMessageEventArgs> MessageArrived;


        /// <summary>
        /// Raises MessageArrived trigger.
        /// If thread used to create object had synchronization context (e.g. main window loop), that context will be used for event processing.
        /// </summary>
        /// <param name="e">Arguments.</param>
        protected void OnMessageArrived(CanankaMessageEventArgs e) {
            if (this.Context != null) {
                this.Context.Post((object state) => {
                    this.MessageArrived?.Invoke(this, state as CanankaMessageEventArgs);
                }, e);
            } else {
                this.MessageArrived?.Invoke(this, e);
            }
        }


        #region Extended

        /// <summary>
        /// Returns extended device flags (only on Cananka USB).
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Method is more appropriate than property as it does more than simple field access.")]
        public CanankaExtendedStatus GetExtendedStatus() {
            var response = WriteCommand(new byte[] { (byte)'*', (byte)'F', CR });
            return new CanankaExtendedStatus(response?.ToBytes());
        }

        /// <summary>
        /// Sets state of 5V power output. Works only on Cananka USB/mini.
        /// </summary>
        /// <param name="state">Whether to turn on or off 5V power output.</param>
        public bool SetPower(bool state) {
            var response = WriteCommand(new byte[] { (byte)'*', (byte)'P', state ? (byte)'1' : (byte)'0', CR });
            return (response.IsPositive);
        }

        /// <summary>
        /// Sets state of 120 ohm termination. Works only on Cananka USB/mini.
        /// </summary>
        /// <param name="state">Whether to turn on or off termination resistors.</param>
        public bool SetTermination(bool state) {
            var response = WriteCommand(new byte[] { (byte)'*', (byte)'T', state ? (byte)'1' : (byte)'0', CR });
            return (response.IsPositive);
        }

        #endregion Extended



        #region IDisposable

        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Dispose resources.
        /// </summary>
        /// <param name="disposing">True if managed resources are to be disposed.</param>
        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    this.Close();
                    this.Uart.Dispose();
                    this.CancelEvent.Dispose();
                }
            }
        }

        /// <summary>
        /// Dispose used resources.
        /// </summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion


        #region Processing

        private ParsedResponseData WriteCommand(byte[] data) {
            lock (this.UartLock) {
#if DEBUG
                Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "[Cananka] -> {0}", ConvertBytesToText(data, data.Length)));
#endif
                this.Uart.Write(data, 0, data.Length);

                var buffer = new byte[BufferSize];
                var sw = Stopwatch.StartNew();
                while (sw.ElapsedMilliseconds < 500) { //wait only 500ms for response to a command
                    try {
                        if (this.Uart.BytesToRead > 0) {
                            var readLength = this.Uart.Read(buffer, 0, buffer.Length);
                            if (readLength > 0) {
                                var response = ProcessRawBytesAndReturnResponse(buffer, readLength);
                                return response;
                            }
                        }
                    } catch (TimeoutException) { }
                }
            }
            return null;
        }


        private const byte BEL = 0x07;
        private const byte CR = 0x0D;
        private const int BufferSize = 1024;
        private readonly string[] LowAscii = new string[] { "NUL", "SOH", "STX", "ETX", "EOT", "ENQ", "ACK", "BEL",
                                                            "BS", "HT", "LF", "VT", "FF", "CR", "SO", "SI",
                                                            "DLE", "DC1", "DC2", "DC3", "DC4", "NAK", "SYN", "ETB",
                                                            "CAN", "EM", "SUB", "ESC", "FS", "GS", "RS", "US" };

        private Queue<byte> ByteQueue = new Queue<byte>();
        private Queue<CanankaMessage> MessageQueue = new Queue<CanankaMessage>();


        private ParsedResponseData ProcessRawBytesAndReturnResponse(byte[] buffer, int length) { //returns non-message response AFTER processing all the bytes
#if DEBUG
            Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "[Cananka] <- {0}", ConvertBytesToText(buffer, length)));
#endif

            var nonMessageResponse = default(ParsedResponseData);
            for (var i = 0; i < length; i++) {
                var curr = buffer[i];
                this.ByteQueue.Enqueue(curr);
                if ((curr == CR) || (curr == BEL)) {
                    var response = new ParsedResponseData(this.ByteQueue.ToArray());
                    this.ByteQueue.Clear();

                    if (response.IsMessage) {
                        var message = response.ToMessage();
                        if (message != null) {
                            this.MessageQueue.Enqueue(message);
#if DEBUG
                            Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "[Cananka] <= {0}", message.ToString()));
#endif
                        }
                    } else {
                        nonMessageResponse = response;
                    }
                }
            }

            return nonMessageResponse;
        }

        private string ConvertBytesToText(byte[] buffer, int length) {
            var sb = new StringBuilder();
            for (var i = 0; i < length; i++) {
                if (buffer[i] < 32) {
                    sb.Append("<" + this.LowAscii[buffer[i]] + ">");
                } else {
                    sb.Append(Encoding.ASCII.GetString(buffer, i, 1));
                }
            }

            return sb.ToString();
        }


        [DebuggerDisplay("{IsPositive ? \"OK\" : \"NOK\"}")]
        private class ParsedResponseData {
            public ParsedResponseData(byte[] data) {
                this.RawData = data;
            }

            private byte[] RawData;


            public bool IsPositive {
                get { return (this.RawData.Length > 0) && (this.RawData[this.RawData.Length - 1] == CR); }
            }

            public bool IsMessage {
                get {
                    if (this.RawData.Length > 0) {
                        switch (this.RawData[0]) {
                            case (byte)'t':
                            case (byte)'T':
                            case (byte)'r':
                            case (byte)'R':
                                return true;
                        }
                    }
                    return false;
                }
            }

            public CanankaMessage ToMessage() {
                var buffer = this.RawData;
                if (buffer.Length > 0) {
                    switch (buffer[0]) {
                        case (byte)'t':
                            return ToStandardMessage(buffer, isRemoteRequest: false);

                        case (byte)'r':
                            return ToStandardMessage(buffer, isRemoteRequest: true);

                        case (byte)'T':
                            return ToExtendedMessage(buffer, isRemoteRequest: false);

                        case (byte)'R':
                            return ToExtendedMessage(buffer, isRemoteRequest: true);
                    }
                }
                return null;
            }

            public byte[] ToBytes() {
                if (this.RawData.Length > 0) {
                    var data = new byte[this.RawData.Length - 1];
                    Buffer.BlockCopy(this.RawData, 0, data, 0, data.Length);
                    return data;
                } else {
                    return new byte[] { };
                }
            }

            public override string ToString() {
                return Encoding.ASCII.GetString(this.ToBytes());
            }



            private static CanankaMessage ToStandardMessage(byte[] buffer, bool isRemoteRequest) {
                if (buffer.Length > 4) {
                    var length = buffer[4] - 0x30;
                    if ((length <= 8) && int.TryParse(Encoding.ASCII.GetString(buffer, 1, 3), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var id)) {
                        var expectedLength = 5 + (isRemoteRequest ? 0 : length * 2) + 1;
                        if ((id >= 0) && (id < 0x7FF) && (buffer.Length == expectedLength)) {
                            var dataBytes = isRemoteRequest ? null : new byte[length];
                            if (!isRemoteRequest) {
                                for (var i = 0; i < length; i++) {
                                    if (byte.TryParse(Encoding.ASCII.GetString(buffer, 5 + i * 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var singleByte)) {
                                        dataBytes[i] = singleByte;
                                    }
                                }
                            }
                            return new CanankaMessage(id, length, isExtended: false, isRemoteRequest: isRemoteRequest, data: dataBytes);
                        }
                    }
                }
                return null;
            }

            private static CanankaMessage ToExtendedMessage(byte[] buffer, bool isRemoteRequest) {
                if (buffer.Length > 9) {
                    var length = buffer[9] - 0x30;
                    if ((length <= 8) && int.TryParse(Encoding.ASCII.GetString(buffer, 1, 8), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var id)) {
                        var expectedLength = 10 + (isRemoteRequest ? 0 : length * 2) + 1;
                        if ((id >= 0) && (id < 0x1FFFFFFF) && (buffer.Length == expectedLength)) {
                            var dataBytes = isRemoteRequest ? null : new byte[length];
                            if (!isRemoteRequest) {
                                for (var i = 0; i < length; i++) {
                                    if (byte.TryParse(Encoding.ASCII.GetString(buffer, 10 + i * 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var singleByte)) {
                                        dataBytes[i] = singleByte;
                                    }
                                }
                            }
                            return new CanankaMessage(id, length, isExtended: false, isRemoteRequest: isRemoteRequest, data: dataBytes);
                        }
                    }
                }
                return null;
            }
        }


        private void Run() {
            var buffer = new byte[BufferSize];

            try {
                while (!CancelEvent.WaitOne(0, false)) {
                    lock (this.UartLock) {
                        try {
                            while (this.Uart.BytesToRead > 0) {
                                var readLength = this.Uart.Read(buffer, 0, buffer.Length);
                                if (readLength > 0) {
                                    ProcessRawBytesAndReturnResponse(buffer, readLength);
                                }
                            }
                        } catch (TimeoutException) { }

                        while (this.MessageQueue.Count > 0) {
                            var e = new CanankaMessageEventArgs(this.MessageQueue.Dequeue());
                            this.OnMessageArrived(e);
                        }

                        Thread.Sleep(1);
                    }
                }
            } catch (ThreadAbortException) { }
        }

        #endregion Processing

    }


    /// <summary>
    /// Device CAN bus state.
    /// </summary>
    public enum CanankaBusState {
        /// <summary>
        /// CAN bus is closed.
        /// </summary>
        Closed = 0,
        /// <summary>
        /// CAN bus is in loopback mode.
        /// </summary>
        Loopback = 1,
        /// <summary>
        /// CAN bus is in listen-only mode.
        /// </summary>
        Listen = 2,
        /// <summary>
        /// CAN bus is open.
        /// </summary>
        Open = 3,
    }


    /// <summary>
    /// Basic SLCAN flags
    /// </summary>
    public class CanankaStatus {

        internal CanankaStatus(byte[] data) {
            if ((data != null) && (data.Length == 3) && (data[0] == (byte)'F')) {
                if (byte.TryParse(Encoding.ASCII.GetString(data, 1, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result)) {
                    this.RxQueueFull = ((result & 0b00000001) != 0);
                    this.TxQueueFull = ((result & 0b00000010) != 0);
                    this.TxRxWarning = ((result & 0b00000100) != 0);
                    this.RxOverflow = ((result & 0b00001000) != 0);
                    this.ErrorPassive = ((result & 0b00100000) != 0);
                    this.ArbitrationLost = ((result & 0b01000000) != 0);
                    this.BusError = ((result & 0b10000000) != 0);
                    this.IsValid = true;
                }
            }
        }


        /// <summary>
        /// Gets if content is valid.
        /// </summary>
        public bool IsValid { get; }


        /// <summary>
        /// Gets is receive queue is full.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Rx", Justification = "Casing is correct and intended.")]
        public bool RxQueueFull { get; }

        /// <summary>
        /// Gets if transmit queue is full.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tx", Justification = "Spelling is correct and intended.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Tx", Justification = "Cased is correct and intended.")]
        public bool TxQueueFull { get; }

        /// <summary>
        /// Gets if transmit/receive warning is present.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tx", Justification = "Spelling is correct and intended.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Rx", Justification = "Casing is correct and intended.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Tx", Justification = "Casing is correct and intended.")]
        public bool TxRxWarning { get; }

        /// <summary>
        /// Gets if receive has overflown.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Rx", Justification = "Casing is correct and intended.")]
        public bool RxOverflow { get; }

        /// <summary>
        /// Gets if bus is in error-passive state.
        /// </summary>
        public bool ErrorPassive { get; }

        /// <summary>
        /// Gets if bus arbitration is lost.
        /// </summary>
        public bool ArbitrationLost { get; }

        /// <summary>
        /// Gets if bus is in error state.
        /// </summary>
        public bool BusError { get; }


        /// <summary>
        /// Returns textual description of flags.
        /// </summary>
        public override string ToString() {
            var sb = new StringBuilder();
            if (this.RxQueueFull) { sb.Append("R"); }
            if (this.TxQueueFull) { sb.Append("T"); }
            if (this.TxRxWarning) { sb.Append("W"); }
            if (this.RxOverflow) { sb.Append("O"); }
            if (this.ErrorPassive) { sb.Append("P"); }
            if (this.ArbitrationLost) { sb.Append("A"); }
            if (this.BusError) { sb.Append("E"); }
            if (sb.Length == 0) { sb.Append("OK"); }
            return sb.ToString();
        }

    }


    /// <summary>
    /// Extended flags. Supported only on Cananka USB.
    /// </summary>
    public class CanankaExtendedStatus {

        internal CanankaExtendedStatus(byte[] data) {
            if ((data != null) && (data.Length == 4) && (data[0] == (byte)'*') && (data[1] == (byte)'F')) {
                if (byte.TryParse(Encoding.ASCII.GetString(data, 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result)) {
                    this.BusState = (CanankaBusState)(result & 0b00000011);
                    this.AutoPooling = ((result & 0b00000100) != 0);
                    this.AnyErrors = ((result & 0b00001000) != 0);
                    this.PowerEnabled = ((result & 0b00010000) != 0);
                    this.TerminationEnabled = ((result & 0b00100000) != 0);
                    this.AnyDebug = ((result & 0b10000000) != 0);
                    this.IsValid = true;
                }
            }
        }


        /// <summary>
        /// Gets if content is valid.
        /// </summary>
        public bool IsValid { get; }


        /// <summary>
        /// Bus state.
        /// </summary>
        public CanankaBusState BusState { get; }

        /// <summary>
        /// Gets if auto-polling is turned on.
        /// </summary>
        public bool AutoPooling { get; }

        /// <summary>
        /// Gets if there are any errors active.
        /// </summary>
        public bool AnyErrors { get; }

        /// <summary>
        /// Gets if power output is turned on.
        /// </summary>
        public bool PowerEnabled { get; }

        /// <summary>
        /// Gets if 120 ohm termination is turned on.
        /// </summary>
        public bool TerminationEnabled { get; }

        /// <summary>
        /// Gets if any debug features are turned on.
        /// </summary>
        public bool AnyDebug { get; }


        /// <summary>
        /// Returns textual description of flags.
        /// </summary>
        public override string ToString() {
            var sb = new StringBuilder();
            switch (this.BusState) {
                case CanankaBusState.Closed:
                    sb.Append("C");
                    break;
                case CanankaBusState.Loopback:
                    sb.Append("l");
                    break;
                case CanankaBusState.Listen:
                    sb.Append("L");
                    break;
                case CanankaBusState.Open:
                    sb.Append("O");
                    break;
            }
            if (this.AutoPooling) { sb.Append("X"); }
            if (this.AnyErrors) { sb.Append("E"); }
            if (this.PowerEnabled) { sb.Append("P"); }
            if (this.TerminationEnabled) { sb.Append("T"); }
            if (this.AnyDebug) { sb.Append("D"); }
            if ((sb.Length == 0) || ((sb.Length == 1) && (sb[0] == 'O'))) { sb.Append("OK"); }
            return sb.ToString();
        }

    }


    /// <summary>
    /// CAN bus message.
    /// </summary>
    public class CanankaMessage {

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="id">11-bit or 29-bit identifier.</param>
        /// <param name="length">Length of request.</param>
        /// <param name="isExtended">True if address is longer than 11 bits.</param>
        /// <param name="isRemoteRequest">True if message is remote request.</param>
        /// <param name="data">Data bytes.</param>
        /// <exception cref="ArgumentOutOfRangeException">ID must be between 0 and 0x1FFFFFFF. -or- Length must be between 0 and 8 bytes. -or- Remote request cannot contain data. -or- Data cannot be longer than 8 bytes.</exception>
        internal CanankaMessage(int id, int length, bool isExtended, bool isRemoteRequest, byte[] data) {
            if ((id < 0x00000000) || (id > 0x1FFFFFFF)) { throw new ArgumentOutOfRangeException(nameof(id), "ID must be between 0 and 0x1FFFFFFF."); }
            if (id > 0x7FF) { isExtended = true; }

            if (isRemoteRequest) {
                if ((length < 0) || (length > 8)) { throw new ArgumentOutOfRangeException(nameof(length), "Length must be between 0 and 8 bytes."); }
                if (data != null) { throw new ArgumentOutOfRangeException(nameof(data), "Remote request cannot contain data."); }
            } else {
                if (data == null) { data = new byte[0]; }
                if (data.Length > 8) { throw new ArgumentOutOfRangeException(nameof(data), "Data cannot be longer than 8 bytes."); }
            }

            this.Id = id;
            this.Length = length;
            this.IsExtended = isExtended;
            this.IsRemoteRequest = isRemoteRequest;
            this.Data = data;
        }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="id">11-bit or 29-bit identifier.</param>
        /// <param name="data">Data.</param>
        /// <exception cref="ArgumentOutOfRangeException">ID must be between 0 and 0x1FFFFFFF. -or- Data cannot be longer than 8 bytes.</exception>
        public CanankaMessage(int id, byte[] data) {
            if ((id < 0x00000000) || (id > 0x1FFFFFFF)) { throw new ArgumentOutOfRangeException(nameof(id), "ID must be between 0 and 0x1FFFFFFF."); }
            if (data == null) { data = new byte[0]; }
            if (data.Length > 8) { throw new ArgumentOutOfRangeException(nameof(data), "Data cannot be longer than 8 bytes."); }

            this.Id = id;
            this.Length = data.Length;
            this.IsExtended = (id > 0x7FF);
            this.IsRemoteRequest = false;
            this.Data = data;
        }

        /// <summary>
        /// Create a new instance of remote request message.
        /// </summary>
        /// <param name="id">11-bit or 29-bit identifier.</param>
        /// <param name="length">Data length.</param>
        /// <exception cref="ArgumentOutOfRangeException">ID must be between 0 and 0x1FFFFFFF. -or- Length must be between 0 and 8 bytes.</exception>
        public CanankaMessage(int id, int length) {
            if ((id < 0x00000000) || (id > 0x1FFFFFFF)) { throw new ArgumentOutOfRangeException(nameof(id), "ID must be between 0 and 0x1FFFFFFF."); }
            if ((length < 0) || (length > 8)) { throw new ArgumentOutOfRangeException(nameof(length), "Length must be between 0 and 8 bytes."); }

            this.Id = id;
            this.Length = length;
            this.IsExtended = (id > 0x7FF);
            this.IsRemoteRequest = true;
            this.Data = null;
        }


        /// <summary>
        /// Gets ID.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets data length.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Gets if 29-bit ID is used.
        /// </summary>
        public bool IsExtended { get; }

        /// <summary>
        /// Gets if this message is remote request.
        /// </summary>
        public bool IsRemoteRequest { get; }

        private byte[] Data { get; }


        /// <summary>
        /// Returns data bytes.
        /// Null for remote request.
        /// </summary>
        public byte[] GetData() {
            if (this.Data != null) {
                var data = new byte[this.Data.Length];
                Buffer.BlockCopy(this.Data, 0, data, 0, data.Length);
                return data;
            }
            return null;
        }


        /// <summary>
        /// Returns text representation of message.
        /// </summary>
        public override string ToString() {
            var sb = new StringBuilder();
            if (this.IsExtended || (this.Id > 0x7FF)) {
                sb.Append(this.Id.ToString("X8", CultureInfo.InvariantCulture));
            } else {
                sb.Append(this.Id.ToString("X3", CultureInfo.InvariantCulture));
            }
            sb.Append("#");
            if (this.IsRemoteRequest) {
                sb.Append("R");
                if (this.Length > 0) { sb.Append(this.Length.ToString(CultureInfo.InvariantCulture)); }
            } else {
                foreach (var b in this.Data) {
                    sb.Append(b.ToString("X2", CultureInfo.InvariantCulture));
                }
            }
            return sb.ToString();
        }
    }


    /// <summary>
    /// Event arguments for message event.
    /// </summary>
    public class CanankaMessageEventArgs : EventArgs {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <exception cref="ArgumentNullException">Message cannot be null.</exception>
        public CanankaMessageEventArgs(CanankaMessage message) {
            this.Message = message ?? throw new ArgumentNullException(nameof(message), "Message cannot be null.");
        }


        /// <summary>
        /// CAN message.
        /// </summary>
        public CanankaMessage Message { get; }

    }
}
