#pragma once
#include "../CommonSources/Message/IMessageConvertor.h"

class LobbyMessageConvertor : public IMessageConvertor
{
public:
	LobbyMessageConvertor();
	virtual ~LobbyMessageConvertor();

public:
	IMessage* GetMessage(const unsigned short nMessageID, const char* pChar) override;
};

