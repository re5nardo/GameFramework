#pragma once

#include <string>

using namespace std;

class ISerializable
{
public:
	ISerializable(){};
	virtual ~ISerializable(){};

public:
	virtual const char* Serialize(int* pLength = NULL) = 0;
};