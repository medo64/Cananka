### Cananka /Framework Parts

|  # | Part                                      | RefDes  | Preferred Part Number      | Alternate Part Number           |
|---:|-------------------------------------------|---------|----------------------------|---------------------------------|
|  3 | C 100nF X7R 16V (0805)                    | C1-C3   | 478-5311-1-ND              |                                 |
|  1 | C 330nF X7R 16V (0805)                    | C4      | 732-7669-1-ND              |                                 |
|  1 | C 10uF X5R 16V (0805)                     | C5      | 1276-1096-1-ND             |                                 |
|  1 | D TVS 150W Zenner CAN (SOT23-3)           | D1      | 497-13262-1-ND             | 568-4032-1-ND                   |
|  1 | DS LED (0805)                             | DS1     | 475-1415-1-ND              |                                 |
|  1 | J JST PH Vertical (2.0mm)                 | J1      | 455-1706-ND                |                                 |
|  1 | L Ferrite 40Ohm (0805)                    | L1      | 445-2201-1-ND              |                                 |
|  1 | P USB C, plug, straddle 0.8mm             | P1      | WM12855-ND                 |                                 |
|  2 | Q P-MOSFET 20V LowVgs {DMG2305UX}         | Q1-Q2   | DMG2305UX-13DICT-ND        | 785-AOSS21319CCT-ND             |
|  1 | R 118 0.125W (0805)                       | R1      | P118CCT-ND                 |                                 |
|  5 | R 5.1K 0.125W (0805)                      | R2-R6   | RMCF0805FT5K10CT-ND        |                                 |
|  1 | U MCP2221A (TSSOP-14)                     | U1      | MCP2221A-I/ST-ND           |                                 |
|  1 | U PIC18F25K80 (SSOP-28)                   | U2      | PIC18F25K80-I/SS-ND        |                                 |
|  1 | U MCP2561 (SOIC-8)                        | U3      | MCP2561-E/SN-ND            | 568-10289-1-ND                  |

WARNING: This is a non-isolated board - beware of ground loops and all the fun
that can happen with different ground potentials. Ideally avoid connecting either
power or ground unless really necessary.


#### Board Size

|       |      Dimensions | Area    | Thickness |
|-------|-----------------|---------|-----------|
| PCB   |  24.0 x 27.9 mm | 1.1 in² |    0.8 mm |


#### Power

| Property | Value  |
|----------|-------:|
| Voltage  |    5 V |
| Current  | 100 mA |


#### UART Configuration

In order to configure the device, one has to use MCP2221 Utility application
(download from [Microchip](https://ww1.microchip.com/downloads/en/DeviceDoc/MCP2221Utility.zip)).
In configuration, one MUST perform the following changes:

    * GP1: 1 (CLK_OUT)

The following changes are suggested:

| Property                     | Value             |
|------------------------------|-------------------|
| VID:                         | 0x04D8            |
| PID:                         | 0xE866            |
| Required Current:            | 100 mA (minimum)  |
| Descriptor:                  | Cananka Framework |
| Manufacturer:                | Medo64            |
| Enumerate with serial number | Yes               |
| GP1                          | CLK_OUT           |

All other settings should remain default.


### Wiring

To connect to this board, one has to use 5-pin JST PH connector. The following
table represents the pinout, pin 1 being on the left as looking into the
expansion card.

While power does have its pin in connector, it's not internally connected to
anything.

Board is NOT INSULATED and thus make sure not to connect it to the CAN bus at
other potential. Alternatively, just run one or the other system off the
battery to avoid ground loops.

| # | Ref   | Color  | Purpose              |
|--:|-------|--------|----------------------|
| 1 | GND   | Black  | Ground               |
| 2 | CAN-L | Green  | CAN Low              |
| 3 | CAN-J | Yellow | CAN High             |
| 4 | -     | -      | Not connected        |
| 5 | V+    | Red    | Power; not connected |

PS: Colors are just a suggestion and it's not necessary to have them match the
table.

PPS: As power consumption for signal lines is negligible, wires can be almost
any AWG. I would recommend using 24 AWG wire 0.25 mm² as they allow for a
greater compatibility with other connectors.
