#include <xc.h>
#include <stdint.h>
#include <stdbool.h>

#include "can.h"
#include "device.h"
#include "hardware.h"
#include "io.h"
#include "uart.h"
#include "settings.h"
#include "state.h"


bool command_process_extra(uint8_t *buffer, uint8_t count);
bool command_process_query(uint8_t *buffer, uint8_t count);
bool command_process_query_extra(uint8_t *buffer, uint8_t count);

bool command_process_send(bool isExtended, bool isRemote, uint8_t *buffer, uint8_t count);
bool command_process_cansend(uint8_t *buffer, uint8_t count);

bool parseHex(uint8_t hexValue, uint8_t *value);
void sendErrorDetail(char error);


bool command_process(uint8_t *buffer, uint8_t count) {
    if (count == 0) { return false; }
    switch (buffer[0]) {

        case 'S': { //Set speed
            if (can_isOpen()) {
                sendErrorDetail('a');
                return false;
            }
            if (count == 2) {
                switch (buffer[1]) {
                    case '0':
                        sendErrorDetail('e');
                        return false;
                    case '1':   can_setup_20k(); return true;
                    case '2':   can_setup_50k(); return true;
                    case '3':  can_setup_100k(); return true;
                    case '4':  can_setup_125k(); return true;
                    case '5':  can_setup_250k(); return true;
                    case '6':  can_setup_500k(); return true;
                    case '7':  can_setup_800k(); return true;
                    case '8': can_setup_1000k(); return true;
                    default: {
                        sendErrorDetail('p');
                        return false;
                    }
                }
            } else {
                sendErrorDetail('p');
                return false;
            }
        }

        case 's': { //Set speed
            if (can_isOpen()) {
                sendErrorDetail('a');
                return false;
            }
            if (count == 5) {
                uint8_t newBtr0, newBtr1;
                if (parseHex(buffer[1], &newBtr0) && parseHex(buffer[2], &newBtr0) && parseHex(buffer[3], &newBtr1) && parseHex(buffer[4], &newBtr1)) {
                    uint8_t newSjw = (newBtr0 >> 6) & 0b11;
                    uint8_t newBrp = newBtr0 & 0b111111;
                    bool newSam = (newBtr1 & 0b10000000) == 0b10000000;
                    uint8_t newSeg2Ph = (newBtr1 >> 4) & 0b111;
                    uint8_t newSeg1Tq = (newBtr1 & 0b1111) + 1;
                    if ((newBrp > 0) && (newSeg1Tq >= 2)) {
                        uint8_t newPrseg  = (newSeg1Tq > 8) ? (newSeg1Tq - 8 - 1) : 0;
                        uint8_t newSeg1Ph = (newSeg1Tq > 8) ? 7 : (newSeg1Tq - 1 - 1);
                        can_setup(newBrp, newPrseg, newSeg1Ph, newSeg2Ph, newSjw, newSam);
                        return true;
                    } else {
                        sendErrorDetail('e');
                        return false;
                    }
                } else {
                    sendErrorDetail('p');
                    return false;
                }
            } else {
                sendErrorDetail('p');
                return false;
            }
        }
        
        case 'O': { //Open channel
            if (can_isOpen()) {
                sendErrorDetail('a');
                return false;
            } else if (count == 1) {
                can_open();
                return true;
            } else {
                sendErrorDetail('p');
                return false;
            }
        }

        case 'L': { //Open channel (listen-only)
            if (can_isOpen()) {
                sendErrorDetail('a');
                return false;
            } else if (count == 1) {
                can_openListenOnly();
                return true;
            } else {
                sendErrorDetail('p');
                return false;
            }
        }

        case 'l': { //Open channel (loopback)
            if (can_isOpen()) {
                sendErrorDetail('a');
                return false;
            } else if (count == 1) {
                can_openLoopback();
                return true;
            } else {
                sendErrorDetail('p');
                return false;
            }
        }

        case 'C': { //Close channel
            if (count == 1) {
                if (can_isOpen()) {
                    can_close();
                    return true;
                } else {
                    sendErrorDetail('a');
                    return false;
                }
            } else {
                return command_process_cansend(&buffer[0], count); //behave as this is ID for sending message
            }
        }
        
        
        case 't': return command_process_send(false, false, &buffer[1], count - 1);
        case 'T': return command_process_send(true, false, &buffer[1], count - 1);
        case 'r': return command_process_send(false, true, &buffer[1], count - 1);
        case 'R': return command_process_send(true, true, &buffer[1], count - 1);

        case '0': case '1': case '2': case '3': case '4': case '5': case '6': case '7': case '8': case '9':
        case 'B': case 'D': case 'E':
        case 'a': case 'b': case 'c': case 'd': case 'e': case 'f':
            return command_process_cansend(&buffer[0], count);

        case 'X': { //Auto-polling
            if (count == 2) {
                switch (buffer[1]) {
                    case '0': State_AutoPoll = false; return true;
                    case '1': State_AutoPoll = true; return true;
                    default: {
                        sendErrorDetail('p');
                        return false;
                    }
                }
            } else {
                sendErrorDetail('p');
                return false;
            }
        }

        case 'P': { //Poll one
            if (count == 1) {
                if (State_AutoPoll) {
                    sendErrorDetail('e');
                    return false;
                } else {
                    State_ManualPollCount = 1;
                    return true;
                }
            } else {
                sendErrorDetail('p');
                return false;
            }
        }

        case 'A': { //Poll all
            if (count == 1) {
                if (State_AutoPoll) {
                    sendErrorDetail('e');
                    return false;
                } else {
                    State_ManualPollCount = UINT8_MAX;
                    return true;
                }
            } else {
                return command_process_cansend(&buffer[0], count); //behave as this is ID for sending message
            }
        }


        case 'V': { //Version
            if (count == 1) {
                uart_writeString("V");
                uart_writeUInt8(device_getMajor());
                uart_writeUInt8(device_getMinor());
                uart_writeString("10"); //software version
                return true;
            } else {
                sendErrorDetail('p');
                return false;
            }
        }
        
        case 'N': { //Serial number
            if (count == 1) {
                uart_writeString("N0000");
                return true;
            } else {
                sendErrorDetail('p');
                return false;
            }
        }

        case 'F': { //Flags
            if (count == 1) {
                CAN_STATUS status = can_getStatus();
                uint8_t data;
                data  = (RXB0CONbits.RXFUL & RXB1CONbits.RXFUL & B0CONbits.RXFUL & B1CONbits.RXFUL & B2CONbits.RXFUL & B3CONbits.RXFUL & B4CONbits.RXFUL & B5CONbits.RXFUL);
                data |= (TXB0CONbits.TXREQ) << 1;
                data |= (status.RxWarning | status.TxWarning) << 2;
                data |= (status.RxOverflow) << 3;
                data |= (status.RxPassive | status.TxPassive) << 5;
                data |= (TXB0CONbits.TXLARB) << 6;
                data |= (status.TxOff) << 7;
                uart_writeString("F");
                uart_writeHexUInt8(data);
                return true;
            } else {
                return command_process_cansend(&buffer[0], count); //behave as this is ID for sending message
            }
        }


        case 'U': { //Set USART speed
            if (can_isOpen()) {
                sendErrorDetail('a');
                return false;
            }
            if (count == 2) {
                switch (buffer[1]) {
                    case 'Y':
                        if (device_supports920K()) {
                            settings_setUsartBaudRate(921600);
                            reset();
                            return true;
                        } else {
                            sendErrorDetail('e');
                            return false;
                        }
                    case 'Z':
                        if (device_supports460K()) {
                            settings_setUsartBaudRate(460800);
                            reset();
                            return true;
                        } else {
                            sendErrorDetail('e');
                            return false;
                        }
                    case '0':
                        if (device_supports230K()) {
                            settings_setUsartBaudRate(230400);
                            reset();
                            return true;
                        } else {
                            sendErrorDetail('e');
                            return false;
                        }
                    case '1': settings_setUsartBaudRate(115200); reset(); return true;
                    case '2':  settings_setUsartBaudRate(57600); reset(); return true;
                    case '3':  settings_setUsartBaudRate(38400); reset(); return true;
                    case '4':  settings_setUsartBaudRate(19200); reset(); return true;
                    case '5':   settings_setUsartBaudRate(9600); reset(); return true;
                    case '6':   settings_setUsartBaudRate(2400); reset(); return true;
                    case '~':      settings_setUsartBaudRate(0); reset(); return true;
                    default: {
                        sendErrorDetail('p');
                        return false;
                    }
                }
            } else {
                sendErrorDetail('p');
                return false;
            }
        }

        case 'Q': { //Set auto-startup
            if (count == 2) {
                if ((buffer[1] == '0') || (buffer[1] == '1') || (buffer[1] == '2')) {
                    uint8_t autoOpenIndex = (buffer[1] - '0');
                    settings_setCanBusConfig(BRGCON1bits.BRP, BRGCON2bits.PRSEG, BRGCON2bits.SEG1PH, BRGCON3bits.SEG2PH, BRGCON1bits.SJW, BRGCON2bits.SAM, autoOpenIndex);
                    return true;
                } else if (buffer[1] == '~') {
                    settings_setCanBusConfig(0, 0, 0, 0, 0, 0, 0);
                    return true;
                } else {
                    sendErrorDetail('p');
                    return false;
                }
            } else {
                sendErrorDetail('p');
                return false;
            }
        }
        
        
        case '*': { //extended commands
            if (count > 1) {
                return command_process_extra(&buffer[1], count - 1);
            } else { //no second char
                sendErrorDetail('x');
                return false;
            }
        }

        default: {
            sendErrorDetail('x');
            return false;        
        }
    }

}

