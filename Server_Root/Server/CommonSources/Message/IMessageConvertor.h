#pragma once

class IMessage;

class IMessageConvertor
{
public:
	IMessageConvertor(){};
	virtual ~IMessageConvertor(){};

public:
	virtual IMessage* GetMessage(const unsigned short nMessageID, const char* pChar) = 0;
};