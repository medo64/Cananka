#include <pic18f25k80.h>
#include <stdbool.h>
#include <stdint.h>

#include "uart.h"
#include "config.h"


void resetRx() {
    CREN1 = 0; //disable continous receive, also clears errors
    CREN1 = 1; //enable continous receive
    uint8_t dummyRead;
    dummyRead = RCREG1;
    dummyRead = RCREG1;
    dummyRead = RCREG1; //read data to clear FIFO
    SPEN1 = 0; //disable USART.
    SPEN1 = 1; //enable USART.
}


void uart_init(uint32_t desiredBaudRate) { //must be 19200 or less
    SPBRG = (uint16_t)(_XTAL_FREQ / desiredBaudRate / 4);
    BRG161 = 1; //16-bit
    BRGH1  = 1; //high speed
    SYNC1  = 0; //asynchronous mode
    SPEN1  = 1; //serial port enabled
    TXEN1  = 1;
    CREN1  = 1;
    resetRx();
}


bool uart_canRead() {
    return RC1IF ? true : false;
}

bool uart_canWrite() {
    return TXIF ? true : false;
}


uint8_t uart_readByte() {
    if (FERR1) { resetRx(); } //framing error
    if (OERR1) { resetRx(); } //overrun error
    while (!RC1IF);
    return RCREG1;
}

bool uart_tryReadByte(uint8_t* value) {
    if (FERR1) { resetRx(); } //framing error
    if (OERR1) { resetRx(); } //overrun error
    if (RC1IF) {
        *value = RCREG1;
        return true;
    } else {
        return false;
    }
}


void uart_writeByte(uint8_t value) {
    while (!TXIF); //wait until buffer is empty
    TXREG1 = value;
}

bool uart_tryWriteByte(uint8_t value) {
    if (TXIF) {
        TXREG1 = value;
        return true;
    } else {
        return false;
    }
}


void uart_writeBytes(uint8_t *value, uint8_t count) {
    for (uint8_t i = 0; i < count; i++) {
        while (!TXIF); //wait until buffer is empty
        TXREG1 = value[i];
    }
}

void uart_writeString(const char* text) {
    for (uint8_t i = 0; i < 255; i++) { //allow max 255 characters
        if (text[i] == '\0') {
            break;
        } else {
            uart_writeByte(text[i]);
        }
    }
}


void uart_writeUInt8(uint8_t number) {
    uint8_t chars[3] = { 0 };
    uint8_t i = 0;
    for (; i < sizeof(chars); i++) {
        chars[i] = 0x30 + (number % 10);
        number /= 10;
        if (number == 0) { break; }
    }
    for (; i != 255; i--) {
        if (chars[i] != 0) { uart_writeByte(chars[i]); }
    }
}

void uart_writeUInt16(uint16_t number) {
    uint8_t chars[5] = { 0 };
    uint8_t i = 0;
    for (; i < sizeof(chars); i++) {
        chars[i] = 0x30 + (number % 10);
        number /= 10;
        if (number == 0) { break; }
    }
    for (; i != 255; i--) {
        if (chars[i] != 0) { uart_writeByte(chars[i]); }
    }
}


void uart_writeHexDigit(uint8_t value) {
    uint8_t data = 0x30 + (value & 0x0F);
    if (data > 0x39) { data += 7; }
    uart_writeByte(data);
}

void uart_writeHexUInt8(uint8_t value) {
    uart_writeHexDigit(value >> 4);
    uart_writeHexDigit(value);
}
