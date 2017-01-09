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

class GameEventTeleportToC : public IMessage
{
public:
	GameEventTeleportToC();
	virtual ~GameEventTeleportToC();

public:
	static const unsigned short MESSAGE_ID = GameEventTeleportToC_ID;

private:
	FlatBufferBuilder m_Builder;

public:
	int m_nPlayerIndex;
	__int64 m_lEventTime;
	Vector3 m_vec3Start;
	Vector3 m_vec3Dest;
	int m_nState;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

