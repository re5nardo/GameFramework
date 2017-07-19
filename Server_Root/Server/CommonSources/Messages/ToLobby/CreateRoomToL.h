#pragma once

#include "../../../CommonSources/Message/IMessage.h"
#include "../../../CommonSources/Message/MessageIDs.h"
#include <vector>
#include <string>

class CreateRoomToL : public IMessage
{
public:
	CreateRoomToL();
	virtual ~CreateRoomToL();

public:
	static const unsigned short MESSAGE_ID = CreateRoomToL_ID;

public:
	int m_nResult;
	vector<string> m_vecPlayers;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

