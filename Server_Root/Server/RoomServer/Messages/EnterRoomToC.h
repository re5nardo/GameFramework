#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../CommonSources/Message/MessageIDs.h"
#include <map>
#ifdef max
#undef max
#undef min
#endif
#include "flatbuffers/flatbuffers.h"

using namespace flatbuffers;

class EnterRoomToC : public IMessage
{
public:
	EnterRoomToC();
	virtual ~EnterRoomToC();

public:
	static const unsigned short MESSAGE_ID = EnterRoomToC_ID;

private:
	FlatBufferBuilder m_Builder;

public:
	int m_nResult;
	int m_nPlayerIndex;
	map<int, string> m_mapPlayers;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

