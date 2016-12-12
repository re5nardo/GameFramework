#include "stdafx.h"
#include "PlayerEnterRoomToC.h"
#include "../../CommonSources/Message/JSONHelper.h"


PlayerEnterRoomToC::PlayerEnterRoomToC()
{
	m_buffer = new GenericStringBuffer<UTF8<>>();
	m_writer = new Writer<StringBuffer, UTF8<>>(*m_buffer);
}

PlayerEnterRoomToC::~PlayerEnterRoomToC()
{
	delete m_buffer;
	delete m_writer;
}

unsigned short PlayerEnterRoomToC::GetID()
{
	return MESSAGE_ID;
}

IMessage* PlayerEnterRoomToC::Clone()
{
	return NULL;
}

const char* PlayerEnterRoomToC::Serialize()
{
	Document document;
	document.SetObject();

	JSONHelper::AddField(&document, "PlayerIndex", m_nPlayerIndex);
	JSONHelper::AddField(&document, "CharacterID", m_strCharacterID);

	m_buffer->Clear();
	document.Accept(*m_writer);

	return m_buffer->GetString();
}

bool PlayerEnterRoomToC::Deserialize(const char* pChar)
{
	Document document;
	document.Parse<0>(pChar);
	if (!document.IsObject())
	{
		return false;
	}

	if (!JSONHelper::GetField(&document, "PlayerIndex", &m_nPlayerIndex)) return false;
	if (!JSONHelper::GetField(&document, "CharacterID", &m_strCharacterID)) return false;

	return true;
}