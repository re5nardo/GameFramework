#pragma once

#include "../../../CommonSources/Message/IMessage.h"
#include "../../../CommonSources/Message/MessageIDs.h"

class JoinLobbyToC : public IMessage
{
public:
	JoinLobbyToC();
	virtual ~JoinLobbyToC();

public:
	static const unsigned short MESSAGE_ID = JoinLobbyToC_ID;

public:
	int m_nResult;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};
