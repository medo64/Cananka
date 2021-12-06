### Cananka Parts ###

|  # | Part                                      | RefDes  | Preferred Part Number      | Alternate Part Number           |
|---:|-------------------------------------------|---------|----------------------------|---------------------------------|
|  2 | C 22pF NP0 16V (0805)                     | C1-C2   | 399-8036-1-ND              |                                 |
|  4 | C 100nF X7R 16V (0805)                    | C3-C6   | 478-5311-1-ND              |                                 |
|  1 | C 330nF X7R 16V (0805)                    | C7      | 732-7669-1-ND              |                                 |
|  3 | C 10uF X5R 16V (0805)                     | C8-C10  | 1276-1096-1-ND             |                                 |
|  1 | D TVS 150W Zenner CAN (SOT23-3)           | D1      | 497-13262-1-ND             | 568-4032-1-ND                   |
|  6 | DS LED (0805)                             | DS1-DS6 | 475-1415-1-ND              |                                 |
|  1 | J USB C 2.0, horizontal                   | J1      | 2073-USB4085-GF-ACT-ND     |                                 |
|  1 | J MC 1,5/ 4-G-3,81 (4w)                   | J2      | ED2810-ND                  | 277-1208-ND                     |
|  1 | L Ferrite 40Ohm (0805)                    | L1      | 445-2201-1-ND              |                                 |
|  1 | Q P-MOSFET 20V LowVgs {DMP3085LSD} (SO-8) | Q1      | DMP3085LSD-13DICT-ND       | 785-AOSD21311CCT-ND             |
|  4 | R 118 0.125W (0805)                       | R1-R4   | P118CCT-ND                 |                                 |
|  9 | R 470 0.125W (0805)                       | R5-R13  | RMCF0805FT470RCT-ND        |                                 |
|  3 | R 5.1K 0.125W (0805)                      | R14-R16 | RMCF0805FT5K10CT-ND        |                                 |
|  1 | U MCP2221A (TSSOP-14)                     | U1      | MCP2221A-I/ST-ND           |                                 |
|  1 | U PIC18F25K80 (SSOP-28)                   | U2      | PIC18F25K80-I/SS-ND        |                                 |
|  1 | U ISO1050 (SOP-8)                         | U3      | 296-24818-1-ND             |                                 |
|  1 | U Optocoupler 817S (SMD-4)                | U4      | 160-1367-5-ND              | 732-140817143200CT-ND           |
|  1 | VR DC-DC 5V->5V 1W (SIP-4)                | VR1     | 945-1655-5-ND              |                                 |
|  1 | Y Crystal 12MHz 50ppm 20pF                | Y1      | 887-2011-ND                | 887-1238-ND                     |
|  1 | H Enclosure 1593DBK                       |         | HM861-ND                   |                                 |
|  4 | H PCB Screw Self-tapping M3 6mm (#4 1/4") |         | 36-9191-3-ND               | SR6004-ND                       |
|  2 | P MC 1,5/ 4-ST-3,81 (4w)                  |         | ED2877-ND                  | 277-1163-ND                     |

Please note pretty much any of these components can be replaced with a carefully
chosen replacement.


#### UART Configuration ####

In order to configure the device, one has to use MCP2221 Utility application
(download from Microchip). Device works with factory configuration but the
following changes are suggested:

| Property                     | Value            |
|------------------------------|------------------|
| VID:                         | 0x04D8           |
| PID:                         | 0xE866           |
| Required Current:            | 100 mA (minimum) |
| Descriptor:                  | Cananka          |
| Manufacturer:                | Medo64           |
| Enumerate with serial number | Yes              |

All other settings should remain default.


#### Board Size ####

|       |      Dimensions | Area    | Thickness |
|-------|-----------------|---------|-----------|
| PCB   | 105.0 x 26.0 mm | 4.3 in² |    0.8 mm |
| Panel |  29.5 x 20.3 mm | 1.0 in² |    1.6 mm |
