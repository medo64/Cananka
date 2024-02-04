#include <xc.h>
#include <stdbool.h>
#include <stdint.h>
#include "device.h"


DEVICE_TYPE cachedType = DEVICE_UNKNOWN;
bool cachedSupportsPower;
bool cachedSupportsTermination;
bool cachedSupportsUsbStatus;
bool cachedNeedsClockOut;
bool cachedSupports230K;
bool cachedSupports460K;
bool cachedSupports920K;
uint8_t cachedRevision;


void device_init() {
    // | A3 | A5 | B0 | B1 | B4 | Type          | Revision  | PIC Clock | UART Clock | Termination | Power Supply |
    // |----|----|----|----|----|---------------|-----------|-----------|------------|-------------|--------------|
    // | X  | X  | L  | H  | H  | Standard      | 3 (A)     | Crystal   | PIC        | No          | No           |
    // | L  | L  | L  | L  | H  | Mini          | 4 (D)     | MCP2221A  | MCP2221A   | Yes         | Yes          |
    // | L  | L  | H  | L  | H  | Framework     | 4 (D)     | MCP2221A  | MCP2221A   | Yes         | No           |
    // | L  | L  | H  | H  | H  | Mini          | 1 (A B C) | MCP2221   | MCP2221    | Yes         | Yes          |
    // | L  | H  | L  | L  | H  | Standard      | 4 (D)     | Crystal   | MCP2221A   | No          | No           |
    // | L  | H  | L  | H  | H  | Standard      | 3 (C)     | Crystal   | PIC        | No          | No           |
    // | L  | H  | H  | H  | H  | Standard      | 2 (B)     | Crystal   | PIC        | No          | No           |
    // | H  | H  | L  | L  | H  | Standard/RJ45 | 4 (D)     | Crystal   | MCP2221A   | No          | No           |
    // | H  | H  | L  | H  | H  | Standard/RJ45 | 3 (C)     | Crystal   | PIC        | No          | No           |
    // | H  | H  | H  | H  | H  | Standard/RJ45 | 2 (B)     | Crystal   | PIC        | No          | No           |
    // | L  | H  | L  | L  | L  | Standard      | 5 (E)     | Crystal   | MCP2221A   | No          | No           |
    // | L  | L  | L  | L  | L  | Mini          | 5 (E)     | MCP2221A  | MCP2221A   | Yes         | No           |
    // | H  | L  | L  | L  | L  | Framework     | 5 (E)     | MCP2221A  | MCP2221A   | Yes         | No           |

    nRBPU = 0; //enable port B pull-ups
    WPUB0 = 1; WPUB1 = 1; WPUB4 = 1;
    TRISA3 = 1; TRISA5 = 1; TRISB0 = 1;  TRISB1 = 1; TRISB4 = 1;
    ANSEL3 = 0; ANSEL4 = 0; ANSEL10 = 0; ANSEL8 = 0; ANSEL9 = 0;

    unsigned bitA5 = PORTAbits.RA5; //Device Type (1)
    unsigned bitA3 = PORTAbits.RA3; //Device Type (2)
    unsigned bitB0 = PORTBbits.RB0; //Aux (1)
    unsigned bitB1 = PORTBbits.RB1; //Aux (2)
    unsigned bitB4 = PORTBbits.RB4; //Aux (3)

    if (!bitB0 && bitB1 && bitB4) {  // Standard [A C]
        cachedType = DEVICE_CANANKA_STANDARD;
        cachedRevision = 3;
        cachedSupportsPower = false;
        cachedSupportsTermination = false;
        cachedSupportsUsbStatus = false;
        cachedNeedsClockOut = true;
        cachedSupports230K = true;
        cachedSupports460K = true;
        cachedSupports920K = true;
    } else if (!bitA3 && bitA5) {  // Standard
        cachedType = DEVICE_CANANKA_STANDARD;
        cachedSupportsPower = false;
        cachedSupportsTermination = false;
        if (bitB0 && bitB1 && bitB4) {  // Standard [B]
            cachedRevision = 2;
            cachedSupportsUsbStatus = false;
            cachedNeedsClockOut = true;
            cachedSupports230K = true;
            cachedSupports460K = true;
            cachedSupports920K = true;
        } else if (!bitB0 && !bitB1 && bitB4) {  // Standard [D]
            cachedRevision = 4;
            cachedSupportsUsbStatus = true;
            cachedNeedsClockOut = false;
            cachedSupports230K = true;
            cachedSupports460K = true;
            cachedSupports920K = false;
        } else {
            cachedRevision = 0;
            cachedSupportsUsbStatus = false;
            cachedNeedsClockOut = false;
            cachedSupports230K = true;
            cachedSupports460K = true;
            cachedSupports920K = false;
        }
    } else if (bitA3 && bitA5) {  // Standard (RJ-45) - obsoleted
        cachedType = DEVICE_CANANKA_STANDARD;
        cachedSupportsPower = false;
        cachedSupportsTermination = false;
        if (bitB0 && bitB1 && bitB4) {  // Standard (RJ-45) [B]
            cachedRevision = 2;
            cachedSupportsUsbStatus = false;
            cachedNeedsClockOut = true;
            cachedSupports230K = true;
            cachedSupports460K = true;
            cachedSupports920K = true;
        } else if (!bitB0 && !bitB1 && bitB4) {  // Standard (RJ-45) [D]
            cachedRevision = 4;
            cachedSupportsUsbStatus = true;
            cachedNeedsClockOut = false;
            cachedSupports230K = true;
            cachedSupports460K = true;
            cachedSupports920K = false;
        } else {
            cachedRevision = 0;
            cachedSupportsUsbStatus = false;
            cachedNeedsClockOut = false;
            cachedSupports230K = true;
            cachedSupports460K = true;
            cachedSupports920K = false;
        }
    } else if (!bitA3 && !bitA5) {
        cachedType = DEVICE_CANANKA_MINI;
        cachedSupportsPower = true;
        cachedSupportsTermination = true;
        cachedNeedsClockOut = false;
        if (bitB0 && bitB1 && bitB4) {  // Mini [B C]
            cachedRevision = 1;
            cachedSupportsUsbStatus = false;
            cachedSupports230K = false;
            cachedSupports460K = false;
            cachedSupports920K = false;
        } else if (!bitB0 && !bitB1 && bitB4) {  // Mini [D]
            cachedRevision = 4;
            cachedSupportsUsbStatus = false;
            cachedSupports230K = true;
            cachedSupports460K = true;
            cachedSupports920K = false;
        } else if (bitB0 && !bitB1 && bitB4) {  // Framework [D]
            cachedRevision = 4;
            cachedSupportsPower = false;
            cachedSupportsUsbStatus = false;
            cachedSupports230K = true;
            cachedSupports460K = true;
            cachedSupports920K = false;
        } else if (!bitB0 && !bitB1 && bitB4) {  // Standard (RJ-45) [D]
            cachedRevision = 4;
            cachedSupportsUsbStatus = true;
            cachedSupports230K = true;
            cachedSupports460K = true;
            cachedSupports920K = false;
        } else {
            cachedRevision = 0;
            cachedSupportsUsbStatus = false;
            cachedSupports230K = false;
            cachedSupports460K = false;
            cachedSupports920K = false;
        }
    } else {
        cachedType = DEVICE_CANANKA_STANDARD;
        cachedRevision = 0;
        cachedSupportsPower = false;
        cachedSupportsTermination = false;
        cachedSupportsUsbStatus = false;
        cachedNeedsClockOut = false;
        cachedSupports230K = false;
        cachedSupports460K = false;
        cachedSupports920K = false;
    }
}


