#include "stdafx.h"
#include "GameEvent_Move_ToS.h"
#include "NetworkDefines.h"
#include "../../rapidjson/document.h"
#include "../../rapidjson/stringbuffer.h"
#include "../../rapidjson/writer.h"

using namespace rapidjson;

GameEvent_Move_ToS::GameEvent_Move_ToS()
{
}


GameEvent_Move_ToS::~GameEvent_Move_ToS()
{
}

unsigned short GameEvent_Move_ToS::GetID()
{
	return (unsigned short)Messages::Game_Event_Move_ToS;
}

string GameEvent_Move_ToS::Serialize()
{
	Document document;
	document.SetObject();

	GetJSONObject(&document);

	GenericStringBuffer<UTF8<>> buffer;
	Writer<StringBuffer, UTF8<>> writer(buffer);
	document.Accept(writer);

	return buffer.GetString();
}

bool GameEvent_Move_ToS::Deserialize(string strJson)
{
	Document document;
	document.Parse<0>(strJson.c_str());
	if (!document.IsObject())
	{
		return false;
	}

	return SetByJSONObject(&document);
}

void GameEvent_Move_ToS::GetJSONObject(Document* pJsonObj)
{
	IGameEvent::GetJSONObject(pJsonObj);

	Value Pos_X(m_vec3Dest.x);
	pJsonObj->AddMember("Pos_X", Pos_X, pJsonObj->GetAllocator());

	Value Pos_Y(m_vec3Dest.y);
	pJsonObj->AddMember("Pos_Y", Pos_Y, pJsonObj->GetAllocator());

	Value Pos_Z(m_vec3Dest.z);
	pJsonObj->AddMember("Pos_Z", Pos_Z, pJsonObj->GetAllocator());
}
bool GameEvent_Move_ToS::SetByJSONObject(Document* pJsonObj)
{
	if (!IGameEvent::SetByJSONObject(pJsonObj))
	{
		return false;
	}

	if (pJsonObj->HasMember("Pos_X") && (*pJsonObj)["Pos_X"].IsDouble())
	{
		m_vec3Dest.x = (float)((*pJsonObj)["Pos_X"].GetDouble());
	}
	else
	{
		return false;
	}

	if (pJsonObj->HasMember("Pos_Y") && (*pJsonObj)["Pos_Y"].IsDouble())
	{
		m_vec3Dest.y = (float)((*pJsonObj)["Pos_Y"].GetDouble());
	}
	else
	{
		return false;
	}

	if (pJsonObj->HasMember("Pos_Z") && (*pJsonObj)["Pos_Z"].IsDouble())
	{
		m_vec3Dest.z = (float)((*pJsonObj)["Pos_Z"].GetDouble());
	}
	else
	{
		return false;
	}

	return true;
}
