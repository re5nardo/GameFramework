#pragma once

class IMessage;

typedef void(*DefaultHandler)(void);
typedef void(*BoolHandler)(bool);
typedef void(*MessageHandler)(IMessage*);

