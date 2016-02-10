#pragma once

#include <string>

using namespace std;

//	JSON format
class IMessage
{
public:
	IMessage(){};
	virtual ~IMessage(){};

public:
	virtual unsigned short GetID() = 0;
	virtual string Serialize() = 0;
	virtual bool Deserialize(string strJson) = 0;
};