#pragma once

#include "../../../CommonSources/Message/IMessage.h"
#include "../../../CommonSources/Message/MessageIDs.h"

class EnterRoomToR : public IMessage
{
public:
	EnterRoomToR();
	virtual ~EnterRoomToR();

public:
	static const unsigned short MESSAGE_ID = EnterRoomToR_ID;

public:
	string m_strPlayerKey;
	int m_nAuthKey;
	int m_nMatchID;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

