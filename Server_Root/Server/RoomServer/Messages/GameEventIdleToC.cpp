#include "stdafx.h"
#include "GameEventIdleToC.h"
#include "GameEventIdleToC_Data_generated.h"

GameEventIdleToC::GameEventIdleToC() : m_Builder(1024)
{
}

GameEventIdleToC::~GameEventIdleToC()
{
}

unsigned short GameEventIdleToC::GetID()
{
	return MESSAGE_ID;
}

IMessage* GameEventIdleToC::Clone()
{
	return NULL;
}

const char* GameEventIdleToC::Serialize(int* pLength)
{
	FBSData::Vector3 pos(m_vec3Pos.x, m_vec3Pos.y, m_vec3Pos.z);

	GameEventIdleToC_DataBuilder data_builder(m_Builder);
	data_builder.add_PlayerIndex(m_nPlayerIndex);
	data_builder.add_EventTime(m_lEventTime);
	data_builder.add_Pos(&pos);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool GameEventIdleToC::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<GameEventIdleToC_Data>((const void*)pChar);

	m_nPlayerIndex = data->PlayerIndex();
	m_lEventTime = data->EventTime();
	m_vec3Pos.x = data->Pos()->x();
	m_vec3Pos.y = data->Pos()->y();
	m_vec3Pos.z = data->Pos()->z();

	return true;
}