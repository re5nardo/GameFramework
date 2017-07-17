#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../CommonSources/Message/MessageIDs.h"
#include <string>

class JoinLobbyToL : public IMessage
{
public:
	JoinLobbyToL();
	virtual ~JoinLobbyToL();

public:
	static const unsigned short MESSAGE_ID = JoinLobbyToL_ID;

public:
	string m_strPlayerKey;
	int m_nAuthKey;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

