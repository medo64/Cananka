#include <p18cxxx.h>
#include <stdbool.h>

#include "can.h"
#include "command.h"
#include "device.h"
#include "hardware.h"
#include "io.h"
#include "state.h"
#include "uart.h"


#define BEL '\a'
#define LF  '\n'
#define CR  '\r'


#define UART_BUFFER_MAX  64
uint8_t UartBuffer[UART_BUFFER_MAX];
uint8_t UartBufferCount = 0;

#define CAN_BUFFER_MAX  255
CAN_MESSAGE CanBuffer[CAN_BUFFER_MAX];
uint8_t CanBufferStart = 0;
uint8_t CanBufferEnd = 0;
uint8_t CanBufferCount = 0;

void processUart(void);
void reportBufferMessage(void);
void reportBufferEmpty(void);


void main(void) {    
    init();
    io_setup();

    if (device_needsClockOut()) { activate_clockOut(); } //for older FTDI-based devices

    for (uint8_t i = 0; i < 3; i++) {
        io_led_on();
        wait_short();
        io_led_off();
        wait_short();
    }

    uart_setup(115200);
    can_setup_125k();


    uint16_t ledDelay = 0;
    
    while (true) {
        ClrWdt();
        
        if (can_isOpen()) {
            ledDelay--;
            if (ledDelay == 0xD000) { io_led_on(); }
        } else {
            io_led_off();
        }

        while ( (CanBufferCount < CAN_BUFFER_MAX) && can_tryRead(&CanBuffer[CanBufferEnd]) ) {
            io_led_off(); ledDelay = 0;
            CanBufferEnd++;
            CanBufferCount++;
        }

        if (State_AutoPoll) {
            if (CanBufferCount > 0) {
                io_led_off(); ledDelay = 0;
                reportBufferMessage();
                CanBufferStart++;
                CanBufferCount--;
            }
        } else if (State_ManualPollCount > 0) {
            if (CanBufferCount > 0) {
                while ((CanBufferCount > 0) && (State_ManualPollCount > 0)) {
                    reportBufferMessage();
                    CanBufferStart++;
                    CanBufferCount--;
                    State_ManualPollCount--;
                }
                State_ManualPollCount = 0; //stop polling for now
            } else { //if no messages to poll, just report empty buffer and be done with it
                reportBufferEmpty();
                State_ManualPollCount = 0;
            }
        }
        
        processUart();
    }
}


void processUart() {
    uint8_t data;
    while (uart_tryReadByte(&data)) {
        ClrWdt();
        if (can_isOpen()) { io_led_off(); } else { io_led_on(); } //toggle LED

        if (State_Echo) { uart_writeByte(data); }

        if ((data == CR) || (data == LF)) {
            if (UartBufferCount > 0) {
                if (command_process(UartBuffer, UartBufferCount)) {
                    uart_writeByte(CR);
                    if (State_ExtraLf) { uart_writeByte(LF); }
                } else {
                    if (State_ExtraLf) { uart_writeByte(CR); uart_writeByte(LF); }
                    uart_writeByte(BEL);
                }

                UartBufferCount = 0;
            }

        } else {
            if (UartBufferCount < UART_BUFFER_MAX) {
                UartBuffer[UartBufferCount] = data;
                UartBufferCount++;
            } else {
                UartBufferCount = 255; //overflow
            }
        }
    }
}


void reportBufferMessage() {
    CAN_MESSAGE message = CanBuffer[CanBufferStart];
    if (State_Cansend) {
        if (message.Flags.IsExtended) {
            uart_writeHexUInt8(message.Header.ID3);
            uart_writeHexUInt8(message.Header.ID2);
            uart_writeHexUInt8(message.Header.ID1);
            uart_writeHexUInt8(message.Header.ID0);
        } else {
            uart_writeHexDigit(message.Header.ID1);
            uart_writeHexUInt8(message.Header.ID0);
        }
        uart_writeByte('#');
        if (message.Flags.IsRemoteRequest) {
            uart_writeByte('R');
            if (message.Flags.Length > 0) { uart_writeHexDigit(message.Flags.Length); }
        } else {
            for (uint8_t i = 0; i < message.Flags.Length; i++) {
                uart_writeHexUInt8(message.Data[i]);
            }
        }
    } else { //SLCAN
        if (message.Flags.IsExtended) {
            if (message.Flags.IsRemoteRequest) { uart_writeByte('R'); } else { uart_writeByte('T'); }
            uart_writeHexUInt8(message.Header.ID3);
            uart_writeHexUInt8(message.Header.ID2);
            uart_writeHexUInt8(message.Header.ID1);
            uart_writeHexUInt8(message.Header.ID0);
        } else {
            if (message.Flags.IsRemoteRequest) { uart_writeByte('r'); } else { uart_writeByte('t'); }
            uart_writeHexDigit(message.Header.ID1);
            uart_writeHexUInt8(message.Header.ID0);
        }
        uart_writeHexDigit(message.Flags.Length);
        if (!message.Flags.IsRemoteRequest) {
            for (uint8_t i = 0; i < message.Flags.Length; i++) {
                uart_writeHexUInt8(message.Data[i]);
            }
        }
    }
    uart_writeByte(CR);
    if (State_ExtraLf) { uart_writeByte(LF); }
}

void reportBufferEmpty() {
    uart_writeByte(CR);
    if (State_ExtraLf) { uart_writeByte(LF); }
}
