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

The following hardware version are also available:
* [RJ45](README-RJ45.md) (uses RJ-45 connector)
* [Mini](https://github.com/medo64/CanankaMini) (USB A connector, non-isolated, and without a case)
* [Framework Expansion Card](https://github.com/medo64/CanankaFEC) (non-isolated, in framework expansion card format)
* [HAT](https://github.com/medo64/CanankaHAT) (intended for Raspberry Pi)

The following additional repositories might be of interest:
* [Firmware](https://github.com/medo64/Cananka.Firmware)
* [Test software](https://github.com/medo64/Cananka.Software)


---
*You can check my blog and other projects at [www.medo64.com](https://medo64.com/electronics/).*
