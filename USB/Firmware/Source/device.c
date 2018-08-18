#include <pic18f25k80.h>

#include "device.h"


DEVICE_TYPE cachedType = DEVICE_UNKNOWN;
bool cachedSupportsPower;
bool cachedSupportsTermination;
bool cachedNeedsClockOut;
uint8_t cachedRevision;


void device_initialize() {
    unsigned bitA5 = PORTAbits.RA5;
    unsigned bitA3 = PORTAbits.RA3;
    unsigned bitB1 = PORTBbits.RB1;
    //unsigned bitB4 = PORTBbits.RB4;

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
    if (cachedType == DEVICE_UNKNOWN) { device_initialize(); }
    return cachedType;
}

uint8_t device_getRevision() {
    if (cachedType == DEVICE_UNKNOWN) { device_initialize(); }
    return cachedRevision;
}

bool device_supportsPower() {
    if (cachedType == DEVICE_UNKNOWN) { device_initialize(); }
    return cachedSupportsPower;
}

bool device_supportsTermination() {
    if (cachedType == DEVICE_UNKNOWN) { device_initialize(); }
    return cachedSupportsTermination;
}

bool device_needsClockOut() {
    if (cachedType == DEVICE_UNKNOWN) { device_initialize(); }
    return cachedNeedsClockOut;
}
