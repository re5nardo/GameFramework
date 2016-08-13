#include "stdafx.h"
#include "GameEventMoveToC.h"
#include "NetworkDefines.h"
#include "JSONHelper.h"


GameEventMoveToC::GameEventMoveToC()
{
	m_buffer = new GenericStringBuffer<UTF8<>>();
	m_writer = new Writer<StringBuffer, UTF8<>>(*m_buffer);
}

GameEventMoveToC::~GameEventMoveToC()
{
	delete m_buffer;
	delete m_writer;
}

unsigned short GameEventMoveToC::GetID()
{
	return (unsigned short)Messages::GameEventMoveToC_ID;
}

const char* GameEventMoveToC::Serialize()
{
	Document document;
	document.SetObject();

	m_buffer->Clear();
	document.Accept(*m_writer);

	return m_buffer->GetString();
}

bool GameEventMoveToC::Deserialize(const char* pChar)
{
	Document document;
	document.Parse<0>(pChar);
	if (!document.IsObject())
	{
		return false;
	}

	return true;
}