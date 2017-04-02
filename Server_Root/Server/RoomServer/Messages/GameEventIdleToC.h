#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../CommonSources/Message/MessageIDs.h"
#include "../../CommonSources/MathematicalData.h"
#ifdef max
#undef max
#undef min
#endif
#include "flatbuffers/flatbuffers.h"

using namespace flatbuffers;
using namespace MathematicalData;

class GameEventIdleToC : public IMessage
{
public:
	GameEventIdleToC();
	virtual ~GameEventIdleToC();

public:
	static const unsigned short MESSAGE_ID = GameEventIdleToC_ID;

private:
	FlatBufferBuilder m_Builder;

public:
	int m_nPlayerIndex;
	__int64 m_lEventTime;
	Vector3 m_vec3Pos;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

