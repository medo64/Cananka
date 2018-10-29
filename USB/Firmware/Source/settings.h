#ifndef SETTINGS_H
#define	SETTINGS_H

#include <stdbool.h>
#include <stdint.h>


/** Retrieves USART baud rate from EEPROM **/
uint32_t settings_getUsartBaudRate(void);

/** Writes USART baud rate to EEPROM **/
void settings_setUsartBaudRate(uint32_t speed);


/** Retrieves CAN bus configuration from EEPROM. Returns false if configuration is not set. **/
bool settings_getCanBusConfig(uint8_t* brp, uint8_t* prseg, uint8_t* seg1ph, uint8_t* seg2ph, uint8_t* sjw, bool* sampleThree, uint8_t* autoOpenIndex);

/** Writes CAN bus configuration to EEPROM. **/
void settings_setCanBusConfig(uint8_t brp, uint8_t prseg, uint8_t seg1ph, uint8_t seg2ph, uint8_t sjw, bool sampleThree, uint8_t autoOpenIndex);


#endif	/* SETTINGS_H */
