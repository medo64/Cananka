#include <p18cxxx.h>
#include <stdint.h>

#include "random.h"

#define EIGHT_BIT


void random_init() {
    //setup Timer4 (used for random numbers)
    T4CONbits.T4CKPS = 0; //no prescale
    T4CONbits.T4OUTPS = 0; //no postscale
    T4CONbits.TMR4ON = 1; //start timer4
}


const uint16_t POLYNOMIALS[] = { 0x8016, 0x888E, 0x905C, 0x9883, 0xA066, 0xA725, 0xAFC6, 0xB814, 0xBF85, 0xC7EC, 0xCFCC, 0xD7C9, 0xDF97, 0xE782, 0xEF9D, 0xF7EF };
uint16_t polynomial = 0xB400;

uint16_t randomState = 0;
uint8_t randomIndex = 0;

uint8_t random_getByte() {
    if (randomIndex == 0) { //switch polynomial every 256 rounds
        randomState = (randomState ^ (TMR4 << 8 | TMR4) ^ polynomial) | 0x8000; //ensure non-zero at cost of LSB
        polynomial = POLYNOMIALS[TMR4 & 0x0F];
    }
    randomIndex++;

    unsigned bit = randomState & 0x01;
    randomState >>= 1;
    if (bit) { randomState ^= polynomial; }

    return (uint8_t)randomState;
}