bool command_process_extra(uint8_t *buffer, uint8_t count) {
    switch (buffer[0]) {

        case 'D': {
            if (count == 1) {
                uint8_t state = 0;
                if (State_ExtraLf)     { state |= 0b00000001; }
                if (State_ErrorDetail) { state |= 0b00000010; }
                if (State_Echo)        { state |= 0b00010000; }
                if (State_Cansend)     { state |= 0b00100000; }
                uart_writeString("*D");
                uart_writeHexUInt8(state);
                return true;
            } else if (count == 2) {
                switch (buffer[1]) {
                    case '0':
                        State_ExtraLf     = false;
                        State_ErrorDetail = false;
                        State_Echo        = false;
                        State_Cansend     = false;
                        return true;
                    case '1':
                        State_ExtraLf     = true;
                        State_ErrorDetail = true;
                        State_Echo        = true;
                        State_Cansend     = true;
                        return true;
                    default: {
                        sendErrorDetail('p');
                        return false;
                    }
                }
            } else if (count == 3) {
                uint8_t newState;
                if (parseHex(buffer[1], &newState) && parseHex(buffer[2], &newState)) {
                    if ((newState & 0b01111000) > 0) { //check for invalid bits set
                        sendErrorDetail('x');
                        return false;
                    } else {
                        State_ExtraLf     = ((newState & 0b00000001) > 0);
                        State_ErrorDetail = ((newState & 0b00000010) > 0);
                        State_Echo        = ((newState & 0b00010000) > 0);
                        State_Cansend     = ((newState & 0b00100000) > 0);
                        return true;
                    }
                } else {
                    sendErrorDetail('p');
                    return false;
                }
            } else {
                sendErrorDetail('p');
                return false;
            }
        }

        case 'F': {
            if (count == 1) {
                uint8_t state = 0;
                switch (can_getState()) {
                    case CAN_STATE_CLOSED:
                        state = 0b00000000;
                        break;
                    case CAN_STATE_OPEN_LOOPBACK:
                        state = 0b00000001;
                        break;
                    case CAN_STATE_OPEN_LISTENONLY:
                        state = 0b00000010;
                        break;
                    case CAN_STATE_OPEN:
                        state = 0b00000011;
                        break;
                }
                if (State_AutoPoll) { state |= 0b00000100; }
                bool anyError = (RXB0CONbits.RXFUL & RXB1CONbits.RXFUL & B0CONbits.RXFUL & B1CONbits.RXFUL & B2CONbits.RXFUL & B3CONbits.RXFUL & B4CONbits.RXFUL & B5CONbits.RXFUL);
                if (!anyError) {
                    CAN_STATUS status = can_getStatus();
                    anyError |= (TXB0CONbits.TXREQ);
                    anyError |= (status.RxWarning | status.TxWarning);
                    anyError |= (status.RxOverflow);
                    anyError |= (status.RxPassive | status.TxPassive);
                    anyError |= (TXB0CONbits.TXLARB);
                    anyError |= (status.TxOff);
                }
                if (anyError) { state |= 0b00001000; }
                if (io_out_getPower()) { state |= 0b00010000; }
                if (io_out_getTermination()) { state |= 0b00100000; }
                if (State_LoadLevel > 0) { state |= 0b01000000; }
                if (State_ExtraLf || State_ErrorDetail || State_Echo || State_Cansend) { state |= 0b10000000; }
                uart_writeString("*F");
                uart_writeHexUInt8(state);
                return true;
            } else {
                sendErrorDetail('p');
                return false;
            }
        }

        case 'L': {
            if (!can_isOpen()) {
                sendErrorDetail('a');
                return false;
            } else if (count == 1) {
                uart_writeString("*L");
                uart_writeByte(0x30 + State_LoadLevel);
                return true;
            } else if ((count == 2) && (buffer[1] >= '0') && (buffer[1] <= '9')) {
                State_LoadLevel = buffer[1] - 0x30;
                return true;
            } else {
                sendErrorDetail('p');
                return false;
            }
        }

        case 'P': {
            if (device_supportsPower()) {
                if (count == 1) {
                    uart_writeString("*P");
                    if (io_out_getPower()) { uart_writeByte('1'); } else { uart_writeByte('0'); }
                    return true;
                } else if ((count == 2) && (buffer[1] == '0')) {
                    io_out_powerOff();
                    return true;
                } else if ((count == 2) && (buffer[1] == '1')) {
                    io_out_powerOn();
                    return true;
                } else {
                    sendErrorDetail('p');
                    return false;
                }
            } else { //only supported on some devices
                sendErrorDetail('x');
                return false;
            }
        }

        case 'T': {
            if (device_supportsTermination()) {
                if (count == 1) {
                    uart_writeString("*T");
                    if (io_out_getTermination()) { uart_writeByte('1'); } else { uart_writeByte('0'); }
                    return true;
                } else if ((count == 2) && (buffer[1] == '0')) {
                    io_out_terminationOff();
                    return true;
                } else if ((count == 2) && (buffer[1] == '1')) {
                    io_out_terminationOn();
                    return true;
                } else {
                    sendErrorDetail('p');
                    return false;
                }
            } else { //only supported on some devices
                sendErrorDetail('x');
                return false;
            }
        }

        case 'R': {
            if (count == 1) {
                reset();
                return true;
            } else {
                sendErrorDetail('p');
                return false;
            }
        }

        case 'r': {
            if (count == 1) {
                can_close();
                can_setup_125k();

                io_out_powerOff();
                if (device_supportsTermination()) { io_out_terminationOn(); } //termination on by default
                State_AutoPoll = true;
                State_LoadLevel = 0;

                State_ExtraLf = false;
                State_ErrorDetail = false;
                State_Echo = false;
                State_Cansend = false;

                CAN_MESSAGE message;
                while(can_tryRead(&message));
                CanBufferStart = 0;
                CanBufferEnd = 0;
                CommandBufferCount = 0;

                return true;
            } else {
                sendErrorDetail('p');
                return false;
            }
        }

        default: {
            sendErrorDetail('x');
            return false;        
        }
    }
}


