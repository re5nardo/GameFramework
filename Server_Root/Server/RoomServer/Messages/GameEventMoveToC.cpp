#include "stdafx.h"
#include "GameEventMoveToC.h"
#include "GameEventMoveToC_Data_generated.h"

GameEventMoveToC::GameEventMoveToC() : m_Builder(1024)
{
}

GameEventMoveToC::~GameEventMoveToC()
{
}

unsigned short GameEventMoveToC::GetID()
{
	return MESSAGE_ID;
}

IMessage* GameEventMoveToC::Clone()
{
	return NULL;
}

const char* GameEventMoveToC::Serialize(int* pLength)
{
	FBSData::Vector3 dest(m_vec3Dest.x, m_vec3Dest.y, m_vec3Dest.z);

	GameEventMoveToC_DataBuilder data_builder(m_Builder);
	data_builder.add_PlayerIndex(m_nPlayerIndex);
	data_builder.add_EventTime(m_lEventTime);
	data_builder.add_Dest(&dest);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool GameEventMoveToC::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<GameEventMoveToC_Data>((const void*)pChar);

	m_nPlayerIndex = data->PlayerIndex();
	m_lEventTime = data->EventTime();
	m_vec3Dest.x = data->Dest()->x();
	m_vec3Dest.y = data->Dest()->y();
	m_vec3Dest.z = data->Dest()->z();

	return true;
}