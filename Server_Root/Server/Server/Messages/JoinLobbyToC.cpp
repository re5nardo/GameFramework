#include "stdafx.h"
#include "JoinLobbyToC.h"
#include "../../CommonSources/Message/JSONHelper.h"


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
	return MESSAGE_ID;
}

IMessage* JoinLobbyToC::Clone()
{
	return NULL;
}

const char* JoinLobbyToC::Serialize()
{
	Document document;
	document.SetObject();

	JSONHelper::AddField(&document, &document, "Result", m_nResult);

	m_buffer->Clear();
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