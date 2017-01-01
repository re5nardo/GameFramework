#include "stdafx.h"
#include "GameEventStopToR.h"
#include "../../CommonSources/Message/JSONHelper.h"


GameEventStopToR::GameEventStopToR()
{
	m_buffer = new GenericStringBuffer<UTF8<>>();
	m_writer = new Writer<StringBuffer, UTF8<>>(*m_buffer);
}

GameEventStopToR::~GameEventStopToR()
{
	delete m_buffer;
	delete m_writer;
}

unsigned short GameEventStopToR::GetID()
{
	return MESSAGE_ID;
}

IMessage* GameEventStopToR::Clone()
{
	GameEventStopToR* clone = new GameEventStopToR();

	clone->m_nPlayerIndex = m_nPlayerIndex;
	clone->m_vec3Pos = m_vec3Pos;

	return clone;
}

const char* GameEventStopToR::Serialize()
{
	Document document;
	document.SetObject();

	JSONHelper::AddField(&document, &document, "PlayerIndex", m_nPlayerIndex);
	JSONHelper::AddField(&document, &document, "Pos_X", m_vec3Pos.x);
	JSONHelper::AddField(&document, &document, "Pos_Y", m_vec3Pos.y);
	JSONHelper::AddField(&document, &document, "Pos_Z", m_vec3Pos.z);

	m_buffer->Clear();
	document.Accept(*m_writer);

	return m_buffer->GetString();
}

bool GameEventStopToR::Deserialize(const char* pChar)
{
	Document document;
	document.Parse<0>(pChar);
	if (!document.IsObject())
	{
		return false;
	}

	if (!JSONHelper::GetField(&document, "PlayerIndex", &m_nPlayerIndex)) return false;
	if (!JSONHelper::GetField(&document, "Pos_X", &m_vec3Pos.x)) return false;
	if (!JSONHelper::GetField(&document, "Pos_Y", &m_vec3Pos.y)) return false;
	if (!JSONHelper::GetField(&document, "Pos_Z", &m_vec3Pos.z)) return false;

	return true;
}