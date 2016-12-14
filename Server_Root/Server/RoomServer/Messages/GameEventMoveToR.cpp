#include "stdafx.h"
#include "GameEventMoveToR.h"
#include "../../CommonSources/Message/JSONHelper.h"


GameEventMoveToR::GameEventMoveToR()
{
	m_buffer = new GenericStringBuffer<UTF8<>>();
	m_writer = new Writer<StringBuffer, UTF8<>>(*m_buffer);
}

GameEventMoveToR::~GameEventMoveToR()
{
	delete m_buffer;
	delete m_writer;
}

unsigned short GameEventMoveToR::GetID()
{
	return MESSAGE_ID;
}

IMessage* GameEventMoveToR::Clone()
{
	GameEventMoveToR* clone = new GameEventMoveToR();

	clone->m_nPlayerIndex = m_nPlayerIndex;
	clone->m_vec3Dest = m_vec3Dest;

	return clone;
}

const char* GameEventMoveToR::Serialize()
{
	Document document;
	document.SetObject();

	JSONHelper::AddField(&document, "PlayerIndex", m_nPlayerIndex);
	JSONHelper::AddField(&document, "Pos_X", m_vec3Dest.x);
	JSONHelper::AddField(&document, "Pos_Y", m_vec3Dest.y);
	JSONHelper::AddField(&document, "Pos_Z", m_vec3Dest.z);

	m_buffer->Clear();
	document.Accept(*m_writer);

	return m_buffer->GetString();
}

bool GameEventMoveToR::Deserialize(const char* pChar)
{
	Document document;
	document.Parse<0>(pChar);
	if (!document.IsObject())
	{
		return false;
	}

	if (!JSONHelper::GetField(&document, "PlayerIndex", &m_nPlayerIndex)) return false;
	if (!JSONHelper::GetField(&document, "Pos_X", &m_vec3Dest.x)) return false;
	if (!JSONHelper::GetField(&document, "Pos_Y", &m_vec3Dest.y)) return false;
	if (!JSONHelper::GetField(&document, "Pos_Z", &m_vec3Dest.z)) return false;

	return true;
}