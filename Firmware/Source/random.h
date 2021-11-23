#ifndef RANDOM_H
#define	RANDOM_H

#include <stdint.h>


/** Initializes RND. After this is called, do something that takes random amount of time (e.g. EEPROM write or PLL startup). */
void random_init(void);

/** Get next random byte. */
uint8_t random_getByte(void);


#endif	/* RANDOM_H */

