#pragma once

#include "Data.h"
#include "../../rapidjson/document.h"
#include "IJSONObjectable.h"
#include "IMessage.h"

using namespace rapidjson;

class IGameEvent : public IMessage, IJSONObjectable
{
public:
	IGameEvent();
	virtual ~IGameEvent();

public:
	int m_nPlayerIndex = 0;				//  json field name : PlayerIndex
	int m_nElapsedTime = 0;				//  json field name : ElapsedTime

public:
	void GetJSONObject(Document* pJsonObj) override;
	bool SetByJSONObject(Document* pJsonObj) override;

	virtual unsigned short GetID() = 0;
	virtual string Serialize() = 0;
	virtual bool Deserialize(string strJson) = 0;
};