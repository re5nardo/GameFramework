#include "stdafx.h"
#include "GameInputSkillToR.h"
#include "GameInputSkillToR_Data_generated.h"

GameInputSkillToR::GameInputSkillToR() : m_Builder(1024)
{
}

GameInputSkillToR::~GameInputSkillToR()
{
}

unsigned short GameInputSkillToR::GetID()
{
	return MESSAGE_ID;
}

IMessage* GameInputSkillToR::Clone()
{
	GameInputSkillToR* pClone = new GameInputSkillToR();

	pClone->m_nPlayerIndex = m_nPlayerIndex;
	pClone->m_nSkillID = m_nSkillID;
	pClone->m_vecVector3 = m_vecVector3;
	pClone->m_vecInt = m_vecInt;
	pClone->m_vecFloat = m_vecFloat;

	return pClone;
}

const char* GameInputSkillToR::Serialize(int* pLength)
{
	vector<FBSData::Vector3> vecVector3;
	for (vector<btVector3>::iterator it = m_vecVector3.begin(); it != m_vecVector3.end(); ++it)
	{
		btVector3 vec3 = *it;
		vecVector3.push_back(FBSData::Vector3(vec3.x(), vec3.y(), vec3.z()));
	}

	auto vector3s = m_Builder.CreateVectorOfStructs(vecVector3);
	auto ints = m_Builder.CreateVector(m_vecInt);
	auto floats = m_Builder.CreateVector(m_vecFloat);

	GameInputSkillToR_DataBuilder data_builder(m_Builder);
	data_builder.add_PlayerIndex(m_nPlayerIndex);
	data_builder.add_SkillID(m_nSkillID);
	data_builder.add_Vector3s(vector3s);
	data_builder.add_Ints(ints);
	data_builder.add_Floats(floats);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool GameInputSkillToR::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<GameInputSkillToR_Data>((const void*)pChar);

	m_nPlayerIndex = data->PlayerIndex();
	m_nSkillID = data->SkillID();
	for (int i = 0; i < data->Vector3s()->size(); ++i)
	{
		const FBSData::Vector3* vec3 = data->Vector3s()->Get(i);
		m_vecVector3.push_back(btVector3(vec3->x(), vec3->y(), vec3->z()));
	}
	for (int i = 0; i < data->Ints()->size(); ++i)
	{
		m_vecInt.push_back(data->Ints()->Get(i));
	}
	for (int i = 0; i < data->Floats()->size(); ++i)
	{
		m_vecFloat.push_back(data->Floats()->Get(i));
	}

	return true;
}