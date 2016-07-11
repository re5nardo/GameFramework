#pragma once

#include "IMessage.h"

class ReadyForStartToS : public IMessage
{
public:
	ReadyForStartToS();
	virtual ~ReadyForStartToS();

public:
	int m_nPlayerIndex;		//	json field name : PlayerIndex

public:
	unsigned short GetID() override;
	string Serialize() override;
	bool Deserialize(string strJson) override;
};

