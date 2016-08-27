#include "stdafx.h"
#include "SelectNormalGameToS.h"
#include "LobbyMessageDefines.h"
#include "JSONHelper.h"


SelectNormalGameToS::SelectNormalGameToS()
{
	m_buffer = new GenericStringBuffer<UTF8<>>();
	m_writer = new Writer<StringBuffer, UTF8<>>(*m_buffer);
}

SelectNormalGameToS::~SelectNormalGameToS()
{
	delete m_buffer;
	delete m_writer;
}

unsigned short SelectNormalGameToS::GetID()
{
	return (unsigned short)Messages::SelectNormalGameToS_ID;
}

const char* SelectNormalGameToS::Serialize()
{
	Document document;
	document.SetObject();

	m_buffer->Clear();
	document.Accept(*m_writer);

	return m_buffer->GetString();
}

bool SelectNormalGameToS::Deserialize(const char* pChar)
{
	Document document;
	document.Parse<0>(pChar);
	if (!document.IsObject())
	{
		return false;
	}

	return true;
}