#include "stdafx.h"
#include "GameEventTeleportToR.h"
#include "../../CommonSources/Message/JSONHelper.h"


GameEventTeleportToR::GameEventTeleportToR()
{
	m_buffer = new GenericStringBuffer<UTF8<>>();
	m_writer = new Writer<StringBuffer, UTF8<>>(*m_buffer);
}

GameEventTeleportToR::~GameEventTeleportToR()
{
	delete m_buffer;
	delete m_writer;
}

unsigned short GameEventTeleportToR::GetID()
{
	return MESSAGE_ID;
}

IMessage* GameEventTeleportToR::Clone()
{
	return NULL;
}

const char* GameEventTeleportToR::Serialize()
{
	Document document;
	document.SetObject();

	JSONHelper::AddField(&document, &document, "PlayerIndex", m_nPlayerIndex);
	JSONHelper::AddField(&document, &document, "Start_X", m_vec3Start.x);
	JSONHelper::AddField(&document, &document, "Start_Y", m_vec3Start.y);
	JSONHelper::AddField(&document, &document, "Start_Z", m_vec3Start.z);
	JSONHelper::AddField(&document, &document, "Dest_X", m_vec3Dest.x);
	JSONHelper::AddField(&document, &document, "Dest_Y", m_vec3Dest.y);
	JSONHelper::AddField(&document, &document, "Dest_Z", m_vec3Dest.z);
	JSONHelper::AddField(&document, &document, "State", m_nState);

	m_buffer->Clear();
	document.Accept(*m_writer);

	return m_buffer->GetString();
}

bool GameEventTeleportToR::Deserialize(const char* pChar)
{
	Document document;
	document.Parse<0>(pChar);
	if (!document.IsObject())
	{
		return false;
	}

	if (!JSONHelper::GetField(&document, "PlayerIndex", &m_nPlayerIndex)) return false;
	if (!JSONHelper::GetField(&document, "Start_X", &m_vec3Start.x)) return false;
	if (!JSONHelper::GetField(&document, "Start_Y", &m_vec3Start.y)) return false;
	if (!JSONHelper::GetField(&document, "Start_Z", &m_vec3Start.z)) return false;
	if (!JSONHelper::GetField(&document, "Dest_X", &m_vec3Dest.x)) return false;
	if (!JSONHelper::GetField(&document, "Dest_Y", &m_vec3Dest.y)) return false;
	if (!JSONHelper::GetField(&document, "Dest_Z", &m_vec3Dest.z)) return false;
	if (!JSONHelper::GetField(&document, "State", &m_nState)) return false;

	return true;
}