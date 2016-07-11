#pragma once

#include "IMessage.h"

class JoinLobbyToS : public IMessage
{
public:
	JoinLobbyToS();
	virtual ~JoinLobbyToS();

public:
	unsigned __int64	m_nPlayerNumber;		//	json field name : PlayerNumber
	int					m_nAuthKey;             //  json field name : AuthKey

public:
	unsigned short GetID() override;
	string Serialize() override;
	bool Deserialize(string strJson) override;
};

