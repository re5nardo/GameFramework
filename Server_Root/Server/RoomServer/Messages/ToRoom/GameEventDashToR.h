#pragma once

#include "../../../CommonSources/Message/IMessage.h"
#include "../../../CommonSources/Message/MessageIDs.h"

class GameEventDashToR : public IMessage
{
public:
	GameEventDashToR();
	virtual ~GameEventDashToR();

public:
	static const unsigned short MESSAGE_ID = GameEventDashToR_ID;

public:
	int m_nPlayerIndex;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

