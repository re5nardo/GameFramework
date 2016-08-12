#include "stdafx.h"
#include "GameStartToC.h"
#include "NetworkDefines.h"
#include "JSONHelper.h"


GameStartToC::GameStartToC()
{
	m_buffer = new GenericStringBuffer<UTF8<>>();
	m_writer = new Writer<StringBuffer, UTF8<>>(*m_buffer);
}

GameStartToC::~GameStartToC()
{
	delete m_buffer;
	delete m_writer;
}

unsigned short GameStartToC::GetID()
{
	return (unsigned short)Messages::Game_Start_ToC;
}

const char* GameStartToC::Serialize()
{
	Document document;
	document.SetObject();

	JSONHelper::AddField(&document, "GameElapsedTime", m_lGameElapsedTime);

	m_buffer->Clear();
	document.Accept(*m_writer);

	return m_buffer->GetString();
}

bool GameStartToC::Deserialize(const char* pChar)
{
	Document document;
	document.Parse<0>(pChar);
	if (!document.IsObject())
	{
		return false;
	}

	if (!JSONHelper::GetField(&document, "GameElapsedTime", &m_lGameElapsedTime)) return false;

	return true;
}