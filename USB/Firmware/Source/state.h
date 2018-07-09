#ifndef STATE_H
#define	STATE_H

#include <stdbool.h>
#include <stdint.h>


bool State_ExtraLf = false;
bool State_ExtendedError = false;
bool State_Cansend = false;
bool State_Echo = false;
bool State_AutoPoll = true;
uint8_t State_ManualPollCount = 0; //UINT8_MAX to poll 

#endif
