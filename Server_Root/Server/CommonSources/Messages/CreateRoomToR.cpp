#include "stdafx.h"
#include "CreateRoomToR.h"
#include "../../CommonSources/Message/JSONHelper.h"


CreateRoomToR::CreateRoomToR()
{
	m_buffer = new GenericStringBuffer<UTF8<>>();
	m_writer = new Writer<StringBuffer, UTF8<>>(*m_buffer);
}

CreateRoomToR::~CreateRoomToR()
{
	delete m_buffer;
	delete m_writer;
}

unsigned short CreateRoomToR::GetID()
{
	return MESSAGE_ID;
}

IMessage* CreateRoomToR::Clone()
{
	return NULL;
}

const char* CreateRoomToR::Serialize()
{
	Document document;
	document.SetObject();

	JSONHelper::AddField(&document, "MatchID", m_nMatchID);
	JSONHelper::AddField(&document, "Players", m_vecPlayers);

	m_buffer->Clear();
	document.Accept(*m_writer);

	return m_buffer->GetString();
}

bool CreateRoomToR::Deserialize(const char* pChar)
{
	Document document;
	document.Parse<0>(pChar);
	if (!document.IsObject())
	{
		return false;
	}

	if (!JSONHelper::GetField(&document, "MatchID", &m_nMatchID)) return false;
	if (!JSONHelper::GetField(&document, "Players", &m_vecPlayers)) return false;

	return true;
}