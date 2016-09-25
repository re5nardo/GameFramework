#include "stdafx.h"
#include "PreparationStateToC.h"
#include "../../CommonSources/Message/JSONHelper.h"


PreparationStateToC::PreparationStateToC()
{
	m_buffer = new GenericStringBuffer<UTF8<>>();
	m_writer = new Writer<StringBuffer, UTF8<>>(*m_buffer);
}

PreparationStateToC::~PreparationStateToC()
{
	delete m_buffer;
	delete m_writer;
}

unsigned short PreparationStateToC::GetID()
{
	return MESSAGE_ID;
}

const char* PreparationStateToC::Serialize()
{
	Document document;
	document.SetObject();

	JSONHelper::AddField(&document, "State", m_fState);

	m_buffer->Clear();
	document.Accept(*m_writer);

	return m_buffer->GetString();
}

bool PreparationStateToC::Deserialize(const char* pChar)
{
	Document document;
	document.Parse<0>(pChar);
	if (!document.IsObject())
	{
		return false;
	}

	if (!JSONHelper::GetField(&document, "State", &m_fState)) return false;

	return true;
}