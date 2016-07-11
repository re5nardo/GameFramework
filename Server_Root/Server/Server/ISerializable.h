#pragma once

#include <string>

using namespace std;

class ISerializable
{
public:
	ISerializable(){};
	virtual ~ISerializable(){};

public:
	virtual string Serialize() = 0;
};