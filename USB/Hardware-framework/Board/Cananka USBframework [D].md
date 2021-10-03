### Cananka USB/framework ###

Board size: 26.0 x 30.0 mm ~ 1.2 sqin


#### Input ####

Voltage: 5 V ±10%
Current: 500 mA


#### Parts ####

|  # | Part                                      | RefDes  | Preferred Part Number      | Alternate Part Number           |
|---:|-------------------------------------------|---------|----------------------------|---------------------------------|
|  3 | C 100nF X7R 16V (0805)                    | C1-C3   | 478-5311-1-ND              |                                 |
|  1 | C 330nF X7R 16V (0805)                    | C4      | 732-7669-1-ND              |                                 |
|  2 | C 10uF X5R 16V (0805)                     | C5-C6   | 1276-1096-1-ND             |                                 |
|  1 | D TVS 150W Zenner CAN (SOT23-3)           | D1      | 497-13262-1-ND             | 568-4032-1-ND                   |
|  2 | DS LED (0805)                             | DS1-DS2 | 475-1415-1-ND              |                                 |
|  1 | L Ferrite 40Ohm (0805)                    | L1      | 445-2201-1-ND              |                                 |
|  1 | P USB C, plug, straddle 0.8mm             | P1      | WM12855-ND                 |                                 |
|  1 | Q P-MOSFET 20V LowVgs {DMG2305UX}         | Q1      | DMG2305UX-13DICT-ND        |                                 |
|  4 | R 120 0.125W (0805)                       | R1-R4   | 311-120CRCT-ND             |                                 |
|  3 | R 1K 0.125W (0805)                        | R5-R7   | RMCF0805FT1K00CT-ND        | 311-1.00KCRCT-ND                |
|  2 | R 5.1K 0.125W (0805)                      | R8-R9   | RMCF0805FT5K10CT-ND        |                                 |
|  1 | U MCP2221A (TSSOP-14)                     | U1      | MCP2221A-I/ST-ND           |                                 |
|  1 | U PIC18F25K80 (SSOP-28)                   | U2      | PIC18F25K80-I/SS-ND        |                                 |
|  1 | U MCP2561 (SOIC-8)                        | U3      | MCP2561-E/SN-ND            | 568-10289-1-ND                  |
|  1 | P ZEC 1,0/ 4-ST-3,5 C1 R1,4               | -       | 277-14304-ND               |                                 |


WARNING: This is a non-isolated board - beware of ground loops and all the fun
that can happen with different ground potentials.

NOTE: Due to type-C connector, bottom board has to be 0.8mm. Tongue board needs
to be 1.6mm due to Phoenix ZEC connector. One connect to other using pins.


#### UART Configuration ####

In order to configure the device, one has to use MCP2221 Utility application
(download from Microchip). In configuration, one MUST perform the following
changes:

    * GP1: 1 (CLK_OUT)

The following changes are suggested:

    * Required Current: 500 mA
    * Descriptor: Cananka
    * Manufacturer: Josip Medved
    * Enumerate with serial number: Yes

All other settings should remain default.
