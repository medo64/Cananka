#### Cananka USB Protocol ####

This device is compatible with SLCAN protocol and most of its communication will
follow the same. Serial port parameters are 115200,N,8,1.


##### Channel commands #####

###### Set speed (S) ######

This command will set the CAN bus speed.

|                 | Send            | Receive         | Notes                                                      |
|-----------------|-----------------|-----------------|------------------------------------------------------------|
| Syntax          | S{1:index}`CR`  | `CR` -or- `BEL` |                                                            |
| Example         | S4`CR`          | `CR`            | Speed is set to 125Kbit/s                                  |
| Example (error) | Sx`CR`          | `BEL`           | Invalid speed (p!)                                         |
| Example (error) | S4`CR`          | `BEL`           | Setting speed on open channel (a!)                         |
| Example (error) | S0`CR`          | `BEL`           | Unsupported speed (e!)                                     |

The following values are allowed (default is 125 Kbit/s):

| Index |           Speed |    BRP |    PRSEG |   SEG1PH |   SEG2PH |      SJW |     Range |
|------:|----------------:|-------:|---------:|---------:|---------:|---------:|----------:|
|  *0*  | *Not supported* |  *N/A* |    *N/A* |    *N/A* |    *N/A* |    *N/A* |     *N/A* |
|   1   |     20 Kbit/s   |   99   |   5 TQ   |   4 TQ   |   2 TQ   |   1 TQ   |  3000 m   |
|   2   |     50 Kbit/s   |   39   |   5 TQ   |   4 TQ   |   2 TQ   |   1 TQ   |  1000 m   |
|   3   |    100 Kbit/s   |   19   |   5 TQ   |   4 TQ   |   2 TQ   |   1 TQ   |   700 m   |
| **4** |  **125 Kbit/s** | **15** | **5 TQ** | **4 TQ** | **2 TQ** | **1 TQ** | **600 m** |
|   5   |    250 Kbit/s   |    7   |   5 TQ   |   4 TQ   |   2 TQ   |   1 TQ   |   200 m   |
|   6   |    500 Kbit/s   |    3   |   5 TQ   |   4 TQ   |   2 TQ   |   1 TQ   |   100 m   |
|   7   |    800 Kbit/s   |    2   |   4 TQ   |   3 TQ   |   2 TQ   |   1 TQ   |    50 m   |
|   8   |   1000 Kbit/s   |    1   |   6 TQ   |   3 TQ   |   2 TQ   |   1 TQ   |    50 m   |


###### Set custom speed (s) ######

This command will set the custom speed by directly manipulating BTR0 and BTR1
registers. As these registers do not exist natively in chip used, their values
are mapped to Microchip's.

|                 | Send                  | Receive               | Notes                                                                         |
|-----------------|-----------------------|-----------------------|-------------------------------------------------------------------------------|
| Syntax          | s{2:btr0}{2:btr1}`CR` | `CR` -or- `BEL`       |                                                                               |
| Example         | s0F98`CR`             | `CR`                  | Speed is set to 125Kbit/s (SJW:1TQ; BRP:15; SAM:triple; TSEG2:2TQ; TSEG1:9TQ) |
| Example (error) | sg`CR`                | `BEL`                 | Invalid number (p!)                                                           |
| Example (error) | s0F98`CR`             | `BEL`                 | Setting speed on open channel (a!)                                            |
| Example (error) | s0000`CR`             | `BEL`                 | Unsupported speed (e!)                                                        |

Following bit values are used (combined BTR0 and BTR1):

| Position | Length | Name  | Description                                                                                     |
|---------:|-------:|-------|-------------------------------------------------------------------------------------------------|
|        0 |      2 | SJW   | Synchronization Jump Width (0-3; 1-4 TQ); directly copied to BRGCON1.SJW                        |
|        2 |      6 | BRP   | Baud Rate Prescaler (1-63); directly copied to BRGCON1.BRP                                      |
|        8 |      1 | SAM   | If 0, one sample is taken; if 1, three samples are taken; directly copied to BRGCON2.SAM        |
|        9 |      3 | TSEG2 | Time Segment 2 (0-7; 1-8 TQ); directly copied to BRGCON3.SEG2PH                                 |
|       12 |      4 | TSEG1 | Time Segment 1 (1-15; 2-16 TQ); total amount is shared between BRGCON2.PRSEG and BRGCON2.SEG1PH |


