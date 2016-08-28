#pragma once
#include "../CommonSources/Message/IMessageConvertor.h"

class RoomMessageConvertor : public IMessageConvertor
{
public:
	RoomMessageConvertor();
	virtual ~RoomMessageConvertor();

public:
	IMessage* GetMessage(const unsigned short nMessageID, const char* pChar) override;
};