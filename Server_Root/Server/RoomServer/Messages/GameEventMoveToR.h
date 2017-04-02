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

class GameEventMoveToR : public IMessage
{
public:
	GameEventMoveToR();
	virtual ~GameEventMoveToR();

public:
	static const unsigned short MESSAGE_ID = GameEventMoveToR_ID;

private:
	FlatBufferBuilder m_Builder;

public:
	int m_nPlayerIndex;
	Vector3 m_vec3Dest;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

