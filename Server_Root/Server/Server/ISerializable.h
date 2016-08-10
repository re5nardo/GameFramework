#pragma once

#include <string>

using namespace std;

class ISerializable
{
public:
	ISerializable(){};
	virtual ~ISerializable(){};

public:
	virtual const char* Serialize() = 0;
};