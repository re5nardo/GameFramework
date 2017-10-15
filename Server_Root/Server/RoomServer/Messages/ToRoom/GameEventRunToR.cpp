#include "stdafx.h"
#include "GameEventRunToR.h"
#include "../../../FBSFiles/GameEventRunToR_generated.h"

GameEventRunToR::GameEventRunToR()
{
}

GameEventRunToR::~GameEventRunToR()
{
}

unsigned short GameEventRunToR::GetID()
{
	return MESSAGE_ID;
}

IMessage* GameEventRunToR::Clone()
{
	GameEventRunToR* pClone = new GameEventRunToR();

	pClone->m_nPlayerIndex = m_nPlayerIndex;
	pClone->m_vec3Dest = m_vec3Dest;

	return pClone;
}

const char* GameEventRunToR::Serialize(int* pLength)
{
	FBS::Data::Vector3 dest(m_vec3Dest.x(), m_vec3Dest.y(), m_vec3Dest.z());

	FBS::GameEventRunToRBuilder data_builder(m_Builder);
	data_builder.add_PlayerIndex(m_nPlayerIndex);
	data_builder.add_Dest(&dest);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool GameEventRunToR::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<FBS::GameEventRunToR>((const void*)pChar);

	m_nPlayerIndex = data->PlayerIndex();
	m_vec3Dest.setX(data->Dest()->x());
	m_vec3Dest.setY(data->Dest()->y());
	m_vec3Dest.setZ(data->Dest()->z());

	return true;
}