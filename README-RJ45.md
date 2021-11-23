[Cananka RJ-45](https://medo64.com/cananka/)
============================================

Cananka RJ45 is a small device connecting to CAN bus using USB serial port
(SLCAN compatible). More information about the protocol is available in a
[separate document](PROTOCOL.md).

For version with 3.81 mm headers, check [Cananka](README.md).

Features:
* USB serial device supporting SLCAN interface
* Supports 1 Mbps CAN bus operation
* Fully isolated (1 kV)
* Does not require CAN bus power supply (onboard DC-to-DC converter)
* 3.81 mm strip headers CAN bus connector
* USB 2.0 interface (type B)


### Hardware Revisions ###

| Revision | Functional | HW Supported | FW Supported | Description                                    |
|----------|------------|--------------|--------------|------------------------------------------------|
| Dxxxx    | Yes        | Yes          | Yes          | Latest revision using MCP2221A UART interface. |
| Cxxxx    | Yes        | Yes          | Yes          | Last revision to use FT232RL UART.             |
| Bxxxx    | No         | No           | Probably †   | Test revision.                                 |
| Axxxx    | No         | No           | Probably †   | Test revision.                                 |

† As these test revisions are quite close to what revision C became, firmware
code generally works just fine. However, if any issues are found, it is unlikely
they will be addressed.


---
*You can check my blog and other projects at [www.medo64.com](https://medo64.com/cananka/rj45/)*