DEVICE_TYPE device_getType() {
    if (cachedType == DEVICE_UNKNOWN) { device_init(); }
    return cachedType;
}

uint8_t device_getMajor() {
    if (cachedType == DEVICE_UNKNOWN) { device_init(); }
    return cachedRevision;
}

uint8_t device_getMinor() {
    if (cachedType == DEVICE_UNKNOWN) { device_init(); }
    switch (device_getType()) {
        case DEVICE_CANANKA_STANDARD: return 0;
        case DEVICE_CANANKA_MINI: return 2;
        default: return 9;
    }
}

bool device_supportsPower() {
    if (cachedType == DEVICE_UNKNOWN) { device_init(); }
    return cachedSupportsPower;
}

bool device_supportsTermination() {
    if (cachedType == DEVICE_UNKNOWN) { device_init(); }
    return cachedSupportsTermination;
}

bool device_needsClockOut() {
    if (cachedType == DEVICE_UNKNOWN) { device_init(); }
    return cachedNeedsClockOut;
}

bool device_supports230K() {
    if (cachedType == DEVICE_UNKNOWN) { device_init(); }
    return cachedSupports230K;
}

bool device_supports460K() {
    if (cachedType == DEVICE_UNKNOWN) { device_init(); }
    return cachedSupports460K;
}

bool device_supports920K() {
    if (cachedType == DEVICE_UNKNOWN) { device_init(); }
    return cachedSupports920K;
}
