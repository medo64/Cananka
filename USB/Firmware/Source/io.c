#include <p18cxxx.h>

void io_init() {
    LC5 = 0; //turn on LED
    LC4 = 1; //turn off termination
    LB5 = 1; //turn off V+

    TRISC5 = 0;
    TRISC4 = 0;
    TRISB5 = 0;
}
