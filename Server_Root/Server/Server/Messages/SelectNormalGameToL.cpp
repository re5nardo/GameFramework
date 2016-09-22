#include "stdafx.h"
#include "SelectNormalGameToL.h"
#include "../../CommonSources/Message/JSONHelper.h"


SelectNormalGameToL::SelectNormalGameToL()
{
	m_buffer = new GenericStringBuffer<UTF8<>>();
	m_writer = new Writer<StringBuffer, UTF8<>>(*m_buffer);
}

SelectNormalGameToL::~SelectNormalGameToL()
{
	delete m_buffer;
	delete m_writer;
}

unsigned short SelectNormalGameToL::GetID()
{
	return MESSAGE_ID;
}

const char* SelectNormalGameToL::Serialize()
{
	Document document;
	document.SetObject();

	m_buffer->Clear();
	document.Accept(*m_writer);

	return m_buffer->GetString();
}

bool SelectNormalGameToL::Deserialize(const char* pChar)
{
	Document document;
	document.Parse<0>(pChar);
	if (!document.IsObject())
	{
		return false;
	}

	return true;
}