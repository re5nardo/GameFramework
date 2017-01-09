#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../CommonSources/Message/MessageIDs.h"
#include <vector>
#include <string>
#ifdef max
#undef max
#undef min
#endif
#include "flatbuffers/flatbuffers.h"

using namespace flatbuffers;

class CreateRoomToR : public IMessage
{
public:
	CreateRoomToR();
	virtual ~CreateRoomToR();

public:
	static const unsigned short MESSAGE_ID = CreateRoomToR_ID;

private:
	FlatBufferBuilder m_Builder;

public:
	int m_nMatchID;
	vector<string> m_vecPlayers;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

