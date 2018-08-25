### Cananka USB ###

Cananka USB is a small device connecting to CAN bus using USB serial port (SLCAN
compatible). More information about the protocol is available in a [separate document](PROTOCOL.md).

It has USB type-B connector on computer side and 3.81 mm strip headers on CAN
bus side.


#### Revisions ####

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

*You can check my blog and other projects at [www.medo64.com](https://www.medo64.com/electronics/)*
