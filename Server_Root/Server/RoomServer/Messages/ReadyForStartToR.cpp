#include "stdafx.h"
#include "ReadyForStartToR.h"
#include "../../CommonSources/Message/JSONHelper.h"


ReadyForStartToR::ReadyForStartToR()
{
	m_buffer = new GenericStringBuffer<UTF8<>>();
	m_writer = new Writer<StringBuffer, UTF8<>>(*m_buffer);
}

ReadyForStartToR::~ReadyForStartToR()
{
	delete m_buffer;
	delete m_writer;
}

unsigned short ReadyForStartToR::GetID()
{
	return MESSAGE_ID;
}

IMessage* ReadyForStartToR::Clone()
{
	return NULL;
}

const char* ReadyForStartToR::Serialize()
{
	Document document;
	document.SetObject();

	JSONHelper::AddField(&document, &document, "PlayerIndex", m_nPlayerIndex);

	m_buffer->Clear();
	document.Accept(*m_writer);

	return m_buffer->GetString();
}

bool ReadyForStartToR::Deserialize(const char* pChar)
{
	Document document;
	document.Parse<0>(pChar);
	if (!document.IsObject())
	{
		return false;
	}

	if (!JSONHelper::GetField(&document, "PlayerIndex", &m_nPlayerIndex)) return false;

	return true;
}