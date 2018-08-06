#ifndef CAN_H
#define	CAN_H

#include <stdint.h>


typedef union {
    struct {
        uint32_t ID;
    };
    struct {
        uint8_t  ID0;
        uint8_t  ID1;
        uint8_t  ID2;
        uint8_t  ID3;
    };
} CAN_ID;

typedef struct {
    unsigned Length        : 4;
    unsigned               : 2;
    unsigned IsExtended    : 1;
    unsigned IsRemoteRequest : 1;
} CAN_FLAGS;

typedef struct {
    CAN_ID    Header;
    CAN_FLAGS Flags;
    uint8_t   Data[8];
} CAN_MESSAGE;


typedef struct {
    unsigned RxOrTxWarning     : 1;
    unsigned RxWarning         : 1;
    unsigned TxWarning         : 1;
    unsigned RxPassive         : 1;
    unsigned TxPassive         : 1;
    unsigned TxOff             : 1;
    unsigned RxOverflow        : 1;
    unsigned RxOverflowWarning : 1;
} CAN_STATUS;

typedef enum CAN_STATE {
    CAN_STATE_CLOSED,
    CAN_STATE_OPEN,
    CAN_STATE_OPEN_LISTENONLY,
    CAN_STATE_OPEN_LOOPBACK,
} CAN_STATE;


/** Initializes CAN module. */
void can_setup(uint8_t brp, uint8_t prseg, uint8_t seg1ph, uint8_t seg2ph, uint8_t sjw, bool sampleThree);

/** Initializes CAN module at 20 kbps. */
void can_setup_20k(void);

/** Initializes CAN module at 50 kbps. */
void can_setup_50k(void);

/** Initializes CAN module at 100 kbps. */
void can_setup_100k(void);

/** Initializes CAN module at 125 kbps. */
void can_setup_125k(void);

/** Initializes CAN module at 250 kbps. */
void can_setup_250k(void);

/** Initializes CAN module at 500 kbps. */
void can_setup_500k(void);

/** Initializes CAN module at 800 kbps. */
void can_setup_800k(void);

/** Initializes CAN module at 1000 kbps. */
void can_setup_1000k(void);


/** Returns CAN bus speed. */
uint16_t can_getSpeed(void);


/** Starts CAN bus. */
void can_open(void);

/** Starts CAN bus in listen-only mode. */
void can_openListenOnly(void);

/** Starts CAN bus in loopback mode. */
void can_openLoopback(void);

/** Stop CAN bus. */
void can_close(void);


/** Returns state of CAN bus channel. */
CAN_STATE can_getState(void);

/** Returns state of CAN bus channel. */
bool can_isOpen(void);


/** Returns CAN status. */
CAN_STATUS can_getStatus(void);


/** Blocking read of CAN message.  */
void can_read(CAN_MESSAGE* message);

/** Tries to read CAN message. Returns true if successful. */
bool can_tryRead(CAN_MESSAGE* message);

/** Blocking write of CAN message.  */
bool can_write(CAN_MESSAGE message);

/** Tries to write CAN message. Returns true if successful. */
bool can_tryWrite(CAN_MESSAGE message);

#endif
