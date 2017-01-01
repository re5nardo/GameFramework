#pragma once

#include "../CommonSources/Message/IJSONObjectConvertible.h"
#include "../CommonSources/Message/JSONHelper.h"

typedef struct : public IJSONObjectConvertible 
{
public:
	float x;		//  json field name : x
	float y;		//  json field name : y
	float z;		//  json field name : z

public:
	Value* GetJSONObject(Document* pDocument) override
	{
		Value* pValue = new Value();
		pValue->SetObject();

		JSONHelper::AddField(pValue, pDocument, "x", x);
		JSONHelper::AddField(pValue, pDocument, "y", y);
		JSONHelper::AddField(pValue, pDocument, "z", z);

		return pValue;
	}

	bool SetJSONObject(Value* pValue) override
	{
		if (!pValue->IsObject())
		{
			return false;
		}

		if (!JSONHelper::GetField(pValue, "x", &x)) return false;
		if (!JSONHelper::GetField(pValue, "y", &y)) return false;
		if (!JSONHelper::GetField(pValue, "z", &z)) return false;

		return true;
	}
}Vector3;

enum GameEventType
{
	None,
	Idle,
	Move,
	Stop,
	Skill,
	Gesture,
	GetItem,
	Collision,
	Die,
};