#pragma once

#include "ISerializable.h"
#include "IDeserializable.h"

using namespace std;

//	JSON format
class IMessage : public ISerializable, public IDeserializable
{
public:
	IMessage(){};
	virtual ~IMessage(){};

public:
	virtual unsigned short GetID() = 0;
};