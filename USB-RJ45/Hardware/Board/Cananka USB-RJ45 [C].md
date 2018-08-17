### Cananka USB (RJ-45) ###

Board size: 105.8 x 26.0 mm ~ 4.3 sqin
Panel size: 29.5 x 20.3 mm ~ 1.0 sqin (2x)


#### Input ####

Voltage: 5 V ±10%
Current: 100 mA


#### Parts ####

|  # | Part                                      | RefDes  | Digi-Key Part Number       | Alternate Part Number      |
|---:|-------------------------------------------|---------|----------------------------|----------------------------|
|  2 | C 22pF 16V NP0 (0603)                     | C1 C2   | 478-1167-1-ND              |                            |
|  4 | C 100nF 35V X7R (0805)                    | C3-C6   | 399-1170-1-ND              |                            |
|  1 | C 470nF /16V X7R (0805)                   | C7      | 1276-1039-1-ND             |                            |
|  3 | C 10uF 16V X5R (0805)                     | C8-C10  | 399-8013-1-ND              |                            |
|  1 | D PESD1CAN-UX (SOT23-3)                   | D1      | 568-4032-1-ND              |                            |
|  1 | DS LED RGB CA (1204)                      | DS1     | 160-2234-1-ND              |                            |
|  4 | DS LED (0805)                             | DS2-DS5 | 475-1415-1-ND              |                            |
|  1 | J USB B, horizontal                       | J1      | ED2982-ND                  |                            |
|  1 | J RJ-45 (56) LED                          | J2      | RJHSE-5381-ND              |                            |
|  1 | L Ferrite 40Ohm (0805)                    | L1      | 445-2201-1-ND              |                            |
|  4 | R 120 0.125W (0805)                       | R1-R4   | 311-120CRDKR-ND            |                            |
|  4 | R 330 0.125W (0805)                       | R5-R8   | RMCF0805FT330RCT-ND        |                            |
|  7 | R 1K 0.125W (0805)                        | R9-R15  | RMCF0805FT1K00CT-ND        |                            |
|  1 | U MCP2221A (TSSOP-14)                     | U1      | MCP2221A-I/ST-ND           |                            |
|  1 | U PIC18F25K80 (SSOP-28)                   | U2      | PIC18F25K80-I/SS-ND        |                            |
|  1 | U ISO1050 (SOP-8)                         | U3      | 296-24818-1-ND             |                            |
|  1 | VR DC-DC 5V->5V 1W (SIP-4)                | VR1     | 945-1655-5-ND              |                            |
|  1 | Y Crystal 12MHz 50ppm 20pF                | Y1      | 887-2011-ND                | 887-1238-ND                |
|  1 | H Enclosure 1593DBK                       |         | HM861-ND                   |                            |
|  4 | H PCB Screw Self-tapping #4 1/4" (M3x6mm) |         | SR6004-ND                  |                            |
|  1 | P MC 1,5/ 4-ST-3,81                       |         | 277-1163-ND                | ED2877-ND                  |



#### UART Configuration ####

In order to configure the device, one has to use MCP2221 Utility application
(download from Microchip). Device works with factory configuration but the
following changes are suggested:

    * Descriptor: Cananka
    * Manufacturer: Josip Medved
    * Enumerate with serial number: Yes

All other settings should remain default.
