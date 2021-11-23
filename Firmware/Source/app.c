#include <p18cxxx.h>
#include <stdbool.h>

#include "can.h"
#include "command.h"
#include "device.h"
#include "hardware.h"
#include "io.h"
#include "random.h"
#include "settings.h"
#include "state.h"
#include "uart.h"


#define BEL '\a'
#define LF  '\n'
#define CR  '\r'


#define LOAD_MASK  0xFFFF
#define LED_DECAY_START  0x1000


void processUart(void);
void reportBufferMessage(void);
void reportBufferEmpty(void);
void sendRandomMessage(void);


void main(void) {
    init();
    io_setup();

    if (device_needsClockOut()) { activate_clockOut(); } //for older FTDI-based devices
    if (device_supportsTermination()) { io_out_terminationOn(); } //termination on by default

    for (uint8_t i = 0; i < 3; i++) {
        io_led_on();
        wait_short();
        io_led_off();
        wait_short();
    }

    uart_init_withReadInterrupt();
    uart_setup(settings_getUsartBaudRate());
    interrupt_enable();

    uint8_t brp; uint8_t prseg; uint8_t seg1ph; uint8_t seg2ph; uint8_t sjw; bool sampleThree; uint8_t autoOpenIndex;
    if (settings_getCanBusConfig(&brp, &prseg, &seg1ph, &seg2ph, &sjw, &sampleThree, &autoOpenIndex)) {
        can_setup(brp, prseg, seg1ph, seg2ph, sjw, sampleThree);
        switch (autoOpenIndex) {
            case 1: can_open(); break;
            case 2: can_openListenOnly(); break;
        }
    } else {
        can_setup_125k();
    }


    uint16_t loadIndex = 0;
    uint16_t ledDecay = 0;

    while (true) {
        ClrWdt();

        bool toggleCanActivity = false;
        
        while ( (CanBufferCount < CAN_BUFFER_MAX) && can_tryRead(&CanBuffer[CanBufferEnd]) ) {
            toggleCanActivity = true;
            CanBufferEnd++;
            CanBufferCount++;
        }

        if (State_AutoPoll) {
            if (CanBufferCount > 0) {
                toggleCanActivity = true;
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

        if (State_LoadLevel > 0) {
            if (can_isOpen()) {
                loadIndex++;
                if ((loadIndex & (LOAD_MASK >> State_LoadLevel)) == 0) {
                    toggleCanActivity = true;
                    sendRandomMessage();
                }
            } else {
                State_LoadLevel = 0;
            }
        }

        if (can_isOpen()) {
            if (toggleCanActivity) {
                io_led_toggle(); //not turned off to prevent it never lighting under load
                if (ledDecay == 0) { ledDecay = LED_DECAY_START; }
            } else if (ledDecay == 0) {
                io_led_on();
            } else {
                ledDecay--;
            }
        } else {
            io_led_off();
            ledDecay = 0;
        }
    }
}

void __interrupt() isr() {
    while ( (UartBufferCount < UART_BUFFER_MAX) && uart_tryReadByte(&UartBuffer[UartBufferEnd]) ) {
        UartBufferEnd = (UartBufferEnd + 1) % UART_BUFFER_MAX;
        UartBufferCount++;
    }
}

void processUart() {
    uint8_t data;
    while (UartBufferCount > 0) {
        data = UartBuffer[UartBufferStart];
        UartBufferStart = (UartBufferStart + 1) % UART_BUFFER_MAX;
        UartBufferCount--;

        ClrWdt();
        if (can_isOpen()) { io_led_off(); } else { io_led_on(); } //toggle LED

        if (State_Echo) { uart_writeByte(data); }

        if ((data == CR) || (data == LF)) {
            if (CommandBufferCount > 0) {
                if (command_process(CommandBuffer, CommandBufferCount)) {
                    uart_writeByte(CR);
                    if (State_ExtraLf) { uart_writeByte(LF); }
                } else {
                    if (State_ExtraLf) { uart_writeByte(CR); uart_writeByte(LF); }
                    uart_writeByte(BEL);
                }

                CommandBufferCount = 0;
            }

        } else {
            if (CommandBufferCount < COMMAND_BUFFER_MAX) {
                CommandBuffer[CommandBufferCount] = data;
                CommandBufferCount++;
            } else {
                CommandBufferCount = 255; //overflow
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

void sendRandomMessage() {
    CAN_MESSAGE message;
    message.Flags.IsExtended = ((random_getByte() & 0x01) == 0); //50% extended
    if (message.Flags.IsExtended) {
        message.Header.ID = (random_getByte() >> 5); message.Header.ID <<= 8;
        message.Header.ID |= random_getByte();       message.Header.ID <<= 8;
        message.Header.ID |= random_getByte();       message.Header.ID <<= 8;
        message.Header.ID |= random_getByte();
    } else {
        message.Header.ID = (random_getByte() >> 5); message.Header.ID <<= 8;
        message.Header.ID |= random_getByte();
    }

    message.Flags.IsRemoteRequest = ((random_getByte() & 0x1F) == 0); //3.1% remote frame probability
    message.Flags.Length = (random_getByte() & 0x07); //select length - each number has 12.5% probability
    if (!message.Flags.IsRemoteRequest) {
        if ((message.Flags.Length == 0) && (random_getByte() & 0x80)) { message.Flags.Length = 8; } //lengths 0 and 8 have 6.25% probability
        for (uint8_t i=0; i<message.Flags.Length; i++) {
            message.Data[i] = random_getByte();
        }
    }

    can_write(message);
}
