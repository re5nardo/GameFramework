#include "stdafx.h"
#include "GameEventTeleportToR.h"
#include "GameEventTeleportToR_Data_generated.h"

GameEventTeleportToR::GameEventTeleportToR() : m_Builder(1024)
{
}

GameEventTeleportToR::~GameEventTeleportToR()
{
}

unsigned short GameEventTeleportToR::GetID()
{
	return MESSAGE_ID;
}

IMessage* GameEventTeleportToR::Clone()
{
	return NULL;
}

const char* GameEventTeleportToR::Serialize(int* pLength)
{
	FBSData::Vector3 start(m_vec3Start.x, m_vec3Start.y, m_vec3Start.z);
	FBSData::Vector3 dest(m_vec3Dest.x, m_vec3Dest.y, m_vec3Dest.z);

	GameEventTeleportToR_DataBuilder data_builder(m_Builder);
	data_builder.add_PlayerIndex(m_nPlayerIndex);
	data_builder.add_Start(&start);
	data_builder.add_Dest(&dest);
	data_builder.add_State(m_nState);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool GameEventTeleportToR::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<GameEventTeleportToR_Data>((const void*)pChar);

	m_nPlayerIndex = data->PlayerIndex();
	m_vec3Start.x = data->Start()->x();
	m_vec3Start.y = data->Start()->y();
	m_vec3Start.z = data->Start()->z();
	m_vec3Dest.x = data->Dest()->x();
	m_vec3Dest.y = data->Dest()->y();
	m_vec3Dest.z = data->Dest()->z();
	m_nState = data->State();

	return true;
}