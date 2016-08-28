#include "stdafx.h"
#include "ReadyForStartToS.h"
#include "RoomMessageDefines.h"
#include "../../CommonSources/Message/JSONHelper.h"


ReadyForStartToS::ReadyForStartToS()
{
	m_buffer = new GenericStringBuffer<UTF8<>>();
	m_writer = new Writer<StringBuffer, UTF8<>>(*m_buffer);
}

ReadyForStartToS::~ReadyForStartToS()
{
	delete m_buffer;
	delete m_writer;
}

unsigned short ReadyForStartToS::GetID()
{
	return (unsigned short)Messages::ReadyForStartToS_ID;
}

const char* ReadyForStartToS::Serialize()
{
	Document document;
	document.SetObject();

	JSONHelper::AddField(&document, "PlayerIndex", m_nPlayerIndex);

	m_buffer->Clear();
	document.Accept(*m_writer);

	return m_buffer->GetString();
}

bool ReadyForStartToS::Deserialize(const char* pChar)
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