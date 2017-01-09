#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../CommonSources/Message/MessageIDs.h"
#include "../../RoomServer/Data.h"
#ifdef max
#undef max
#undef min
#endif
#include "flatbuffers/flatbuffers.h"

using namespace flatbuffers;

class GameEventIdleToR : public IMessage
{
public:
	GameEventIdleToR();
	virtual ~GameEventIdleToR();

public:
	static const unsigned short MESSAGE_ID = GameEventIdleToR_ID;

private:
	FlatBufferBuilder m_Builder;

public:
	int m_nPlayerIndex;
	Vector3 m_vec3Pos;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

