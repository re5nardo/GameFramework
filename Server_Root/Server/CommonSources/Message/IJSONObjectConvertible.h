#pragma once

#include "../../../rapidjson/document.h"
#include "../../../rapidjson/rapidjson.h"

using namespace rapidjson;

class IJSONObjectConvertible
{
public:
	IJSONObjectConvertible(){};
	virtual ~IJSONObjectConvertible(){};

public:
	virtual Value* GetJSONObject(Document* pDocument) = 0;
	virtual bool SetJSONObject(Value* pValue) = 0;
};