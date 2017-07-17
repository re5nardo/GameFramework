#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../CommonSources/Message/MessageIDs.h"

class PreparationStateToC : public IMessage
{
public:
	PreparationStateToC();
	virtual ~PreparationStateToC();

public:
	static const unsigned short MESSAGE_ID = PreparationStateToC_ID;

public:
	int m_nPlayerIndex;
	float m_fState;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

