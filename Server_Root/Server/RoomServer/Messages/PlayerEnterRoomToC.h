#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../CommonSources/Message/MessageIDs.h"

class PlayerEnterRoomToC : public IMessage
{
public:
	PlayerEnterRoomToC();
	virtual ~PlayerEnterRoomToC();

public:
	static const unsigned short MESSAGE_ID = PlayerEnterRoomToC_ID;

public:
	int m_nPlayerIndex;
	string m_strCharacterID;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

