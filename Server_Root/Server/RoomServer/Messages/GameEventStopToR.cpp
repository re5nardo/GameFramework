#include "stdafx.h"
#include "GameEventStopToR.h"
#include "GameEventStopToR_Data_generated.h"

GameEventStopToR::GameEventStopToR()
{
}

GameEventStopToR::~GameEventStopToR()
{
}

unsigned short GameEventStopToR::GetID()
{
	return MESSAGE_ID;
}

IMessage* GameEventStopToR::Clone()
{
	GameEventStopToR* pClone = new GameEventStopToR();

	pClone->m_nPlayerIndex = m_nPlayerIndex;
	pClone->m_vec3Pos = m_vec3Pos;

	return pClone;
}

const char* GameEventStopToR::Serialize(int* pLength)
{
	FBSData::Vector3 pos(m_vec3Pos.x(), m_vec3Pos.y(), m_vec3Pos.z());

	GameEventStopToR_DataBuilder data_builder(m_Builder);
	data_builder.add_PlayerIndex(m_nPlayerIndex);
	data_builder.add_Pos(&pos);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool GameEventStopToR::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<GameEventStopToR_Data>((const void*)pChar);

	m_nPlayerIndex = data->PlayerIndex();
	m_vec3Pos.setX(data->Pos()->x());
	m_vec3Pos.setY(data->Pos()->y());
	m_vec3Pos.setZ(data->Pos()->z());

	return true;
}