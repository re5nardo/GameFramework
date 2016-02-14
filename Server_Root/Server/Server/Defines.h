#pragma once

class IMessage;

typedef void(*DefaultHandler)(void);
typedef void(*BoolHandler)(bool);
typedef void(*MessageHandler)(IMessage*);

typedef struct
{
	float x;
	float y;
	float z;
}Vector3;