bool command_process_send(bool isExtended, bool isRemote, uint8_t *buffer, uint8_t count) {
    if (!can_isOpen()) { //channel not open
        sendErrorDetail('a');
        return false;
    }

    if (can_getState() == CAN_STATE_OPEN_LISTENONLY) { //channel is listen-only
        sendErrorDetail('a');
        return false;
    }
    
    uint8_t lenIndex = isExtended ? 8 : 3;
    if (count <= lenIndex) {
        sendErrorDetail('p');
        return false;
    }

    uint8_t len = buffer[lenIndex] - 0x30;
    if (len > 8) {
        sendErrorDetail('p');
        return false;
    }
    
    if (isRemote) {
        if (count != (lenIndex + 1)) { //no data bytes for remote
            sendErrorDetail('p');
            return false;
        }
    } else {
        if (count != (lenIndex + 1 + len * 2)) { //too many or not enough data bytes
            sendErrorDetail('p');
            return false;
        }
    }

    CAN_MESSAGE message;
    message.Header.ID = 0;
    message.Flags.IsRemoteRequest = isRemote ? 1 : 0;
    message.Flags.IsExtended = isExtended ? 1 : 0;
    message.Flags.Length = len;

    if (isExtended) {
        if (!parseHex(buffer[0], &message.Header.ID3) || !parseHex(buffer[1], &message.Header.ID3)
        || !parseHex(buffer[2], &message.Header.ID2) || !parseHex(buffer[3], &message.Header.ID2)
        || !parseHex(buffer[4], &message.Header.ID1) || !parseHex(buffer[5], &message.Header.ID1)
        || !parseHex(buffer[6], &message.Header.ID0) || !parseHex(buffer[7], &message.Header.ID0)
        || (message.Header.ID > 0x1FFFFFFF)) {
            sendErrorDetail('p');
            return false;
        }
    } else {
        if (!parseHex(buffer[0], &message.Header.ID1) || !parseHex(buffer[1], &message.Header.ID0) || !parseHex(buffer[2], &message.Header.ID0)
        || (message.Header.ID > 0x7FF)) {
            sendErrorDetail('p');
            return false;
        }
    }
    
    if (!isRemote) { //remote frames have no data
        uint8_t i = isExtended ? 9 : 4;
        for (uint8_t j = 0; j<len; j++) {
            if (!parseHex(buffer[i], &message.Data[j]) || !parseHex(buffer[i+1], &message.Data[j])) {
                sendErrorDetail('p');
                return false;
            }
            i+=2;
        }
    }
    
    if (can_tryWrite(message)) {
        return true;
    } else { //if buffer is full
        sendErrorDetail('a');
        return false;
    }
}

