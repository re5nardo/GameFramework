#pragma once

#include <string>

using namespace std;

class IDeserializable
{
public:
	IDeserializable(){};
	virtual ~IDeserializable(){};

public:
	virtual bool Deserialize(string strJson) = 0;
};

