#include "stdafx.h"
#include "CreateRoomToL.h"
#include "../../CommonSources/Message/JSONHelper.h"


CreateRoomToL::CreateRoomToL()
{
	m_buffer = new GenericStringBuffer<UTF8<>>();
	m_writer = new Writer<StringBuffer, UTF8<>>(*m_buffer);
}

CreateRoomToL::~CreateRoomToL()
{
	delete m_buffer;
	delete m_writer;
}

unsigned short CreateRoomToL::GetID()
{
	return MESSAGE_ID;
}

const char* CreateRoomToL::Serialize()
{
	Document document;
	document.SetObject();

	JSONHelper::AddField(&document, "Result", m_nResult);
	JSONHelper::AddField(&document, "Players", m_vecPlayers);

	m_buffer->Clear();
	document.Accept(*m_writer);

	return m_buffer->GetString();
}

bool CreateRoomToL::Deserialize(const char* pChar)
{
	Document document;
	document.Parse<0>(pChar);
	if (!document.IsObject())
	{
		return false;
	}

	if (!JSONHelper::GetField(&document, "Result", &m_nResult)) return false;
	if (!JSONHelper::GetField(&document, "Players", &m_vecPlayers)) return false;

	return true;
}