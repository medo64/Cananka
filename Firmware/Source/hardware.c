#include <p18cxxx.h>

#include "device.h"
#include "hardware.h"
#include "random.h"


void init(void) {
    //disable interrupts
    GIE = 0;

    random_init();

    //wait for PLL lock
    PLLEN = 1;
    while (!OSCCONbits.OSTS);
    __delay_ms(250);

    REFOCONbits.RODIV3 = 0;
    REFOCONbits.RODIV2 = 0;
    REFOCONbits.RODIV1 = 0;
    REFOCONbits.RODIV0 = 0;
    REFOCONbits.ROSEL = 1;
    REFOCONbits.ROSSLP = 1;
    TRISC3 = 0;

    //versioning
    WPUB = 0; //disable all pull-ups
    device_init();

    //io
    TRISC5 = 0; //O LED
    TRISC4 = 0; //O Termination
    TRISB5 = 0; //O Power

    //clear all outputs
    LATA = 0b00000000;
    LATB = 0b00000000;
    LATC = 0b00000000;
}

void reset(void) {
    Reset();
}

void wait_short(void) {
    __delay_ms(150);
}

void activate_clockOut() {
    LC3 = 1;
    REFOCONbits.ROON = 1;
}
