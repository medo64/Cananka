#include <xc.h>
#include <stdint.h>

#include "settings.h"


#define EEPROM_OFFSET_USART_SPEED     0


__EEPROM_DATA(0, 0, 0, 0, 0, 0, 0, 0);


typedef union {
    struct {
        uint32_t Integer;
    };
    struct {
        uint8_t Byte0;
        uint8_t Byte1;
        uint8_t Byte2;
        uint8_t Byte3;
    };
} UINT32_UNION;


uint8_t eeprom18_read(uint16_t offset) {
    EECON1bits.EEPGD = 0; //accesses Flash program memory
    EECON1bits.CFGS = 0; //accesses Flash program or data EEPROM memory

    EEADRH = offset >> 8;
    EEADR = offset;

    EECON1bits.RD = 1; //initiates an EEPROM read
    Nop(); //it can be read after one NOP instruction

    return EEDATA;
}

void eeprom18_write(uint16_t offset, uint8_t value) {
    EECON1bits.EEPGD = 0; //accesses Flash program memory
    EECON1bits.CFGS = 0; //accesses Flash program or data EEPROM memory

    EEADRH = offset >> 8;
    EEADR = offset;

    EEDATA = value;

    EECON1bits.WREN = 1; //allows write cycles
    uint8_t oldGIE = GIE; //interrupts be disabled during this code segment

    EECON2 = 0x55; //write sequence unlock
    EECON2 = 0xAA; //write sequence unlock

    EECON1bits.WR = 1; //initiates a data EEPROM erase/write cycle
    while(EECON1bits.WR); //waits for write cycle to complete

    GIE = oldGIE; //restore interrupts
    EECON1bits.WREN = 0; //disable write
}


uint32_t settings_getUsartBaudRate() {
    UINT32_UNION value;
    value.Byte0 = eeprom18_read(EEPROM_OFFSET_USART_SPEED + 0);
    value.Byte1 = eeprom18_read(EEPROM_OFFSET_USART_SPEED + 1);
    value.Byte2 = eeprom18_read(EEPROM_OFFSET_USART_SPEED + 2);
    value.Byte3 = eeprom18_read(EEPROM_OFFSET_USART_SPEED + 3);

    return (value.Integer != 0) ? value.Integer : 115200;
}

void settings_setUsartBaudRate(uint32_t value) {
    UINT32_UNION newValue;
    newValue.Integer = value;

    eeprom18_write(EEPROM_OFFSET_USART_SPEED + 0, newValue.Byte0);
    eeprom18_write(EEPROM_OFFSET_USART_SPEED + 1, newValue.Byte1);
    eeprom18_write(EEPROM_OFFSET_USART_SPEED + 2, newValue.Byte2);
    eeprom18_write(EEPROM_OFFSET_USART_SPEED + 3, newValue.Byte3);
}
