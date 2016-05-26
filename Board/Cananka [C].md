### Cananka ###

|-------------------------|---------------------------|
| Board size              | 65.0 x 56.0 mm ~ 5.7 sqin |
| Current used (3.3 V)    | 15 mA                     |
| Current used (5 V)      | 110 mA                    |
| Current used (12 V CAN) | 2.5 A (maximum)           |
| Current provided (5 V)  | 2 A (maximum)             |
|-------------------------|---------------------------|


#### Parts ####

|  # | Part                             | RefDes  | Digi-Key Part Number       | Substitute Part Number          |
|---:|----------------------------------|---------|----------------------------|---------------------------------|
|  2 | C 22pF 16V NP0 (0603)            | C1 C2   | 478-1167-1-ND              |                                 |
|  4 | C 100nF X7R (0805)               | C3-C6   | 399-1170-1-ND              |                                 |
|  2 | C 10uF 35V X5R (0805)            | C7-C8   | 490-10515-1-ND             |                                 |
|  3 | C 10uF 35V X5R (0805) *          | C9-C11  | 490-10515-1-ND             |                                 |
|  1 | D PESD1CAN-UX (SOT23-3)          | D1      | 568-4032-1-ND              |                                 |
|  1 | DS LED (0805)                    | DS1     | 475-1415-1-ND              |                                 |
|  5 | DS LED (3mm)                     | DS2-DS6 | 754-1218-ND                |                                 |
|  1 | F 2A (1206) *                    | F1      | 507-1881-1-ND              |                                 |
|  1 | J Header (40w)                   | J1      | S6104-ND                   |                                 |
|  1 | J MCV 1,5/ 4-G-3,81 (4w)         | J2      | 277-1223-ND                | ED2821-ND                       |
|  1 | Q DMG2305UX-13 (SOT23-3) *       | Q1      | DMG2305UX-13DICT-ND        |                                 |
|  1 | Q DMMT5401-7-F (SOT23-6) *       | Q2      | DMMT5401-FDICT-ND          |                                 |
|  2 | R 120 0.5W (1206) †              | R1 R2   | P120ALCT-ND                |                                 |
|  3 | R 220 0.125W (0805)              | R3-R5   | RMCF0805FT220RCT-ND        |                                 |
|  4 | R 1K 0.125W (0805)               | R6-R9   | RMCF0805FT1K00CT-ND        |                                 |
|  2 | R 3.9K 0.125W (0805)             | R10 R11 | RMCF0805FT3K90CT-ND        |                                 |
|  1 | R 10K 0.125W (0805) *            | R12     | RMCF0805FT10K0CT-ND        |                                 |
|  1 | R 47K 0.125W (0805) *            | R13     | RMCF0805FT47K0CT-ND        |                                 |
|  1 | U MCP2515 (SOIC-18)              | U1      | MCP2515T-I/SOCT-ND         |                                 |
|  1 | U ISO1050 (SOP-8)                | U2      | 296-24818-1-ND             |                                 |
|  1 | U CAT24C32WI-GT3 (SOIC-8)        | U3      | CAT24C32WI-GT3CT-ND        |                                 |
|  1 | VR DC-DC 5V->5V 1W (SIP-4)       | VR1     | 945-1655-5-ND              | 1470-1397-5-ND                  |
|  1 | VR DC-DC 12V->5V 10W (1x1") *    | VR2     | 941-1586-ND                | 454-1712-ND                     |
|  1 | Y Crystal 16MHz 50ppm 20pF       | Y1      | 887-2015-ND                | 887-1244-ND                     |
|  1 | P MC 1,5/ 4-ST-3,81              |         | 277-1163-ND                | ED2877-ND                       |

* Optional power-back component
† Optional termination resistor


#### CAN bus termination ####

It is possible to use on-board termination resistors to permanently terminate
one side of CAN bus. Board allows up to two resistors for increased power.


#### Power-back components ####

To power Raspberry from CAN, all power-back components have to be populated. If
later we want to turn it off, removal of fuse resistor (on top) will suffice.
