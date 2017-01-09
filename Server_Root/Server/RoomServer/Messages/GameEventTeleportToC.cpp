#include "stdafx.h"
#include "GameEventTeleportToC.h"
#include "GameEventTeleportToC_Data_generated.h"

GameEventTeleportToC::GameEventTeleportToC() : m_Builder(1024)
{
}

GameEventTeleportToC::~GameEventTeleportToC()
{
}

unsigned short GameEventTeleportToC::GetID()
{
	return MESSAGE_ID;
}

IMessage* GameEventTeleportToC::Clone()
{
	return NULL;
}

const char* GameEventTeleportToC::Serialize(int* pLength)
{
	FBSData::Vector3 start(m_vec3Start.x, m_vec3Start.y, m_vec3Start.z);
	FBSData::Vector3 dest(m_vec3Dest.x, m_vec3Dest.y, m_vec3Dest.z);

	GameEventTeleportToC_DataBuilder data_builder(m_Builder);
	data_builder.add_PlayerIndex(m_nPlayerIndex);
	data_builder.add_EventTime(m_lEventTime);
	data_builder.add_Start(&start);
	data_builder.add_Dest(&dest);
	data_builder.add_State(m_nState);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool GameEventTeleportToC::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<GameEventTeleportToC_Data>((const void*)pChar);

	m_nPlayerIndex = data->PlayerIndex();
	m_lEventTime = data->EventTime();
	m_vec3Start.x = data->Start()->x();
	m_vec3Start.y = data->Start()->y();
	m_vec3Start.z = data->Start()->z();
	m_vec3Dest.x = data->Dest()->x();
	m_vec3Dest.y = data->Dest()->y();
	m_vec3Dest.z = data->Dest()->z();
	m_nState = data->State();

	return true;
}