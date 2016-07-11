#include "stdafx.h"

#include "../../rapidjson/stringbuffer.h"
#include "../../rapidjson/writer.h"
#include "IGameEvent.h"

using namespace rapidjson;

IGameEvent::IGameEvent()
{
}


IGameEvent::~IGameEvent()
{
}


void IGameEvent::GetJSONObject(Document* pJsonObj)
{
	Value PlayerIndex(m_nPlayerIndex);
	pJsonObj->AddMember("PlayerIndex", m_nPlayerIndex, pJsonObj->GetAllocator());

	Value ElapsedTime(m_nElapsedTime);
	pJsonObj->AddMember("ElapsedTime", m_nElapsedTime, pJsonObj->GetAllocator());
}

bool IGameEvent::SetByJSONObject(Document* pJsonObj)
{
	if (!pJsonObj->IsObject())
	{
		return false;
	}

	if (pJsonObj->HasMember("PlayerIndex") && (*pJsonObj)["PlayerIndex"].IsInt())
	{
		m_nPlayerIndex = (*pJsonObj)["PlayerIndex"].GetInt();
	}
	else
	{
		return false;
	}

	if (pJsonObj->HasMember("ElapsedTime") && (*pJsonObj)["ElapsedTime"].IsInt())
	{
		m_nElapsedTime = (*pJsonObj)["ElapsedTime"].GetInt();
	}
	else
	{
		return false;
	}

	return true;
}