#include "stdafx.h"
#include "GameEvent_Move_ToS.h"
#include "NetworkDefines.h"
#include "JSONHelper.h"


GameEvent_Move_ToS::GameEvent_Move_ToS()
{
	m_buffer = new GenericStringBuffer<UTF8<>>();
	m_writer = new Writer<StringBuffer, UTF8<>>(*m_buffer);
}

GameEvent_Move_ToS::~GameEvent_Move_ToS()
{
	delete m_buffer;
	delete m_writer;
}

unsigned short GameEvent_Move_ToS::GetID()
{
	return (unsigned short)Messages::Game_Event_Move_ToS;
}

const char* GameEvent_Move_ToS::Serialize()
{
	Document document;
	document.SetObject();

	m_buffer->Clear();
	document.Accept(*m_writer);

	return m_buffer->GetString();
}

bool GameEvent_Move_ToS::Deserialize(const char* pChar)
{
	Document document;
	document.Parse<0>(pChar);
	if (!document.IsObject())
	{
		return false;
	}

	return true;
}