#include "stdafx.h"
#include "SelectNormalGameToC.h"
#include "../../CommonSources/Message/JSONHelper.h"


SelectNormalGameToC::SelectNormalGameToC()
{
	m_buffer = new GenericStringBuffer<UTF8<>>();
	m_writer = new Writer<StringBuffer, UTF8<>>(*m_buffer);
}

SelectNormalGameToC::~SelectNormalGameToC()
{
	delete m_buffer;
	delete m_writer;
}

unsigned short SelectNormalGameToC::GetID()
{
	return MESSAGE_ID;
}

IMessage* SelectNormalGameToC::Clone()
{
	return NULL;
}

const char* SelectNormalGameToC::Serialize()
{
	Document document;
	document.SetObject();

	JSONHelper::AddField(&document, "Result", m_nResult);
	JSONHelper::AddField(&document, "ExpectedTime", m_nExpectedTime);

	m_buffer->Clear();
	document.Accept(*m_writer);

	return m_buffer->GetString();
}

bool SelectNormalGameToC::Deserialize(const char* pChar)
{
	Document document;
	document.Parse<0>(pChar);
	if (!document.IsObject())
	{
		return false;
	}

	if (!JSONHelper::GetField(&document, "Result", &m_nResult)) return false;
	if (!JSONHelper::GetField(&document, "ExpectedTime", &m_nExpectedTime)) return false;

	return true;
}