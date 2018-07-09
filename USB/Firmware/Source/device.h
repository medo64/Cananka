#ifndef DEVICE_H
#define DEVICE_H

#include <stdint.h>

typedef enum DEVICE_TYPE {
    DEVICE_UNKNOWN,
    DEVICE_CANANKA_USB,
    DEVICE_CANANKA_USB_RJ45,
    DEVICE_CANANKA_USB_MINI
} DEVICE_TYPE;

DEVICE_TYPE device_getType(void);

#endif
