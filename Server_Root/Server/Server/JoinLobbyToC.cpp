#include "stdafx.h"
#include "JoinLobbyToC.h"
#include "NetworkDefines.h"
#include "JSONHelper.h"


JoinLobbyToC::JoinLobbyToC()
{
	m_buffer = new GenericStringBuffer<UTF8<>>();
	m_writer = new Writer<StringBuffer, UTF8<>>(*m_buffer);
}

JoinLobbyToC::~JoinLobbyToC()
{
	delete m_buffer;
	delete m_writer;
}

unsigned short JoinLobbyToC::GetID()
{
	return (unsigned short)Messages::Join_Lobby_ToC;
}

const char* JoinLobbyToC::Serialize()
{
	Document document;
	document.SetObject();

	JSONHelper::AddField(&document, "Result", m_nResult);

	document.Accept(*m_writer);

	return m_buffer->GetString();
}

bool JoinLobbyToC::Deserialize(const char* pChar)
{
	Document document;
	document.Parse<0>(pChar);
	if (!document.IsObject())
	{
		return false;
	}

	if (!JSONHelper::GetField(&document, "Result", &m_nResult)) return false;

	return true;
}