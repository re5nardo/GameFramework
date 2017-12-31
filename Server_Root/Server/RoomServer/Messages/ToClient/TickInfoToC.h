#pragma once

#include "../../../CommonSources/Message/IMessage.h"
#include "../../../CommonSources/Message/MessageIDs.h"
#include <list>

class IPlayerInput;

using namespace std;

class TickInfoToC : public IMessage
{
public:
	TickInfoToC();
	virtual ~TickInfoToC();

public:
	static const unsigned short MESSAGE_ID = TickInfoToC_ID;

public:
	int m_nTick;
	list<IPlayerInput*> m_listPlayerInput;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

