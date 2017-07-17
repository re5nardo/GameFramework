#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../CommonSources/Message/MessageIDs.h"

class GameStartToC : public IMessage
{
public:
	GameStartToC();
	virtual ~GameStartToC();

public:
	static const unsigned short MESSAGE_ID = GameStartToC_ID;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

