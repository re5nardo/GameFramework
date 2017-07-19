#pragma once

#ifdef max
#undef max
#undef min
#endif
#include "flatbuffers/flatbuffers.h"
#include "../../FBSFiles/GameEventData_generated.h"
#include "../../CommonSources/Message/ISerializable.h"

using namespace flatbuffers;

class IGameEvent : public ISerializable
{
public:
	IGameEvent();
	virtual ~IGameEvent();

protected:
	FlatBufferBuilder m_Builder;

public:
	float m_fEventTime;

public:
	virtual FBS::GameEventType GetType() = 0;
};