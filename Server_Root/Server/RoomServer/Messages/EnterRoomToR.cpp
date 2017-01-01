#include "stdafx.h"
#include "EnterRoomToR.h"
#include "../../CommonSources/Message/JSONHelper.h"


EnterRoomToR::EnterRoomToR()
{
	m_buffer = new GenericStringBuffer<UTF8<>>();
	m_writer = new Writer<StringBuffer, UTF8<>>(*m_buffer);
}

EnterRoomToR::~EnterRoomToR()
{
	delete m_buffer;
	delete m_writer;
}

unsigned short EnterRoomToR::GetID()
{
	return MESSAGE_ID;
}

IMessage* EnterRoomToR::Clone()
{
	return NULL;
}

const char* EnterRoomToR::Serialize()
{
	Document document;
	document.SetObject();

	JSONHelper::AddField(&document, &document, "PlayerKey", m_strPlayerKey);
	JSONHelper::AddField(&document, &document, "AuthKey", m_nAuthKey);
	JSONHelper::AddField(&document, &document, "MatchID", m_nMatchID);

	m_buffer->Clear();
	document.Accept(*m_writer);

	return m_buffer->GetString();
}

bool EnterRoomToR::Deserialize(const char* pChar)
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