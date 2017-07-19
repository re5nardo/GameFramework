#pragma once

#include "../../../CommonSources/Message/IMessage.h"
#include "../../../CommonSources/Message/MessageIDs.h"

class SelectNormalGameToL : public IMessage
{
public:
	SelectNormalGameToL();
	virtual ~SelectNormalGameToL();

public:
	static const unsigned short MESSAGE_ID = SelectNormalGameToL_ID;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

