#include <pic18f25k80.h>
#include <stdint.h>

#include "device.h"


DEVICE_TYPE cachedType = DEVICE_UNKNOWN;
bool cachedSupportsPower;
bool cachedSupportsTermination;
bool cachedNeedsClockOut;
uint8_t cachedRevision;


void device_init() {
    //| A3 | A5 | B0 | B1 | Type     | Revision  | PIC Clock | UART Clock | Termination | Power Supply |
    //|----|----|----|----|----------|-----------|-----------|------------|-------------|--------------|
    //| L  | L  | H  | H  | USBmini  | 1 (B C)   | MCP2221A  | MCP2221A   | Yes         | Yes          |
    //| L  | H  | H  | H  | USB      | 1 (C)     | Crystal   | PIC        | No          | No           |
    //| L  | H  | H  | L  | USB      | 2 (D)     | Crystal   | MCP2221A   | No          | No           |
    //| H  | H  | H  | H  | USB-RJ45 | 1 (C)     | Crystal   | PIC        | No          | No           |
    //| H  | H  | H  | L  | USB-RJ45 | 2 (D)     | Crystal   | MCP2221A   | No          | No           |

    RBPU = 0; //enable port B pull-ups
    TRISA3 = 0; TRISA5 = 0; TRISB1 = 0; TRISB0 = 0;
    ANSEL3 = 0; ANSEL4 = 0; ANSEL8 = 0; ANSEL10 = 0;

    unsigned bitA5 = PORTAbits.RA5;
    unsigned bitA3 = PORTAbits.RA3;
    unsigned bitB1 = PORTBbits.RB1;

    if (!bitA3 && bitA5) {
        cachedType = DEVICE_CANANKA_USB;
        cachedRevision = !bitB1 ? 2 : 1;
        cachedSupportsPower = false;
        cachedSupportsTermination = false;
        cachedNeedsClockOut = (cachedRevision > 1);
    } else if (bitA3 && bitA5) {
        cachedType = DEVICE_CANANKA_USB_RJ45;
        cachedRevision = !bitB1 ? 2 : 1;
        cachedSupportsPower = false;
        cachedSupportsTermination = false;
        cachedNeedsClockOut = (cachedRevision > 1);
    } else if (!bitA3 && !bitA5) {
        cachedType = DEVICE_CANANKA_USB_MINI;
        cachedRevision = 1;
        cachedSupportsPower = true;
        cachedSupportsTermination = true;
        cachedNeedsClockOut = false;
    } else {
        cachedType = DEVICE_CANANKA_USB;
        cachedRevision = 0;
        cachedSupportsPower = false;
        cachedSupportsTermination = false;
        cachedNeedsClockOut = false;
    }
}


DEVICE_TYPE device_getType() {
    if (cachedType == DEVICE_UNKNOWN) { device_init(); }
    return cachedType;
}

uint8_t device_getRevision() {
    if (cachedType == DEVICE_UNKNOWN) { device_init(); }
    return cachedRevision;
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
