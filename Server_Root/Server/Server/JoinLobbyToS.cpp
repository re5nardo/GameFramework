#include "stdafx.h"
#include "JoinLobbyToS.h"
#include "NetworkDefines.h"
#include "JSONHelper.h"


JoinLobbyToS::JoinLobbyToS()
{
	m_buffer = new GenericStringBuffer<UTF8<>>();
	m_writer = new Writer<StringBuffer, UTF8<>>(*m_buffer);
}

JoinLobbyToS::~JoinLobbyToS()
{
	delete m_buffer;
	delete m_writer;
}

unsigned short JoinLobbyToS::GetID()
{
	return (unsigned short)Messages::JoinLobbyToS_ID;
}

const char* JoinLobbyToS::Serialize()
{
	Document document;
	document.SetObject();

	JSONHelper::AddField(&document, "PlayerKey", m_strPlayerKey);
	JSONHelper::AddField(&document, "AuthKey", m_nAuthKey);

	m_buffer->Clear();
	document.Accept(*m_writer);

	return m_buffer->GetString();
}

bool JoinLobbyToS::Deserialize(const char* pChar)
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