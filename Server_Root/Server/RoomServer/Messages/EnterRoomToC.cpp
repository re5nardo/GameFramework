#include "stdafx.h"
#include "EnterRoomToC.h"
#include "../../CommonSources/Message/JSONHelper.h"


EnterRoomToC::EnterRoomToC()
{
	m_buffer = new GenericStringBuffer<UTF8<>>();
	m_writer = new Writer<StringBuffer, UTF8<>>(*m_buffer);
}

EnterRoomToC::~EnterRoomToC()
{
	delete m_buffer;
	delete m_writer;
}

unsigned short EnterRoomToC::GetID()
{
	return MESSAGE_ID;
}

const char* EnterRoomToC::Serialize()
{
	Document document;
	document.SetObject();

	JSONHelper::AddField(&document, "Result", m_nResult);
	JSONHelper::AddField(&document, "PlayerIndex", m_nPlayerIndex);
	JSONHelper::AddField(&document, "Players", m_mapPlayers);

	m_buffer->Clear();
	document.Accept(*m_writer);

	return m_buffer->GetString();
}

bool EnterRoomToC::Deserialize(const char* pChar)
{
	Document document;
	document.Parse<0>(pChar);
	if (!document.IsObject())
	{
		return false;
	}

	if (!JSONHelper::GetField(&document, "Result", &m_nResult)) return false;
	if (!JSONHelper::GetField(&document, "PlayerIndex", &m_nPlayerIndex)) return false;
	if (!JSONHelper::GetField(&document, "Players", &m_mapPlayers)) return false;

	return true;
}