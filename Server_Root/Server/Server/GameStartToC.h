#pragma once

#include "IMessage.h"

class GameStartToC : public IMessage
{
public:
	GameStartToC();
	virtual ~GameStartToC();

public:
	unsigned __int64 m_lGameElapsedTime;		//	json field name : GameElapsedTime

public:
	unsigned short GetID() override;
	string Serialize() override;
	bool Deserialize(string strJson) override;
};

