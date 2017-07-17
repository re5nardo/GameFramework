#include "stdafx.h"
#include "GameEventMoveToR.h"
#include "GameEventMoveToR_Data_generated.h"

GameEventMoveToR::GameEventMoveToR()
{
}

GameEventMoveToR::~GameEventMoveToR()
{
}

unsigned short GameEventMoveToR::GetID()
{
	return MESSAGE_ID;
}

IMessage* GameEventMoveToR::Clone()
{
	GameEventMoveToR* pClone = new GameEventMoveToR();

	pClone->m_nPlayerIndex = m_nPlayerIndex;
	pClone->m_vec3Dest = m_vec3Dest;

	return pClone;
}

const char* GameEventMoveToR::Serialize(int* pLength)
{
	FBSData::Vector3 dest(m_vec3Dest.x(), m_vec3Dest.y(), m_vec3Dest.z());

	GameEventMoveToR_DataBuilder data_builder(m_Builder);
	data_builder.add_PlayerIndex(m_nPlayerIndex);
	data_builder.add_Dest(&dest);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool GameEventMoveToR::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<GameEventMoveToR_Data>((const void*)pChar);

	m_nPlayerIndex = data->PlayerIndex();
	m_vec3Dest.setX(data->Dest()->x());
	m_vec3Dest.setY(data->Dest()->y());
	m_vec3Dest.setZ(data->Dest()->z());

	return true;
}