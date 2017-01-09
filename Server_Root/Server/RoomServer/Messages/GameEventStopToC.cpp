#include "stdafx.h"
#include "GameEventStopToC.h"
#include "GameEventStopToC_Data_generated.h"

GameEventStopToC::GameEventStopToC() : m_Builder(1024)
{
}

GameEventStopToC::~GameEventStopToC()
{
}

unsigned short GameEventStopToC::GetID()
{
	return MESSAGE_ID;
}

IMessage* GameEventStopToC::Clone()
{
	return NULL;
}

const char* GameEventStopToC::Serialize(int* pLength)
{
	FBSData::Vector3 pos(m_vec3Pos.x, m_vec3Pos.y, m_vec3Pos.z);

	GameEventStopToC_DataBuilder data_builder(m_Builder);
	data_builder.add_PlayerIndex(m_nPlayerIndex);
	data_builder.add_EventTime(m_lEventTime);
	data_builder.add_Pos(&pos);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool GameEventStopToC::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<GameEventStopToC_Data>((const void*)pChar);

	m_nPlayerIndex = data->PlayerIndex();
	m_lEventTime = data->EventTime();
	m_vec3Pos.x = data->Pos()->x();
	m_vec3Pos.y = data->Pos()->y();
	m_vec3Pos.z = data->Pos()->z();

	return true;
}