###### Open channel (O) ######

Opening the channel so that CAN bus data can be sent and received.

|                 | Send    | Receive         | Notes                                     |
|-----------------|---------|-----------------|-------------------------------------------|
| Syntax          | O`CR`   | `CR` -or- `BEL` |                                           |
| Example         | O`CR`   | `CR`            | Open channel                              |
| Example (error) | O`CR`   | `BEL`           | Channel is already open (a!)              |


###### Open channel in listen-only mode (L) ######

Opening the channel so that CAN bus data can be only received.

|                 | Send    | Receive         | Notes                                                    |
|-----------------|---------|-----------------|----------------------------------------------------------|
| Syntax          | L`CR`   | `CR` -or- `BEL` |                                                          |
| Example         | L`CR`   | `CR`            | Open channel                                             |
| Example (error) | L`CR`   | `BEL`           | Channel is already open (a!)                             |


###### Open channel in loopback mode (l) ######

Opening the channel in loopback mode.

|                 | Send    | Receive         | Notes                                                 |
|-----------------|---------|-----------------|-------------------------------------------------------|
| Syntax          | l`CR`   | `CR` -or- `BEL` |                                                       |
| Example         | l`CR`   | `CR`            | Open channel                                          |
| Example (error) | l`CR`   | `BEL`           | Channel is already open (a!)                          |


###### Close channel (C) ######

Close the CAN bus.

|                 | Send    | Receive         | Notes                                       |
|-----------------|---------|-----------------|---------------------------------------------|
| Syntax          | C`CR`   | `CR` -or- `BEL` |                                             |
| Example         | C`CR`   | `CR`            | Close channel                               |
| Example (error) | C`CR`   | `BEL`           | Channel is already closed (a!)              |


##### Data commands #####

###### Transmit standard frame (t) ######

Transmits standard frame with 11-bit ID.

|                 | Send                            | Receive         | Notes                                                         |
|-----------------|---------------------------------|-----------------|---------------------------------------------------------------|
| Syntax          | t{3:id}{1:length}{0-8:data}`CR` | `CR` -or- `BEL` |                                                               |
| Example         | t12324142`CR`                   | `CR`            | Transmits standard frame (ID=0x123 Length=2 Data=0x4142)      |
| Example (error) | t`CR`                           | `BEL`           | No ID (p!)                                                    |
| Example (error) | t1232`CR`                       | `BEL`           | No data (p!)                                                  |
| Example (error) | t123241429`CR`                  | `BEL`           | Extra data (p!)                                               |
| Example (error) | t123941429`CR`                  | `BEL`           | Invalid length (p!)                                           |
| Example         | t12324142`CR`                   | `BEL`           | Channel not open (a!)                                         |
| Example         | t12324142`CR`                   | `BEL`           | Cannot transmit in read-only (e!)                             |


###### Transmit extended frame (T) ######

Transmits standard frame with 29-bit ID.

|                 | Send                            | Receive         | Notes                                                         |
|-----------------|---------------------------------|-----------------|---------------------------------------------------------------|
| Syntax          | T{8:id}{1:length}{0-8:data}`CR` | `CR` -or- `BEL` |                                                               |
| Example         | T1234567824142`CR`              | `CR`            | Transmits standard frame (ID=0x12345678 Length=2 Data=0x4142) |
| Example (error) | T`CR`                           | `BEL`           | No ID (p!)                                                    |
| Example (error) | T123456782`CR`                  | `BEL`           | No data (p!)                                                  |
| Example (error) | T12345678241429`CR`             | `BEL`           | Extra data (p!)                                               |
| Example (error) | T12345678941429`CR`             | `BEL`           | Invalid length (p!)                                           |
| Example         | T1234567824142`CR`              | `BEL`           | Channel not open (a!)                                         |
| Example         | T1234567824142`CR`              | `BEL`           | Cannot transmit in read-only (e!)                             |


