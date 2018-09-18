#ifndef HARDWARE_H
#define HARDWARE_H


#pragma config RETEN     = OFF
#pragma config INTOSCSEL = HIGH
#pragma config SOSCSEL   = HIGH
#pragma config XINST     = OFF

#pragma config FOSC      = HS1
#pragma config PLLCFG    = ON
#pragma config FCMEN     = OFF
#pragma config IESO      = OFF

#pragma config PWRTEN    = ON
#pragma config BOREN     = SBORDIS
#pragma config BORV      = 0
#pragma config BORPWR    = HIGH

#pragma config WDTEN     = SWDTDIS
#pragma config WDTPS     = 1048576

#pragma config CANMX     = PORTB
#pragma config MSSPMSK   = MSK7
#pragma config MCLRE     = OFF

#pragma config STVREN    = ON
#pragma config BBSIZ     = BB2K

#pragma config CP0       = OFF
#pragma config CP1       = OFF
#pragma config CP2       = OFF
#pragma config CP3       = OFF

#pragma config CPB       = OFF
#pragma config CPD       = OFF

#pragma config WRT0      = OFF
#pragma config WRT1      = OFF
#pragma config WRT2      = OFF
#pragma config WRT3      = OFF

#pragma config WRTC      = OFF
#pragma config WRTB      = OFF
#pragma config WRTD      = OFF

#pragma config EBTR0     = OFF
#pragma config EBTR1     = OFF
#pragma config EBTR2     = OFF
#pragma config EBTR3     = OFF

#pragma config EBTRB     = OFF


#define _XTAL_FREQ 48000000


#define interrupt_enable()   GIE = 1
#define interrupt_disable()  GIE = 0


void init(void);
void reset(void);

void wait_short(void);

void activate_clockOut(void);


#endif	/* HARDWARE_H */
