#include "stdafx.h"
#include "GameEventIdleToR.h"
#include "GameEventIdleToR_Data_generated.h"

GameEventIdleToR::GameEventIdleToR() : m_Builder(1024)
{
}

GameEventIdleToR::~GameEventIdleToR()
{
}

unsigned short GameEventIdleToR::GetID()
{
	return MESSAGE_ID;
}

IMessage* GameEventIdleToR::Clone()
{
	GameEventIdleToR* pClone = new GameEventIdleToR();

	pClone->m_nPlayerIndex = m_nPlayerIndex;
	pClone->m_vec3Pos = m_vec3Pos;

	return pClone;
}

const char* GameEventIdleToR::Serialize(int* pLength)
{
	FBSData::Vector3 pos(m_vec3Pos.x, m_vec3Pos.y, m_vec3Pos.z);

	GameEventIdleToR_DataBuilder data_builder(m_Builder);
	data_builder.add_PlayerIndex(m_nPlayerIndex);
	data_builder.add_Pos(&pos);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool GameEventIdleToR::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<GameEventIdleToR_Data>((const void*)pChar);

	m_nPlayerIndex = data->PlayerIndex();
	m_vec3Pos.x = data->Pos()->x();
	m_vec3Pos.y = data->Pos()->y();
	m_vec3Pos.z = data->Pos()->z();

	return true;
}