#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../CommonSources/Message/MessageIDs.h"

class PreparationStateToR : public IMessage
{
public:
	PreparationStateToR();
	virtual ~PreparationStateToR();

public:
	static const unsigned short MESSAGE_ID = PreparationStateToR_ID;

public:
	float m_fState;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

