//Copyright (c) 2010 Josip Medved <jmedved@jmedved.com>

//2010-09-11: Initial version.
//2011-02-16: Fixed ReadLine.
//            Added fix for serial ports above COM9.
//2011-04-02: Removed Flush from Close.
//            Port name is not immediately prepended with \\?\.
//2011-08-04: Compatible with Mono.
//2012-11-24: Suppressing bogus CA5122 warning (http://connect.microsoft.com/VisualStudio/feedback/details/729254/bogus-ca5122-warning-about-p-invoke-declarations-should-not-be-safe-critical).
//2014-06-14: Optimizing Mono check.


using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace Medo.IO {

    /// <summary>
    /// Represents a serial port resource.
    /// </summary>
    public class UartPort : IDisposable {

        /// <summary>
        /// Indicates that no time-out should occur.
        /// </summary>
        public const int InfiniteTimeout = -1;


        /// <summary>
        /// Initializes a new instance of the class using the specified port name.
        /// </summary>
        /// <param name="portName">The port to use.</param>
        public UartPort(string portName)
            : this(portName, 9600, Parity.None, 8, StopBits.One) {
        }

        /// <summary>
        /// Initializes a new instance of the class using the specified port name and baud rate.
        /// </summary>
        /// <param name="portName">The port to use.</param>
        /// <param name="baudRate">The baud rate.</param>
        public UartPort(string portName, int baudRate)
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
        /// <exception cref="System.ArgumentNullException">PortName cannot be null.</exception>
        public UartPort(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits) {
            if (portName == null) { throw new ArgumentNullException("portName", "PortName cannot be null."); }
            this.PortName = portName.Trim();
            if (IsRunningOnMono == false) { this.PortName = this.PortName.ToUpperInvariant(); }
            this.BaudRate = baudRate;
            this.Parity = parity;
            this.DataBits = dataBits;
            this.StopBits = stopBits;

            this.NewLine = "\n";
            this.ReadTimeout = UartPort.InfiniteTimeout;
            this.WriteTimeout = UartPort.InfiniteTimeout;
        }


        /// <summary>
        /// Gets or sets the port for communications, including but not limited to all available COM ports.
        /// </summary>
        public string PortName { get; set; }

        /// <summary>
        /// Gets or sets the serial baud rate.
        /// </summary>
        public int BaudRate { get; set; }

        /// <summary>
        /// Gets or sets the parity-checking protocol.
        /// </summary>
        public Parity Parity { get; set; }

        /// <summary>
        /// Gets or sets the standard length of data bits per byte.
        /// </summary>
        public int DataBits { get; set; }

        /// <summary>
        /// Gets or sets the standard number of stopbits per byte.
        /// </summary>
        public StopBits StopBits { get; set; }

        /// <summary>
        /// Gets or sets the value used to interpret the end of a call to the ReadLine and WriteLine methods.
        /// </summary>
        public string NewLine { get; set; }

        /// <summary>
        /// Gets or sets the number of milliseconds before a time-out occurs when a read operation does not finish.
        /// </summary>
        public int ReadTimeout { get; set; }

        /// <summary>
        /// Gets or sets the number of milliseconds before a time-out occurs when a write operation does not finish.
        /// </summary>
        public int WriteTimeout { get; set; }


        /// <summary>
        /// Gets the operating system file handle for the serial port.
        /// </summary>
        private CreateFileWHandle Handle { get; set; }

        /// <summary>
        /// Gets serial port used in case of Mono.
        /// </summary>
        private SerialPort FrameworkSerialPort { get; set; }

        /// <summary>
        /// Gets a value indicating whether port is open or closed.
        /// </summary>
        public bool IsOpen {
            get {
                if (IsRunningOnMono == false) {
                    return (this.Handle != null) && (this.Handle.IsClosed == false) && (this.Handle.IsInvalid == false);
                } else {
                    return (this.FrameworkSerialPort != null) && (this.FrameworkSerialPort.IsOpen);
                }
            }
        }

        /// <summary>
        /// Opens a new serial port connection.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Port is already open.</exception>
        /// <exception cref="System.IO.IOException">The port is in an invalid state. -or- An attempt to set the state of the underlying port failed.</exception>
        /// <exception cref="System.UnauthorizedAccessException">Access is denied to the port.</exception>
        public void Open() {
            if (this.IsOpen) { throw new InvalidOperationException("Port is already open."); }

            if (IsRunningOnMono == false) {

                var portName = (this.PortName.StartsWith(@"\\?\", StringComparison.Ordinal) ? "" : @"\\?\") + this.PortName;
                this.Handle = NativeMethods.CreateFileW(
                    portName,
                    NativeMethods.GENERIC_READ | NativeMethods.GENERIC_WRITE,
                    0, //exclusive access
                    IntPtr.Zero,
                    NativeMethods.OPEN_EXISTING,
                    0,
                    IntPtr.Zero
                    );

                if (this.IsOpen == false) { throw new IOException("The port is in an invalid state."); }

                var config = new NativeMethods.COMMCONFIG();
                config.dwSize = Marshal.SizeOf(config);
                config.wVersion = 1;
                config.wReserved = 0;
                config.dcb = new NativeMethods.DCB();
                config.dcb.DCBlength = Marshal.SizeOf(config.dcb);
                config.dcb.BaudRate = this.BaudRate;
                config.dcb.fBinary = 1;
                config.dcb.fParity = 0;
                config.dcb.fOutxCtsFlow = 0;
                config.dcb.fOutxDsrFlow = 0;
                config.dcb.fDtrControl = NativeMethods.DTR_CONTROL_ENABLE;
                config.dcb.fDsrSensitivity = 0;
                config.dcb.fTXContinueOnXoff = 0;
                config.dcb.fOutX = 0;
                config.dcb.fInX = 0;
                config.dcb.fErrorChar = 0;
                config.dcb.fNull = 0;
                config.dcb.fRtsControl = NativeMethods.RTS_CONTROL_ENABLE;
                config.dcb.fAbortOnError = 0;
                config.dcb.fDummy2 = 0;
                config.dcb.wReserved = 0;
                config.dcb.XonLim = 0;
                config.dcb.XoffLim = 0;
                config.dcb.ByteSize = (byte)this.DataBits;
                switch (this.Parity) {
                    case Parity.None: config.dcb.Parity = NativeMethods.NOPARITY; break;
                    case Parity.Odd: config.dcb.Parity = NativeMethods.ODDPARITY; break;
                    case Parity.Even: config.dcb.Parity = NativeMethods.EVENPARITY; break;
                    case Parity.Mark: config.dcb.Parity = NativeMethods.MARKPARITY; break;
                    case Parity.Space: config.dcb.Parity = NativeMethods.SPACEPARITY; break;
                    default: throw new IOException("An attempt to set the state of the underlying port failed (invalid parity).");
                }
                switch (this.StopBits) {
                    case StopBits.One: config.dcb.StopBits = NativeMethods.ONESTOPBIT; break;
                    case StopBits.OnePointFive: config.dcb.StopBits = NativeMethods.ONE5STOPBITS; break;
                    case StopBits.Two: config.dcb.StopBits = NativeMethods.TWOSTOPBITS; break;
                    default: throw new IOException("An attempt to set the state of the underlying port failed (invalid stop bit count).");
                }
                config.dcb.XonChar = 0x11;
                config.dcb.XoffChar = 0x13;
                config.dcb.ErrorChar = 0x3F;
                config.dcb.EofChar = 0;
                config.dcb.EvtChar = 0;
                config.dcb.wReserved1 = 0;
                config.dwProviderSubType = NativeMethods.PST_RS232;
                config.dwProviderOffset = 0;
                config.dwProviderSize = 0;
                config.wcProviderData = null;
                var resultConfigSet = NativeMethods.SetCommConfig(this.Handle, ref config, config.dwSize);
                if (resultConfigSet != true) { throw new IOException("An attempt to set the state of the underlying port failed.", new Win32Exception()); }

                var commTimeouts = new NativeMethods.COMMTIMEOUTS();
                commTimeouts.ReadIntervalTimeout = NativeMethods.MAXDWORD;
                commTimeouts.ReadTotalTimeoutMultiplier = (this.ReadTimeout == 0) ? 0 : -1;
                commTimeouts.ReadTotalTimeoutConstant = (this.ReadTimeout != InfiniteTimeout) ? this.ReadTimeout : -2;
                commTimeouts.WriteTotalTimeoutMultiplier = 0;
                commTimeouts.WriteTotalTimeoutConstant = (this.ReadTimeout != InfiniteTimeout) ? this.WriteTimeout : 0;
                var resultTimeouts = NativeMethods.SetCommTimeouts(this.Handle, ref commTimeouts);
                if (resultTimeouts != true) { throw new IOException("An attempt to set the state of the underlying port failed.", new Win32Exception()); }

            } else {

                var portName = (this.PortName.StartsWith(@"\\?\", StringComparison.Ordinal) ? this.PortName.Remove(0, 4).Trim() : this.PortName.Trim());
                this.FrameworkSerialPort = new SerialPort(portName, this.BaudRate, this.Parity, this.DataBits, this.StopBits);
                this.FrameworkSerialPort.ReadTimeout = this.ReadTimeout;
                this.FrameworkSerialPort.WriteTimeout = this.WriteTimeout;
                this.FrameworkSerialPort.DtrEnable = true;
                this.FrameworkSerialPort.RtsEnable = true;
                this.FrameworkSerialPort.Open();

            }

            this.Purge();
        }

        /// <summary>
        /// Closes the port connection.
        /// </summary>
        public void Close() {
            if (this.IsOpen) {
                if (IsRunningOnMono == false) {
                    this.Handle.Close();
                } else {
                    this.FrameworkSerialPort.Close();
                }
            }
        }

        /// <summary>
        /// Gets the number of bytes of data in the receive buffer.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Cannot read state.</exception>
        public int BytesToRead {
            get {
                if (IsRunningOnMono == false) {
                    var errorFlags = IntPtr.Zero;
                    var stats = new NativeMethods.COMSTAT();
                    var result = NativeMethods.ClearCommError(this.Handle, ref errorFlags, ref stats);
                    if (result != true) {
                        throw new InvalidOperationException("Cannot read state (native error " + Marshal.GetLastWin32Error().ToString(CultureInfo.InvariantCulture) + ").");
                    }
                    return (int)stats.cbInQue;
                } else {
                    return this.FrameworkSerialPort.BytesToRead;
                }
            }
        }

        /// <summary>
        /// Discards data from the serial driver's receive and transmit buffer.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Port is not open.</exception>
        /// <exception cref="System.IO.IOException">Communication error.</exception>
        public void Purge() {
            if (this.IsOpen == false) { throw new InvalidOperationException("Port is not open."); }
            if (IsRunningOnMono == false) {
                var result = NativeMethods.PurgeComm(this.Handle, NativeMethods.PURGE_TXABORT | NativeMethods.PURGE_TXCLEAR | NativeMethods.PURGE_RXABORT | NativeMethods.PURGE_RXCLEAR);
                if (result != true) { throw new IOException("Communication error.", new Win32Exception()); }
            } else {
                this.FrameworkSerialPort.DiscardInBuffer();
                this.FrameworkSerialPort.DiscardOutBuffer();
            }
        }

        /// <summary>
        /// Clears all buffers and causes any buffered data to be written to the underlying device.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Port is not open.</exception>
        /// <exception cref="System.IO.IOException">Communication error.</exception>
        public void Flush() {
            if (this.IsOpen == false) { throw new InvalidOperationException("Port is not open."); }
            if (IsRunningOnMono == false) {
                var result = NativeMethods.FlushFileBuffers(this.Handle);
                if (result != true) { throw new IOException("Communication error.", new Win32Exception()); }
            } else {
                this.FrameworkSerialPort.BaseStream.Flush();
            }
        }

        /// <summary>
        /// Discards data from the serial driver's receive buffer.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Port is not open.</exception>
        /// <exception cref="System.IO.IOException">Communication error.</exception>
        public void DiscardInBuffer() {
            if (this.IsOpen == false) { throw new InvalidOperationException("Port is not open."); }
            if (IsRunningOnMono == false) {
                var result = NativeMethods.PurgeComm(this.Handle, NativeMethods.PURGE_RXABORT | NativeMethods.PURGE_RXCLEAR);
                if (result != true) { throw new IOException("Communication error.", new Win32Exception()); }
            } else {
                this.FrameworkSerialPort.DiscardInBuffer();
            }
        }

        /// <summary>
        /// Discards data from the serial driver's transmit buffer.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Port is not open.</exception>
        /// <exception cref="System.IO.IOException">Communication error.</exception>
        public void DiscardOutBuffer() {
            if (this.IsOpen == false) { throw new InvalidOperationException("Port is not open."); }
            if (IsRunningOnMono == false) {
                var result = NativeMethods.PurgeComm(this.Handle, NativeMethods.PURGE_TXABORT | NativeMethods.PURGE_TXCLEAR);
                if (result != true) { throw new IOException("Communication error.", new Win32Exception()); }
            } else {
                this.FrameworkSerialPort.DiscardOutBuffer();
            }
        }

        /// <summary>
        /// Reads a number of bytes from the input buffer and writes those bytes into a byte array at the specified offset.
        /// </summary>
        /// <param name="buffer">The byte array to write the input to.</param>
        /// <exception cref="System.ArgumentNullException">The buffer passed is null.</exception>
        public int Read(byte[] buffer) {
            if (buffer == null) { throw new ArgumentNullException("buffer", "The buffer passed is null."); }
            return Read(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Reads a number of bytes from the input buffer and writes those bytes into a byte array at the specified offset.
        /// </summary>
        /// <param name="buffer">The byte array to write the input to.</param>
        /// <param name="offset">The offset in the buffer array to begin writing.</param>
        /// <param name="count">The number of bytes to read.</param>
        /// <exception cref="System.InvalidOperationException">Port is not open.</exception>
        /// <exception cref="System.ArgumentNullException">The buffer passed is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The offset or count is outside of valid buffer region.</exception>
        /// <exception cref="System.TimeoutException">The operation did not complete before the time-out period ended.</exception>
        /// <exception cref="System.IO.IOException">Communication error.</exception>
        public int Read(byte[] buffer, int offset, int count) {
            if (this.IsOpen == false) { throw new InvalidOperationException("Port is not open."); }
            if (buffer == null) { throw new ArgumentNullException("buffer", "The buffer passed is null."); }
            if ((offset < 0) || (count < 0) || (offset + count > buffer.Length)) { throw new ArgumentOutOfRangeException("offset", "The offset or count is outside of valid buffer region."); }
            if (IsRunningOnMono == false) {
                var nPtrRead = IntPtr.Zero;
                var overlapped = new NativeOverlapped();
                var result = NativeMethods.ReadFile(this.Handle, buffer, count, ref nPtrRead, ref overlapped);
                if (result != true) { throw new IOException("Communication error.", new Win32Exception()); }
                var nRead = nPtrRead.ToInt32();
                if (nRead < count) { throw new TimeoutException("The operation did not complete before the time-out period ended."); }
                return nRead;
            } else {
                return this.FrameworkSerialPort.Read(buffer, offset, count);
            }
        }

        /// <summary>
        /// Reads up to the NewLine value in the input buffer.
        /// </summary>
        public string ReadLine() {
            var sb = new StringBuilder();
            var buffer = new byte[1];
            for (; ; ) {
                this.Read(buffer);
                sb.Append(ASCIIEncoding.ASCII.GetString(buffer));
                if (sb.Length >= this.NewLine.Length) {
                    bool isNewLine = true;
                    for (int i = 0; i < this.NewLine.Length; ++i) {
                        if (sb[sb.Length - this.NewLine.Length + i] != this.NewLine[i]) {
                            isNewLine = false;
                        }
                    }
                    if (isNewLine) { break; }
                }
            }
            return sb.ToString(0, sb.Length - this.NewLine.Length);
        }


        /// <summary>
        /// Writes a specified number of bytes to the serial port using data from a buffer.
        /// </summary>
        /// <param name="buffer">The byte array that contains the data to write to the port.</param>
        /// <exception cref="System.ArgumentNullException">The buffer passed is null.</exception>
        public int Write(byte[] buffer) {
            if (buffer == null) { throw new ArgumentNullException("buffer", "The buffer passed is null."); }
            return Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Writes a specified number of bytes to the serial port using data from a buffer.
        /// </summary>
        /// <param name="buffer">The byte array that contains the data to write to the port.</param>
        /// <param name="offset">The zero-based byte offset in the buffer parameter at which to begin copying bytes to the port.</param>
        /// <param name="count">The number of bytes to write.</param>
        /// <exception cref="System.InvalidOperationException">Port is already open.</exception>
        /// <exception cref="System.ArgumentNullException">The buffer passed is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The offset or count is outside of valid buffer region.</exception>
        /// <exception cref="System.TimeoutException">The operation did not complete before the time-out period ended.</exception>
        /// <exception cref="System.IO.IOException">Communication error.</exception>
        public int Write(byte[] buffer, int offset, int count) {
            if (this.IsOpen == false) { throw new InvalidOperationException("Port is not open."); }
            if (buffer == null) { throw new ArgumentNullException("buffer", "The buffer passed is null."); }
            if ((offset < 0) || (count < 0) || (offset + count > buffer.Length)) { throw new ArgumentOutOfRangeException("offset", "The offset or count is outside of valid buffer region."); }

            if (IsRunningOnMono == false) {
                if ((offset != 0) || (buffer.Length != count)) {
                    byte[] newBuffer = new byte[count];
                    Buffer.BlockCopy(buffer, offset, newBuffer, 0, count);
                    buffer = newBuffer;
                }
                var nPtrWritten = IntPtr.Zero;
                var overlapped = new NativeOverlapped();
                var result = NativeMethods.WriteFile(this.Handle, buffer, count, ref nPtrWritten, ref overlapped);
                if (result != true) { throw new Win32Exception("Communication error.", new Win32Exception()); }
                var nWriten = nPtrWritten.ToInt32();
                if (nWriten < count) { throw new TimeoutException("The operation did not complete before the time-out period ended."); }
                return nWriten;
            } else {
                this.FrameworkSerialPort.Write(buffer, offset, count);
                return count;
            }
        }

        /// <summary>
        /// Writes the specified string and the NewLine value to the output buffer.
        /// </summary>
        /// <param name="text">The string to write to the output buffer.</param>
        public int WriteLine(string text) {
            var buffer = ASCIIEncoding.ASCII.GetBytes(text + this.NewLine);
            return Write(buffer);
        }




        private static class NativeMethods {

            public const UInt32 FILE_SHARE_READ = 0x00000001;
            public const UInt32 FILE_SHARE_WRITE = 0x00000002;
            public const UInt32 GENERIC_READ = 0x80000000;
            public const UInt32 GENERIC_WRITE = 0x40000000;
            public const UInt32 OPEN_EXISTING = 0x00000003;
            public const UInt32 PURGE_TXABORT = 0x0001;
            public const UInt32 PURGE_RXABORT = 0x0002;
            public const UInt32 PURGE_TXCLEAR = 0x0004;
            public const UInt32 PURGE_RXCLEAR = 0x0008;
            public const Byte NOPARITY = 0;
            public const Byte ODDPARITY = 1;
            public const Byte EVENPARITY = 2;
            public const Byte MARKPARITY = 3;
            public const Byte SPACEPARITY = 4;
            public const Byte ONESTOPBIT = 0;
            public const Byte ONE5STOPBITS = 1;
            public const Byte TWOSTOPBITS = 2;
            public const UInt32 PST_RS232 = 0x6;
            public const UInt32 DTR_CONTROL_DISABLE = 0;
            public const UInt32 DTR_CONTROL_ENABLE = 1;
            public const UInt32 DTR_CONTROL_HANDSHAKE = 2;
            public const UInt32 RTS_CONTROL_DISABLE = 0;
            public const UInt32 RTS_CONTROL_ENABLE = 1;
            public const UInt32 RTS_CONTROL_HANDSHAKE = 2;
            public const UInt32 RTS_CONTROL_TOGGLE = 3;
            public const UInt32 MAXDWORD = 0xFFFFFFFF;


            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            public struct COMMCONFIG {
                public Int32 dwSize;
                public UInt16 wVersion;
                public UInt16 wReserved;
                public DCB dcb;
                public UInt32 dwProviderSubType;
                public UInt32 dwProviderOffset;
                public UInt32 dwProviderSize;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
                public String wcProviderData;
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct COMSTAT {
                public UInt32 bitvector1;
                public UInt32 cbInQue;
                public UInt32 cbOutQue;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct COMMTIMEOUTS {
                public UInt32 ReadIntervalTimeout;
                public Int32 ReadTotalTimeoutMultiplier;
                public Int32 ReadTotalTimeoutConstant;
                public Int32 WriteTotalTimeoutMultiplier;
                public Int32 WriteTotalTimeoutConstant;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct DCB {
                public Int32 DCBlength;
                public Int32 BaudRate;
                public UInt32 bitvector1;
                public UInt16 wReserved;
                public UInt16 XonLim;
                public UInt16 XoffLim;
                public Byte ByteSize;
                public Byte Parity;
                public Byte StopBits;
                public Byte XonChar;
                public Byte XoffChar;
                public Byte ErrorChar;
                public Byte EofChar;
                public Byte EvtChar;
                public UInt16 wReserved1;

                public UInt32 fBinary {
                    set { this.bitvector1 = ((UInt32)((value | this.bitvector1))); }
                }
                public UInt32 fParity {
                    set { this.bitvector1 = ((UInt32)(((value * 0x2) | this.bitvector1))); }
                }
                public UInt32 fOutxCtsFlow {
                    set { this.bitvector1 = ((UInt32)(((value * 0x4) | this.bitvector1))); }
                }
                public UInt32 fOutxDsrFlow {
                    set { this.bitvector1 = ((UInt32)(((value * 0x8) | this.bitvector1))); }
                }
                public UInt32 fDtrControl {
                    set { this.bitvector1 = ((UInt32)(((value * 0x10) | this.bitvector1))); }
                }
                public UInt32 fDsrSensitivity {
                    set { this.bitvector1 = ((UInt32)(((value * 0x40) | this.bitvector1))); }
                }
                public UInt32 fTXContinueOnXoff {
                    set { this.bitvector1 = ((UInt32)(((value * 0x80) | this.bitvector1))); }
                }
                public UInt32 fOutX {
                    set { this.bitvector1 = ((UInt32)(((value * 0x100) | this.bitvector1))); }
                }
                public UInt32 fInX {
                    set { this.bitvector1 = ((UInt32)(((value * 0x200) | this.bitvector1))); }
                }
                public UInt32 fErrorChar {
                    set { this.bitvector1 = ((UInt32)(((value * 0x400) | this.bitvector1))); }
                }
                public UInt32 fNull {
                    set { this.bitvector1 = ((UInt32)(((value * 0x800) | this.bitvector1))); }
                }
                public UInt32 fRtsControl {
                    set { this.bitvector1 = ((UInt32)(((value * 0x1000) | this.bitvector1))); }
                }
                public UInt32 fAbortOnError {
                    set { this.bitvector1 = ((UInt32)(((value * 0x4000) | this.bitvector1))); }
                }
                public UInt32 fDummy2 {
                    set { this.bitvector1 = ((UInt32)(((value * 0x8000) | this.bitvector1))); }
                }
            }


            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule", Justification = "Warning is bogus.")]
            [DllImport("kernel32.dll", EntryPoint = "ClearCommError", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern Boolean ClearCommError([In()] CreateFileWHandle hFile, ref IntPtr lpErrors, ref COMSTAT lpStat);

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule", Justification = "Warning is bogus.")]
            [DllImport("kernel32.dll", EntryPoint = "CloseHandle", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern Boolean CloseHandle([In()] IntPtr hObject);

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule", Justification = "Warning is bogus.")]
            [DllImport("kernel32.dll", EntryPoint = "CreateFileW", SetLastError = true)]
            public static extern CreateFileWHandle CreateFileW([In()] [MarshalAs(UnmanagedType.LPWStr)] String lpFileName, UInt32 dwDesiredAccess, UInt32 dwShareMode, [In()] IntPtr lpSecuritys, UInt32 dwCreationDisposition, UInt32 dwFlagsAndAttributes, [In()] IntPtr hTemplateFile);

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule", Justification = "Warning is bogus.")]
            [DllImport("kernel32.dll", EntryPoint = "FlushFileBuffers", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern Boolean FlushFileBuffers([In()] CreateFileWHandle hFile);

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule", Justification = "Warning is bogus.")]
            [DllImport("kernel32.dll", EntryPoint = "PurgeComm")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern Boolean PurgeComm([In()] CreateFileWHandle hFile, UInt32 dwFlags);

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule", Justification = "Warning is bogus.")]
            [DllImport("kernel32.dll", EntryPoint = "ReadFile", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern Boolean ReadFile([In()] CreateFileWHandle hFile, Byte[] lpBuffer, Int32 nNumberOfBytesToRead, ref IntPtr lpNumberOfBytesRead, ref NativeOverlapped lpOverlapped);

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule", Justification = "Warning is bogus.")]
            [DllImport("kernel32.dll", EntryPoint = "SetCommConfig", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern Boolean SetCommConfig([In()] CreateFileWHandle hCommDev, [In()] ref COMMCONFIG lpCC, Int32 dwSize);

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule", Justification = "Warning is bogus.")]
            [DllImport("kernel32.dll", EntryPoint = "SetCommTimeouts", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern Boolean SetCommTimeouts([In()] CreateFileWHandle hFile, [In()] ref COMMTIMEOUTS lpCommTimeouts);

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule", Justification = "Warning is bogus.")]
            [DllImport("kernel32.dll", EntryPoint = "WriteFile", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern Boolean WriteFile([In()] CreateFileWHandle hFile, [In()] Byte[] lpBuffer, Int32 nNumberOfBytesToWrite, ref IntPtr lpNumberOfBytesWritten, ref NativeOverlapped lpOverlapped);

        }

        private class CreateFileWHandle : SafeHandleZeroOrMinusOneIsInvalid {

            public CreateFileWHandle()
                : base(true) {
            }

            protected override bool ReleaseHandle() {
                return NativeMethods.CloseHandle(this.handle);
            }

            public override string ToString() {
                return base.handle.ToString();
            }

        }


        /// <summary>
        /// Releases all resources.
        /// </summary>
        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing) {
            this.Close();
            if (disposing) {
                this.Handle.Dispose();
            }
        }


        private readonly bool IsRunningOnMono = (Type.GetType("Mono.Runtime") != null);

    }
}
