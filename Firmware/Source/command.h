#ifndef COMMAND_H
#define COMMAND_H

#include <stdint.h>
#include <stdbool.h>


bool command_process(uint8_t *buffer, uint8_t count);

#endif
