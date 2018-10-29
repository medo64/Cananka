#include <xc.h>
#include <stdbool.h>
#include <stdint.h>

#include "hardware.h"
#include "uart.h"


void uart_init() {
    TRISC6 = 1;
    TRISC7 = 1;
}

void uart_init_withReadInterrupt() {
    uart_init();
    RC1IE = 1; //enables the receive interrupt
    RC1IP = 1; //high priority (only makes difference if IPEN=1)
    if (IPEN == 0) { //compatibility mode (default)
        PEIE = 1; //enables all unmasked peripheral interrupts
    }
}

void uart_setup(uint32_t desiredBaudRate) {
    SPBRG = (uint16_t)(_XTAL_FREQ / desiredBaudRate / 4);
    BRG161 = 1; //16-bit
    BRGH1  = 1; //high speed
    SYNC1  = 0; //asynchronous mode
    SPEN1  = 1; //serial port enabled
    TXEN1  = 1; //enable transmit
    CREN1  = 1; //enable receive

    uint8_t dummyRead;
    dummyRead = RCREG1; dummyRead = RCREG1; dummyRead = RCREG1; //read data to clear FIFO
}


bool uart_canRead() {
    return RC1IF ? true : false;
}

bool uart_canWrite() {
    return TXIF ? true : false;
}


uint8_t uart_readByte() {
    if (FERR1 || OERR1) { //framing or overrun error
        CREN1 = 0; //disable receive, also clears errors
        CREN1 = 1; //enable receive
        uint8_t dummyRead;
        dummyRead = RCREG1; dummyRead = RCREG1; dummyRead = RCREG1; //read data to clear FIFO
        SPEN1 = 0; //disable USART.
        SPEN1 = 1; //enable USART.
    }

    while (!RC1IF);
    return RCREG1;
}

bool uart_tryReadByte(uint8_t* value) {
    if (FERR1 || OERR1) { //framing or overrun error
        CREN1 = 0; //disable receive, also clears errors
        CREN1 = 1; //enable receive
        uint8_t dummyRead;
        dummyRead = RCREG1; dummyRead = RCREG1; dummyRead = RCREG1; //read data to clear FIFO
        SPEN1 = 0; //disable USART.
        SPEN1 = 1; //enable USART.
    }

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
