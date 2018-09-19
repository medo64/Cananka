#ifndef SETTINGS_H
#define	SETTINGS_H

#include <stdint.h>


/** Retrieves USART baud rate from EEPROM **/
uint32_t settings_getUsartBaudRate(void);

/** Writes USART baud rate to EEPROM **/
void settings_setUsartBaudRate(uint32_t speed);


#endif	/* SETTINGS_H */
