#include <p18cxxx.h>

#include "hardware.h"


void init(void) {
    //disable interrupts
    GIE = 0;

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
    REFOCONbits.ROON = 1;
    TRISC3 = 0;

    //versioning
    WPUB = 0; //disable all pull-ups
    RBPU = 0; //enable port B pull-ups
    TRISA3 = 0; TRISA5 = 0; TRISB1 = 0; TRISB4 = 0;
    ANSEL3 = 0; ANSEL4 = 0; ANSEL8 = 0; ANSEL9 = 0;

    //clear all outputs
    LATA = 0b00000000;
    LATB = 0b00000000;
    LATC = 0b00001000;
}

void reset(void) {
    Reset();
}

void wait_short(void) {
    __delay_ms(150);
}
