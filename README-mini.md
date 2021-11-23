[Cananka Mini](https://medo64.com/cananka/)
===========================================

Cananka mini is a minimalistic device connecting to CAN bus using USB serial
port (SLCAN compatible). More information about the protocol is available in a
[separate document](PROTOCOL.md).

It has USB type-A connector on computer side and 3.81 mm strip headers on CAN
bus side. For isolated version check [Cananka](README.md).

Features:
* USB serial device supporting SLCAN interface
* Supports 125 Kbps CAN bus operation (might work at higher speeds but untested)
* Non-isolated
* Does not require CAN bus power supply
* 3.81 mm strip headers CAN bus connector
* USB 2.0 interface (type A)


### Hardware Revisions ###

| Revision | Functional | HW Supported | FW Supported | Description                                    |
|----------|------------|--------------|--------------|------------------------------------------------|
| Dxxxx    | Yes        | Yes          | Yes          | Latest revision using MCP2221A UART interface. |
| Cxxxx    | Yes        | Yes          | Yes          | First revision.                                |


---

*You can check my blog and other projects at [www.medo64.com](https://medo64.com/cananka/mini/)*
