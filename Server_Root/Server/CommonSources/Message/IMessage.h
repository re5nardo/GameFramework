#pragma once

#include "ISerializable.h"
#include "IDeserializable.h"
#ifdef max
#undef max
#undef min
#endif
#include "flatbuffers/flatbuffers.h"

using namespace flatbuffers;
using namespace std;

class IMessage : public ISerializable, public IDeserializable
{
public:
	IMessage() : m_Builder(1024){};
	virtual ~IMessage(){};

protected:
	FlatBufferBuilder m_Builder;

public:
	virtual unsigned short GetID() = 0;
	virtual IMessage* Clone() = 0;
};