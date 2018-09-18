#ifndef STATE_H
#define	STATE_H

#include <stdbool.h>
#include <stdint.h>


bool State_ExtraLf = false;
bool State_ErrorDetail = false;
bool State_Echo = false;
bool State_Cansend = false;

uint8_t State_LoadLevel = 0;

bool State_AutoPoll = true;
uint8_t State_ManualPollCount = 0; //UINT8_MAX to poll 

#define UART_BUFFER_MAX  64
uint8_t UartBuffer[UART_BUFFER_MAX];
uint8_t UartBufferStart = 0;
uint8_t UartBufferEnd = 0;
uint8_t UartBufferCount = 0;

#define COMMAND_BUFFER_MAX  64
uint8_t CommandBuffer[COMMAND_BUFFER_MAX];
uint8_t CommandBufferCount = 0;

#define CAN_BUFFER_MAX  255
#define CAN_BUFFER_SIZE  256
CAN_MESSAGE CanBuffer[CAN_BUFFER_SIZE];
uint8_t CanBufferStart = 0;
uint8_t CanBufferEnd = 0;
uint8_t CanBufferCount = 0;

#endif