bool command_process_cansend(uint8_t *buffer, uint8_t count) {
    if (!can_isOpen()) { //channel not open
        sendErrorDetail('a');
        return false;
    }

    if (can_getState() == CAN_STATE_OPEN_LISTENONLY) { //channel is listen-only
        sendErrorDetail('a');
        return false;
    }
    
    if (count == 0) {
        sendErrorDetail('p');
        return false;
    }
    
    CAN_MESSAGE message;
    message.Header.ID = 0;
    message.Flags.IsRemoteRequest = 0;

    uint8_t state = 0; //0:ID 1:DataOrRemote 2:Data 3:RemoteLength 255:Done
    uint8_t idHexLength = 0;
    uint8_t dataHexLength = 0;
    
    for (uint8_t i = 0; i<count; i++) {
        uint8_t ch = buffer[i];
        uint8_t value = 255;
        switch (ch) {
            case '0': case '1': case '2': case '3': case '4': case '5': case '6': case '7': case '8': case '9':
                value = (ch - 0x30);
            case 'A': case 'B': case 'C': case 'D': case 'E': case 'F':
                if (value == 255) { value = (ch - 0x37); }
            case 'a': case 'b': case 'c': case 'd': case 'e': case 'f':
                if (value == 255) { value = (ch - 0x57); }
                    
                if (state == 0) { //ID
                    message.Header.ID <<= 4;
                    message.Header.ID |= value;
                    idHexLength++;
                } else if ((state == 1) || (state == 2)) { //DataOrRemote and Data
                    if (dataHexLength < 16) {
                        message.Data[dataHexLength / 2] <<= 4;
                        message.Data[dataHexLength / 2] |= value;
                        dataHexLength++;
                        state = 2; //->Data
                    } else { //way too long
                        sendErrorDetail('p');
                        return false;
                    }
                } else if (state == 3) { //RemoteLength
                    if (value <= 8) {
                        message.Flags.Length = value;
                        state = 255; //->Done
                    } else {
                        sendErrorDetail('p');
                        return false;
                    }
                } else {
                    sendErrorDetail('p');
                    return false;
                }
                break;
            
            case '#':
                if (state == 0) { //ID
                    state = 1; //->DataOrRemote
                } else {
                    sendErrorDetail('p');
                    return false;
                }
                break;

            case 'R':
            case 'r':
                if (state == 1) { //DataOrRemote
                    message.Flags.IsRemoteRequest = true;
                    state = 3; //->RemoteLength
                } else if (state == 0) { //ID
                    message.Flags.IsRemoteRequest = true;
                    state = 3; //->RemoteLength
                } else {
                    sendErrorDetail('p');
                    return false;
                }
                break;
                
            case '.':
            case ' ': //ignore dots and spaces
                break;

            default:
                sendErrorDetail('p');
                return false;
        }
    }
    
    if ((state == 0) && (idHexLength == 0)) { //not even ID is present
        sendErrorDetail('p');
        return false;
    }
    
    if ((dataHexLength % 2) != 0) { //not all data bytes are here
        sendErrorDetail('p');
        return false;
    }
    
    if (!message.Flags.IsRemoteRequest) {
        message.Flags.Length = (dataHexLength / 2);
    }
    
    if (idHexLength > 8) { //cannot have more than 8 characters in ID
        sendErrorDetail('p');
        return false;
    } else if ((idHexLength <= 3) && (message.Header.ID < 0x7FF)) {
        message.Flags.IsExtended = false;
    } else {
        message.Flags.IsExtended = true;
    }
    
    if (can_tryWrite(message)) {
        return true;
    } else { //if buffer is full
        sendErrorDetail('a');
        return false;
    }
}


bool parseHex(uint8_t hexValue, uint8_t *value) {
    *value <<= 4; //shift (potentially) previously parsed value to next nibble
    if ((hexValue >= 0x30) && (hexValue <= 0x39)) { //decimal hex value
        *value |= (hexValue - 0x30);
    } else if ((hexValue >= 0x41) && (hexValue <= 0x46)) { //uppercase hex value
        *value |= (hexValue - 0x37);
    } else if ((hexValue >= 0x61) && (hexValue <= 0x66)) { //lowercase hex value
        *value |= (hexValue - 0x57);
    } else {
        return false;
    }
    return true;
}

void sendErrorDetail(char error) {
    if (State_ErrorDetail) {
        uart_writeString(&error);
        uart_writeString("!");
    }
}
