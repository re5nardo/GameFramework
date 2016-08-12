#include "stdafx.h"
#include "GameEvent_Move_ToC.h"
#include "NetworkDefines.h"
#include "JSONHelper.h"


GameEvent_Move_ToC::GameEvent_Move_ToC()
{
	m_buffer = new GenericStringBuffer<UTF8<>>();
	m_writer = new Writer<StringBuffer, UTF8<>>(*m_buffer);
}

GameEvent_Move_ToC::~GameEvent_Move_ToC()
{
	delete m_buffer;
	delete m_writer;
}

unsigned short GameEvent_Move_ToC::GetID()
{
	return (unsigned short)Messages::Game_Event_Move_ToC;
}

const char* GameEvent_Move_ToC::Serialize()
{
	Document document;
	document.SetObject();

	m_buffer->Clear();
	document.Accept(*m_writer);

	return m_buffer->GetString();
}

bool GameEvent_Move_ToC::Deserialize(const char* pChar)
{
	Document document;
	document.Parse<0>(pChar);
	if (!document.IsObject())
	{
		return false;
	}

	return true;
}