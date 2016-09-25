#include "stdafx.h"
#include "PreparationStateToR.h"
#include "../../CommonSources/Message/JSONHelper.h"


PreparationStateToR::PreparationStateToR()
{
	m_buffer = new GenericStringBuffer<UTF8<>>();
	m_writer = new Writer<StringBuffer, UTF8<>>(*m_buffer);
}

PreparationStateToR::~PreparationStateToR()
{
	delete m_buffer;
	delete m_writer;
}

unsigned short PreparationStateToR::GetID()
{
	return MESSAGE_ID;
}

const char* PreparationStateToR::Serialize()
{
	Document document;
	document.SetObject();

	JSONHelper::AddField(&document, "State", m_fState);

	m_buffer->Clear();
	document.Accept(*m_writer);

	return m_buffer->GetString();
}

bool PreparationStateToR::Deserialize(const char* pChar)
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