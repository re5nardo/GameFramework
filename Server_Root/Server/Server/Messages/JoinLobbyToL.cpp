#include "stdafx.h"
#include "JoinLobbyToL.h"
#include "../../CommonSources/Message/JSONHelper.h"


JoinLobbyToL::JoinLobbyToL()
{
	m_buffer = new GenericStringBuffer<UTF8<>>();
	m_writer = new Writer<StringBuffer, UTF8<>>(*m_buffer);
}

JoinLobbyToL::~JoinLobbyToL()
{
	delete m_buffer;
	delete m_writer;
}

unsigned short JoinLobbyToL::GetID()
{
	return MESSAGE_ID;
}

IMessage* JoinLobbyToL::Clone()
{
	return NULL;
}

const char* JoinLobbyToL::Serialize()
{
	Document document;
	document.SetObject();

	JSONHelper::AddField(&document, "PlayerKey", m_strPlayerKey);
	JSONHelper::AddField(&document, "AuthKey", m_nAuthKey);

	m_buffer->Clear();
	document.Accept(*m_writer);

	return m_buffer->GetString();
}

bool JoinLobbyToL::Deserialize(const char* pChar)
{
	Document document;
	document.Parse<0>(pChar);
	if (!document.IsObject())
	{
		return false;
	}

	if (!JSONHelper::GetField(&document, "PlayerKey", &m_strPlayerKey)) return false;
	if (!JSONHelper::GetField(&document, "AuthKey", &m_nAuthKey)) return false;

	return true;
}