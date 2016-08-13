#include "stdafx.h"
#include "GameEventMoveToS.h"
#include "NetworkDefines.h"
#include "JSONHelper.h"


GameEventMoveToS::GameEventMoveToS()
{
	m_buffer = new GenericStringBuffer<UTF8<>>();
	m_writer = new Writer<StringBuffer, UTF8<>>(*m_buffer);
}

GameEventMoveToS::~GameEventMoveToS()
{
	delete m_buffer;
	delete m_writer;
}

unsigned short GameEventMoveToS::GetID()
{
	return (unsigned short)Messages::GameEventMoveToS_ID;
}

const char* GameEventMoveToS::Serialize()
{
	Document document;
	document.SetObject();

	m_buffer->Clear();
	document.Accept(*m_writer);

	return m_buffer->GetString();
}

bool GameEventMoveToS::Deserialize(const char* pChar)
{
	Document document;
	document.Parse<0>(pChar);
	if (!document.IsObject())
	{
		return false;
	}

	return true;
}