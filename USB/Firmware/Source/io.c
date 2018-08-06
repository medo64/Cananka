#include <p18cxxx.h>

void io_setup() {
    LC5 = 0; //turn on LED
    LC4 = 1; //turn off termination
    LB5 = 1; //turn off V+
}
