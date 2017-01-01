#pragma once

#include "ISerializable.h"
#include "IDeserializable.h"

using namespace std;

class IMessage : public ISerializable, public IDeserializable
{
public:
	IMessage(){};
	virtual ~IMessage(){};

public:
	virtual unsigned short GetID() = 0;
	virtual IMessage* Clone() = 0;
};