[Cananka](https://medo64.com/cananka/)
======================================

Cananka allows for the computer control of a CAN bus. The control of device is
possible via serial port (SLCAN compatible, Windows/Linux) or via SocketCAN
network driver (Linux). More information about the SLCAN protocol is available
in a [separate document](PROTOCOL.md).

Features:
* USB serial device supporting SLCAN interface
* Supports 1 Mbps CAN bus operation
* Fully isolated (1 kV)
* Does not require CAN bus power supply (onboard DC-to-DC converter)
* 3.81 mm strip headers CAN bus connector
* USB 2.0 interface (type B)

There are alternative versions available too:
* [Mini](README-Mini.md) (directly pluggable, non-insulated, and without a case)


### Hardware Revisions ###

| Revision | Functional | HW Supported | FW Supported | Description                                    |
|----------|------------|--------------|--------------|------------------------------------------------|
| Dxxxx    | Yes        | Yes          | Yes          | Latest revision using MCP2221A UART interface. |
| Cxxxx †  | Yes        | Yes          | Yes          | Last revision to use FT232RL UART.             |
| Bxxxx    | No         | No           | Probably ‡   | Test revision.                                 |
| Axxxx    | No         | No           | Probably ‡   | Test revision.                                 |

† Early revision D test boards might carry revision C markings (slight
versioning oversight). However, they can be visually distingushed by the
difference in UART chip (FT232RL vs MCP2221A) and a lowercase c used on the
silkscreen. In firmware they will be correctly recognized as revision D.

‡ As these test revisions are quite close to what revision C became, firmware
code generally works just fine. However, if any issues are found, it is unlikely
they will be addressed.


---
*You can check my blog and other projects at [www.medo64.com](https://medo64.com/electronics/).*
