### Cananka USB-RJ45 ###

Cananka USB-RJ45 is a small device connecting to CAN bus using USB serial port
(SLCAN compatible). More information about the protocol is available in a
[separate document](PROTOCOL.md).

It has USB type-B connector on computer side and RJ-45 connector toward CAN bus.


#### Revisions ####

| Revision | Functional | HW Supported | FW Supported | Description                                    |
|----------|------------|--------------|--------------|------------------------------------------------|
| Dxxxx    | Yes        | Yes          | Yes          | Latest revision using MCP2221A UART interface. |
| Cxxxx    | Yes        | Yes          | Yes          | Last revision to use FT232RL UART.             |
| Bxxxx    | No         | No           | Probably †   | Test revision.                                 |
| Axxxx    | No         | No           | Probably †   | Test revision.                                 |

† As these test revisions are quite close to what revision C became, firmware
code generally works just fine. However, if any issues are found, it is unlikely
they will be addressed.


#### Cananka USB ####

[Cananka USB](README.md) is essentially the same board but with Phoenix 4-pin
connector on CAN bus end.


#### Cananka USB/mini ####

[Cananka USB/mini](README-mini.md) is smaller variant directly pluggable into
USB type A connector and without CAN bus isolation.

---

*You can check my blog and other projects at [www.medo64.com](https://www.medo64.com/electronics/)*
