#pragma once

#include "IMessage.h"

class JoinLobbyToC : public IMessage
{
public:
	JoinLobbyToC();
	virtual ~JoinLobbyToC();

public:
	int m_nResult;     //  json field name : Result

public:
	unsigned short GetID() override;
	string Serialize() override;
	bool Deserialize(string strJson) override;
};

