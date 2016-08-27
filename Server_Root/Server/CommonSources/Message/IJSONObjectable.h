#pragma once

#include "../../rapidjson/document.h"

using namespace rapidjson;

class IJSONObjectable
{
public:
	IJSONObjectable(){};
	virtual ~IJSONObjectable(){};

public:
	virtual void GetJSONObject(Document* pJsonObj) = 0;
	virtual bool SetByJSONObject(Document* pJsonObj) = 0;
};



