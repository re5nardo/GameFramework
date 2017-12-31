#pragma once

#ifdef max
#undef max
#undef min
#endif
#include "flatbuffers/flatbuffers.h"
#include "../../FBSFiles/PlayerInputData_generated.h"
#include "../../CommonSources/Message/ISerializable.h"
#include "../../CommonSources/Message/IDeserializable.h"

using namespace flatbuffers;

class IPlayerInput : public ISerializable, public IDeserializable
{
public:
	IPlayerInput();
	virtual ~IPlayerInput();

protected:
	FlatBufferBuilder m_Builder;

public:
	virtual FBS::PlayerInputType GetType() = 0;
	virtual IPlayerInput* Clone() = 0;
};