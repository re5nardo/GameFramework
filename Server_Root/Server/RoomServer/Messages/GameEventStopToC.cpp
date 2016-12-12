#include "stdafx.h"
#include "GameEventStopToC.h"
#include "../../CommonSources/Message/JSONHelper.h"


GameEventStopToC::GameEventStopToC()
{
	m_buffer = new GenericStringBuffer<UTF8<>>();
	m_writer = new Writer<StringBuffer, UTF8<>>(*m_buffer);
}

GameEventStopToC::~GameEventStopToC()
{
	delete m_buffer;
	delete m_writer;
}

unsigned short GameEventStopToC::GetID()
{
	return MESSAGE_ID;
}

IMessage* GameEventStopToC::Clone()
{
	return NULL;
}

const char* GameEventStopToC::Serialize()
{
	Document document;
	document.SetObject();

	JSONHelper::AddField(&document, "PlayerIndex", m_nPlayerIndex);
	JSONHelper::AddField(&document, "EventTime", m_lEventTime);
	JSONHelper::AddField(&document, "Pos_X", m_vec3Pos.x);
	JSONHelper::AddField(&document, "Pos_Y", m_vec3Pos.y);
	JSONHelper::AddField(&document, "Pos_Z", m_vec3Pos.z);

	m_buffer->Clear();
	document.Accept(*m_writer);

	return m_buffer->GetString();
}

bool GameEventStopToC::Deserialize(const char* pChar)
{
	Document document;
	document.Parse<0>(pChar);
	if (!document.IsObject())
	{
		return false;
	}

	if (!JSONHelper::GetField(&document, "PlayerIndex", &m_nPlayerIndex)) return false;
	if (!JSONHelper::GetField(&document, "EventTime", &m_lEventTime)) return false;
	if (!JSONHelper::GetField(&document, "Pos_X", &m_vec3Pos.x)) return false;
	if (!JSONHelper::GetField(&document, "Pos_Y", &m_vec3Pos.y)) return false;
	if (!JSONHelper::GetField(&document, "Pos_Z", &m_vec3Pos.z)) return false;

	return true;
}