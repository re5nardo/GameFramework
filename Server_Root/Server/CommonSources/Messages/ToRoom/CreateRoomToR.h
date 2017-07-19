#pragma once

#include "../../../CommonSources/Message/IMessage.h"
#include "../../../CommonSources/Message/MessageIDs.h"
#include <vector>
#include <string>

class CreateRoomToR : public IMessage
{
public:
	CreateRoomToR();
	virtual ~CreateRoomToR();

public:
	static const unsigned short MESSAGE_ID = CreateRoomToR_ID;

public:
	int m_nMatchID;
	vector<string> m_vecPlayers;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

