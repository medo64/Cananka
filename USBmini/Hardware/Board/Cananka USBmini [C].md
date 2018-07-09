### Cananka USB/mini ###

53.0 x 17.0 mm ~ 1.4 sqin


#### Parts ####

|  # | Part                             | RefDes  | Digi-Key Part Number       | Alternate Part Number           |
|---:|----------------------------------|---------|----------------------------|---------------------------------|
|  3 | C 100nF, X7R (0805)              | C1-C3   | 399-1170-1-ND              |                                 |
|  1 | C 470nF /16V X7R (0805)          | C4      | 1276-1039-1-ND             |                                 |
|  2 | C 10uF /16V X5R (0805)           | C5-C6   | 399-8013-1-ND              |                                 |
|  1 | D PMEG3010BEA,115 (SOD323)       | D1      | 568-6511-1-ND              |                                 |
|  6 | DS LED (0805)                    | DS1-DS6 | 475-1415-1-ND              |                                 |
|  1 | J MCV 1,5/ 4-G-3,81              | J1      | 277-1223-ND                | ED2821-ND                       |
|  1 | L Ferrite 40Ohm (0805)           | L1      | 445-2201-1-ND              |                                 |
|  1 | P USB A, plug, horizontal        | P1      | WM17118-ND                 |                                 |
|  2 | Q P-MOSFET /10V                  | Q1 Q2   | DMP3098LDICT-ND            |                                 |
|  4 | R 120 0.125W (0805)              | R1-R4   | RHM120AECT-ND              |                                 |
|  3 | R 330 0.125W (0805)              | R5-R7   | RMCF0805FT330RCT-ND        |                                 |
|  3 | R 1K 0.125W (0805)               | R8-R10  | RMCF0805FT1K00CT-ND        |                                 |
|  3 | R 10K 0.125W (0805)              | R11-R13 | RMCF0805JT10K0CT-ND        |                                 |
|  1 | U MCP2221 (TSSOP-14)             | U1      | MCP2221-I/ST               |                                 |
|  1 | U PIC18F25K80 (SSOP-28)          | U2      | PIC18F25K80-I/SS-ND        |                                 |
|  1 | U MCP2561 (SOIC-8)               | U3      | MCP2561-E/SN-ND            | 568-10289-1-ND  MCP2562-E/SN-ND |
|  1 | P MC 1,5/ 4-ST-3,81              |         | 277-1163-ND                | ED2877-ND                       |

1/2" heat-shrink tubing, 50mm


WARNING: This is a non-isolated board - beware of ground loops and all the fun
that can happen with different ground potentials.



#### Configuration ####

In order to configure CanStick/Mini, one has to use MCP2221 Utility application
(download from Microchip). In configuration, one MUST change GP1 designation to
CLK_OUT. Optionally one can setup Descriptor ("CanStick/Mini"), Manufacturer
("Josip Medved"), and check Enumerate with serial number. Later will allow you
to have separate COM port number for each of devices.
