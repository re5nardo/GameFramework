#include "stdafx.h"
#include "ReadyForStartToS.h"
#include "NetworkDefines.h"
#include "JSONHelper.h"


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
	return (unsigned short)Messages::Ready_For_Start_ToS;
}

const char* ReadyForStartToS::Serialize()
{
	Document document;
	document.SetObject();

	JSONHelper::AddField(&document, "PlayerIndex", m_nPlayerIndex);

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