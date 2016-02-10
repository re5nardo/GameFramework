#pragma once

void(*DefaultHandler)(void);
void(*BoolHandler)(bool);
void(*MessageHandler)(IMessage*);

enum Messages
{
	TEST_MESSAGE_ID = 0,
};