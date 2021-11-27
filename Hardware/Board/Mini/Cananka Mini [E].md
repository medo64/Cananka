### Cananka Mini ###

Board size: 44.0 x 16.5 mm ~ 1.2 sqin


#### Input ####

Voltage: 5 V ±10%
Current: 100 mA


#### Parts ####

|  # | Part                                      | RefDes  | Preferred Part Number      | Alternate Part Number           |
|---:|-------------------------------------------|---------|----------------------------|---------------------------------|
|  2 | C 100nF X7R 16V (0805)                    | C1-C2   | 478-5311-1-ND              |                                 |
|  1 | C 330nF X7R 16V (0805)                    | C3      | 732-7669-1-ND              |                                 |
|  2 | C 10uF X5R 16V (0805)                     | C4-C5   | 1276-1096-1-ND             |                                 |
|  3 | DS LED (0805)                             | DS1-DS3 | 475-1415-1-ND              |                                 |
|  1 | J MC 1,5/ 4-G-3,81                        | J2      | ED2810-ND                  | 277-1208-ND                     |
|  1 | L Ferrite 40Ohm (0805)                    | L1      | 445-2201-1-ND              |                                 |
|  1 | P USB A, plug, horizontal                 | P1      | WM3983CT-ND                | WM17118-ND                      |
|  1 | Q P-MOSFET 20V LowVgs {DMG2305UX}         | Q1 Q1   | DMG2305UX-13DICT-ND        |                                 |
|  1 | R 120 0.125W (0805)                       | R1      | 311-120CRCT-ND             |                                 |
|  4 | R 1K 0.125W (0805)                        | R2-R5   | RMCF0805FT1K00CT-ND        | 311-1.00KCRCT-ND                |
|  1 | U MCP2221A (TSSOP-14)                     | U1      | MCP2221A-I/ST-ND           |                                 |
|  1 | U PIC18F25K80 (SSOP-28)                   | U2      | PIC18F25K80-I/SS-ND        |                                 |
|  1 | U MCP2561 (SOIC-8)                        | U3      | MCP2561-E/SN-ND            | 568-10289-1-ND                  |
|  1 | P MC 1,5/ 4-ST-3,81                       |         | ED2877-ND                  | 277-1163-ND                     |

1/2" heat-shrink tubing, 50mm

WARNING: This is a non-isolated board - beware of ground loops and all the fun
that can happen with different ground potentials.


#### UART Configuration ####

In order to configure the device, one has to use MCP2221 Utility application
(download from Microchip). In configuration, one MUST perform the following
changes:

    * GP1: 1 (CLK_OUT)

The following changes are suggested:

    * Required Current: 100 mA (minimum)
    * Descriptor: Cananka
    * Manufacturer: Medo64
    * Enumerate with serial number: Yes

All other settings should remain default.


#### JP1 ####

For testing purposes, it is possible to power other devices from 5V USB
connection. However, considering CAN bus is usually 12V based, this is
definitely not a recommendation. If one wants to have 5V output on CAN
connector, solder a SOD123FL diode at this location.
