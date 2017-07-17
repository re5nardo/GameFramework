#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../CommonSources/Message/MessageIDs.h"
#include <map>

class EnterRoomToC : public IMessage
{
public:
	EnterRoomToC();
	virtual ~EnterRoomToC();

public:
	static const unsigned short MESSAGE_ID = EnterRoomToC_ID;

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

