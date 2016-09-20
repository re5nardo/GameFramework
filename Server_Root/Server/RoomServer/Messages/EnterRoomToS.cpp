#include "stdafx.h"
#include "EnterRoomToS.h"
#include "../../CommonSources/Message/JSONHelper.h"


EnterRoomToS::EnterRoomToS()
{
	m_buffer = new GenericStringBuffer<UTF8<>>();
	m_writer = new Writer<StringBuffer, UTF8<>>(*m_buffer);
}

EnterRoomToS::~EnterRoomToS()
{
	delete m_buffer;
	delete m_writer;
}

unsigned short EnterRoomToS::GetID()
{
	return MESSAGE_ID;
}

const char* EnterRoomToS::Serialize()
{
	Document document;
	document.SetObject();

	JSONHelper::AddField(&document, "PlayerKey", m_strPlayerKey);
	JSONHelper::AddField(&document, "AuthKey", m_nAuthKey);
	JSONHelper::AddField(&document, "MatchID", m_nMatchID);

	m_buffer->Clear();
	document.Accept(*m_writer);

	return m_buffer->GetString();
}

bool EnterRoomToS::Deserialize(const char* pChar)
{
	Document document;
	document.Parse<0>(pChar);
	if (!document.IsObject())
	{
		return false;
	}

	if (!JSONHelper::GetField(&document, "PlayerKey", &m_strPlayerKey)) return false;
	if (!JSONHelper::GetField(&document, "AuthKey", &m_nAuthKey)) return false;
	if (!JSONHelper::GetField(&document, "MatchID", &m_nMatchID)) return false;

	return true;
}