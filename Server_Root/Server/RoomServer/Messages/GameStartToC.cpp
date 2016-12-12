#include "stdafx.h"
#include "GameStartToC.h"
#include "../../CommonSources/Message/JSONHelper.h"


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
	return MESSAGE_ID;
}

IMessage* GameStartToC::Clone()
{
	return NULL;
}

const char* GameStartToC::Serialize()
{
	Document document;
	document.SetObject();

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

	return true;
}