###### Transmit standard remote frame (r) ######

Transmits remote frame with 11-bit ID.

|                 | Send                            | Receive         | Notes                                                         |
|-----------------|---------------------------------|-----------------|---------------------------------------------------------------|
| Syntax          | r{3:id}{1:length}`CR`           | `CR` -or- `BEL` |                                                               |
| Example         | r1232`CR`                       | `CR`            | Transmits remote frame (ID=0x123 Length=2)                    |
| Example (error) | r`CR`                           | `BEL`           | No ID (p!)                                                    |
| Example (error) | r12324142`CR`                   | `BEL`           | Extra data (p!)                                               |
| Example (error) | r1239`CR`                       | `BEL`           | Invalid length (p!)                                           |
| Example         | r1232`CR`                       | `BEL`           | Channel not open (a!)                                         |
| Example         | r1232`CR`                       | `BEL`           | Cannot transmit in read-only (e!)                             |


###### Transmit extended remote frame (R) ######

Transmits remote frame with 29-bit ID.

|                 | Send                            | Receive         | Notes                                                         |
|-----------------|---------------------------------|-----------------|---------------------------------------------------------------|
| Syntax          | R{8:id}{1:length}`CR`           | `CR` -or- `BEL` |                                                               |
| Example         | R123456782`CR`                  | `CR`            | Transmits remote frame (ID=0x12345678 Length=2                |
| Example (error) | R`CR`                           | `BEL`           | No ID (p!)                                                    |
| Example (error) | R1234567824142`CR`              | `BEL`           | Extra data (p!)                                               |
| Example (error) | R123456789`CR`                  | `BEL`           | Invalid length (p!)                                           |
| Example         | R123456782`CR`                  | `BEL`           | Channel not open (a!)                                         |
| Example         | R123456782`CR`                  | `BEL`           | Cannot transmit in read-only (e!)                             |


###### Auto-polling (X) ######

Determines if automatic or manual pooling is to be used. Automatic pooling is default.

|                 | Send                            | Receive         | Notes                                                         |
|-----------------|---------------------------------|-----------------|---------------------------------------------------------------|
| Syntax          | X{1:state}`CR`                  | `CR` -or- `BEL` |                                                               |
| Example         | X0`CR`                          | `CR`            | Disable automatic polling                                     |
| Example (error) | X`CR`                           | `BEL`           | No state (p!)                                                 |


###### Poll one (P) ######

Reports next message waiting in buffer.

|                 | Send                            | Receive         | Notes                                                         |
|-----------------|---------------------------------|-----------------|---------------------------------------------------------------|
| Syntax          | P`CR`                           | `CR` -or- `BEL` |                                                               |
| Example (error) | P0`CR`                          | `BEL`           | Extra data (p!)                                               |
| Example (error) | P`CR`                           | `BEL`           | Cannot poll in auto-polling mode (e!)                         |


###### Poll all (A) ######

Reports all messages waiting in buffer.

|                 | Send                            | Receive         | Notes                                                         |
|-----------------|---------------------------------|-----------------|---------------------------------------------------------------|
| Syntax          | A`CR`                           | `CR` -or- `BEL` |                                                               |
| Example (error) | A0`CR`                          | `BEL`           | Extra data (p!)                                               |
| Example (error) | A`CR`                           | `BEL`           | Cannot poll in auto-polling mode (e!)                         |


##### Status commands #####

###### Version (V) ######

Returns hardware and software version.

|                 | Send    | Receive                                  | Notes                                     |
|-----------------|---------|------------------------------------------|-------------------------------------------|
| Syntax          | V`CR`   | V{2:software}{2:hardware}`CR` -or- `BEL` |                                           |
| Example         | V`CR`   | V1010`CR`                                | Hardware=1.0  Software=1.0                |
| Example (error) | V0`CR`  | `BEL`                                    | Invalid parameters (p!)                   |

Second digit of hardware revisions shows board type:
  * 0: Cananka
  * 1: Cananka RJ45
  * 2: Cananka Mini


###### Serial number (N) ######

Returns serial number of device. Not used.

|                 | Send    | Receive                    | Notes                                     |
|-----------------|---------|----------------------------|-------------------------------------------|
| Syntax          | N`CR`   | N{4:number}`CR` -or- `BEL` |                                           |
| Example         | N`CR`   | N0000`CR`                  | Always returns 0x0000                     |
| Example (error) | N0`CR`  | `BEL`                      | Invalid parameters (p!)                   |


###### Flags (F) ######

Returns current error flags.

|                 | Send    | Receive                   | Notes                                     |
|-----------------|---------|---------------------------|-------------------------------------------|
| Syntax          | F`CR`   | F{2:flags}`CR` -or- `BEL` |                                           |
| Example         | F`CR`   | F00`CR`                   | No errors                                 |
| Example (error) | F0`CR`  | `BEL`                     | Invalid parameters (p!)                   |

Following are bits and their meanings:

| Bit | Value | Description      |
|-----|-------|------------------|
|   0 | 0x01  | Rx queue full    |
|   1 | 0x02  | Tx queue full    |
|   2 | 0x04  | Tx/Rx warning    |
|   3 | 0x08  | Rx overflow      |
|   4 | 0x10  | -                |
|   5 | 0x20  | Error-passive    |
|   6 | 0x40  | Arbitration lost |
|   7 | 0x80  | Bus error        |


##### Permanent commands #####

###### Set UART speed (U) ######

This command will set the UART speed. Device will reset afterward. Speed will be
set permanently and used upon the next startup.

|                 | Send            | Receive         | Notes                                                      |
|-----------------|-----------------|-----------------|------------------------------------------------------------|
| Syntax          | U{1:index}`CR`  | `CR` -or- `BEL` |                                                            |
| Example         | U1`CR`          | `CR`            | Speed is set to 115200 baud (default)                      |
| Example         | Q~`CR`          | `CR`            | Revert to default (i.e. 115200)                            |
| Example (error) | Ux`CR`          | `BEL`           | Invalid speed (p!)                                         |
| Example (error) | U4`CR`          | `BEL`           | Setting speed on open channel (a!)                         |
| Example (error) | U9`CR`          | `BEL`           | Unsupported speed (e!)                                     |

The following values are allowed (default is 115200 baud):

| Index | Baud Rate  | Notes                                               |
|------:|-----------:|-----------------------------------------------------|
|   Y   |  *921600*  | Supported only on USB revision C (FTDI) devices     |
|   Z   |  *460800*  | Supported only on USB revision D (MCP2221A) devices |
|   0   |  *230400*  | Supported only on USB revision D (MCP2221A) devices |
| **1** | **115200** |                                                     |
|   2   |    57600   |                                                     |
|   3   |    38400   |                                                     |
|   4   |    19200   |                                                     |
|   5   |     9600   |                                                     |
|   6   |     2400   |                                                     |


###### Auto-startup (Q) ######

Saves current CAN bus settings for use upon the next boot. Optionally it can
also open port automatically.

|                 | Send            | Receive         | Notes                                                                                    |
|-----------------|-----------------|-----------------|------------------------------------------------------------------------------------------|
| Syntax          | Q{1:index}`CR`  | `CR` -or- `BEL` |                                                                                          |
| Example         | Q0`CR`          | `CR`            | Current CAN bus speed is saved for use on next boot                                      |
| Example         | Q1`CR`          | `CR`            | Same as previous, with the addition of automatically opening CAN bus                     |
| Example         | Q2`CR`          | `CR`            | Same as previous, with the addition of automatically opening CAN bus in listen-only mode |
| Example         | Q~`CR`          | `CR`            | Revert to default (i.e. no auto-startup, 125 Kbit/s)                                     |
| Example (error) | Qx`CR`          | `BEL`           | Invalid index (p!)                                                                       |
| Example (error) | Q9`CR`          | `BEL`           | Unsupported index (e!)                                                                   |


##### Extra commands #####

In addition to standard SLCAN commands, there are additional commands available.


###### Debug mode (*D) ######

Turns on/off various features that will help with debugging:

|                  | Send            | Receive         | Notes                                                  |
|------------------|-----------------|-----------------|--------------------------------------------------------|
| Syntax (set)     | *D{2:state}`CR` | `CR`            | Configures debugging features                          |
| Syntax (set all) | *D{1:state}`CR` | `CR`            | Configures all debugging features (0:off 1:on)         |
| Syntax (query)   | *D`CR`          | *D{2:state}`CR` | Returns the current state                              |
| Example          | *D1`CR`         | `CR``LF`        | Turns on all debugging features (bits 0-7)             |
| Example          | *D00`CR`        | `CR``LF`        | Turns debugging features off                           |
| Example          | *D07`CR`        | `CR``LF`        | Turns on all debugging features except echo            |
| Example (error)  | *DFF`CR`        | `BEL`           | Unsupported flags (x!)                                 |
| Example (error)  | *D8`CR`         | `BEL`           | Invalid value (p!)                                     |
| Example (error)  | *D888`CR`       | `BEL`           | Invalid value (p!)                                     |

Following are bits and their meanings:

| Bit | Value |                | Description                                                                                                           |
|-----|-------|----------------|-----------------------------------------------------------------------------------------------------------------------|
|   0 |  0x01 | Extra LF       | Send LF after each command is executed (in addition to CR or BEL)                                                     |
|   1 |  0x02 | Error Detail   | If set, every error is accompanied by additional letter specifying the nature of error and an extra exclamation point |
|   2 |  0x04 | Not used       |                                                                                                                       |
|   3 |  0x08 | Not used       |                                                                                                                       |
|   4 |  0x10 | Echo           | All received characters are echoed back                                                                               |
|   5 |  0x20 | Cansend Format | If set, cansend format is used instead of default SLCAN                                                               |
|   6 |  0x40 | Not used       |                                                                                                                       |
|   7 |  0x80 | Not used       |                                                                                                                       |

When error details are turned on, every `BEL` will be preceeded by an additional
character describing error in more details. The following reasons are currently
defined:

| Response | Reason                                                            |
|----------|-------------------------------------------------------------------|
| a!`BEL`   | Invalid access (e.g. writing in listen-only mode)                |
| e!`BEL`   | Generic error (e.g. cannot transmit at this time)                |
| p!`BEL`   | Value given to parameter is not valid (e.g. unknown speed)       |
| x!`BEL`   | Command not supported                                            |


###### Extended Flags (*F) ######

Reports the extended flags for the device:

|                 | Send            | Receive         | Notes                                                  |
|-----------------|-----------------|-----------------|--------------------------------------------------------|
| Syntax          | ?*S`CR`         | *S{2:state}`CR` | Returns the current state                              |
| Example (error) | ?*SFF`CR`       | `BEL`           | Unsupported flags (x!)                                 |

Following are bits and their meanings:

| Bit | Value | Status         | Description                                                            |
|-----|-------|----------------|------------------------------------------------------------------------|
| 0-1 |  0-3  | State          | Returns channel state (00: closed, 01: loopback, 10: listen, 11: open) |
|   2 | 0x04  | Auto Pooling   | Returns if auto-pooling is enabled                                     |
|   3 | 0x08  | Any Errors     | Returns if any errors are present                                      |
|   4 | 0x10  | Power          | Returns if power is turned on                                          |
|   5 | 0x20  | Termination    | Returns if termination is turned on                                    |
|   6 | 0x40  | Any Load       | Returns if artificial load is being produced by the node               |
|   7 | 0x80  | Any Debug      | Returns if any debug feature is on                                     |


###### Load bus (*L) ######

Device will produce artifical load on bus.

|                 | Send            | Receive         | Notes                            |
|-----------------|-----------------|-----------------|----------------------------------|
| Syntax (set)    | *L{1:level}`CR` | `CR`            | Set the load level (0:off)       |
| Syntax (query)  | *L`CR`          | *P{1:state}`CR` | Returns the current load level   |
| Example         | *L0`CR`         | `CR`            | Turns load off                   |
| Example         | *L9`CR`         | `CR`            | Maximum load                     |
| Example (error) | *LA`CR`         | `BEL`           | Invalid value (p!)               |

When it comes to load levels for this command, 1 is the minimum level and 9 is
the maximum. However, these numbers do not correspond to any particular TPS but
rather to a relative values. General recommendation is to have each level
produce double the amount previous level did; e.g. if level 1 corresponds to 1
TPS, then level 2 will be 2 TPS, level 3 will be 4 TPS and so on. All parameters
used for these messages will be produced randomly, including ID, length, and
data.


###### Power CAN bus (*P) ######

Powers CAN bus from the device itself. Works only on Cananka Mini.

|                 | Send            | Receive         | Notes                            |
|-----------------|-----------------|-----------------|----------------------------------|
| Syntax (set)    | *P{1:state}`CR` | `CR`            | Turns V+ on (1) or off (0)       |
| Syntax (query)  | *P`CR`          | *P{1:state}`CR` | Returns the current state        |
| Example         | *P0`CR`         | `CR`            | Turns V+ off                     |
| Example (error) | *P2`CR`         | `BEL`           | Invalid value (p!)               |
| Example (error) | *P1`CR`         | `BEL`           | Unsupported device (x!)          |


###### Reset device (*R) ######

Resets the device.

|                 | Send    | Receive         | Notes                            |
|-----------------|---------|-----------------|----------------------------------|
| Syntax          | *R`CR`  |                 | Reset                            |
| Example (error) | *R1`CR` | `BEL`           | Invalid parameters (p!)          |


###### Reset settings (*r) ######

Resets all settings to their default value. Used to bring device to power on
state.

|                 | Send    | Receive         | Notes                            |
|-----------------|---------|-----------------|----------------------------------|
| Syntax          | *r`CR`  |                 | Reset all settings               |
| Example (error) | *r1`CR` | `BEL`           | Invalid parameters (p!)          |


###### Terminate CAN bus (*T) ######

Turns on termination resistors on CAN bus. Works only on Cananka Mini.

|                 | Send            | Receive         | Notes                               |
|-----------------|-----------------|-----------------|-------------------------------------|
| Syntax (set)    | *T{1:state}`CR` | `CR`            | Turns termination on (1) or off (0) |
| Syntax (query)  | *T`CR`          | *T{1:state}`CR` | Returns the current state           |
| Example         | *T0`CR`         | `CR`            | Turns termination off               |
| Example (error) | *T2`CR`         | `BEL`           | Invalid value (p!)                  |
| Example (error) | *T1`CR`         | `BEL`           | Unsupported device (x!)             |


##### Data format #####

SLCAN format:

| Type                   | Format                          | Example            | Notes                              |
|------------------------|---------------------------------|--------------------|------------------------------------|
| Standard frame         | t{3:id}{1:length}{0-8:data}`CR` | t12324142`CR`      | ID=0x123 Length=2 Data=0x4142      |
| Extended frame         | T{8:id}{1:length}{0-8:data}`CR` | T1234567824142`CR` | ID=0x12345678 Length=2 Data=0x4142 |
| Standard remote frame  | r{3:id}{1:length}`CR`           | r1230`CR`          | ID=0x123 Length=0                  |
| Extended remote frame  | R{8:id}{1:length}`CR`           | R123456782`CR`     | ID=0x12345678 Length=2             |

Cansend format:

| Type                   | Format                          | Example            | Notes                              |
|------------------------|---------------------------------|--------------------|------------------------------------|
| Standard frame         | {3:id}#{0-8:data}`CR`           | 123#4142`CR`       | ID=0x123 Length=2 Data=0x4142      |
| Extended frame         | {8:id}#{0-8:data}`CR`           | 12345678#4142`CR`  | ID=0x12345678 Length=2 Data=0x4142 |
| Standard remote frame  | {3:id}#R{0-1:length}`CR`        | 123#R`CR`          | ID=0x123 Length=0                  |
| Extended remote frame  | {8:id}#R{0-1:length}`CR`        | 12345678#R2`CR`    | ID=0x12345678 Length=2             |

---

*You can check my blog and other projects at [www.medo64.com](https://www.medo64.com/electronics/)*
