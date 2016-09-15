#include "stdafx.h"
#include "GameEventMoveToS.h"
#include "../../CommonSources/Message/JSONHelper.h"


GameEventMoveToS::GameEventMoveToS()
{
	m_buffer = new GenericStringBuffer<UTF8<>>();
	m_writer = new Writer<StringBuffer, UTF8<>>(*m_buffer);
}

GameEventMoveToS::~GameEventMoveToS()
{
	delete m_buffer;
	delete m_writer;
}

unsigned short GameEventMoveToS::GetID()
{
	return MESSAGE_ID;
}

const char* GameEventMoveToS::Serialize()
{
	Document document;
	document.SetObject();

	JSONHelper::AddField(&document, "PlayerIndex", m_nPlayerIndex);
	JSONHelper::AddField(&document, "ElapsedTime", m_nElapsedTime);
	JSONHelper::AddField(&document, "Pos_X", m_vec3Dest.x);
	JSONHelper::AddField(&document, "Pos_Y", m_vec3Dest.y);
	JSONHelper::AddField(&document, "Pos_Z", m_vec3Dest.z);

	m_buffer->Clear();
	document.Accept(*m_writer);

	return m_buffer->GetString();
}

bool GameEventMoveToS::Deserialize(const char* pChar)
{
	Document document;
	document.Parse<0>(pChar);
	if (!document.IsObject())
	{
		return false;
	}

	if (!JSONHelper::GetField(&document, "PlayerIndex", &m_nPlayerIndex)) return false;
	if (!JSONHelper::GetField(&document, "ElapsedTime", &m_nElapsedTime)) return false;
	if (!JSONHelper::GetField(&document, "Pos_X", &m_vec3Dest.x)) return false;
	if (!JSONHelper::GetField(&document, "Pos_Y", &m_vec3Dest.y)) return false;
	if (!JSONHelper::GetField(&document, "Pos_Z", &m_vec3Dest.z)) return false;

	return true;
}