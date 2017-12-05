#include "stdafx.h"
#include "GameInputRotationToR.h"
#include "../../../FBSFiles/GameInputRotationToR_generated.h"

GameInputRotationToR::GameInputRotationToR()
{
}

GameInputRotationToR::~GameInputRotationToR()
{
}

unsigned short GameInputRotationToR::GetID()
{
	return MESSAGE_ID;
}

IMessage* GameInputRotationToR::Clone()
{
	GameInputRotationToR* pClone = new GameInputRotationToR();

	pClone->m_nPlayerIndex = m_nPlayerIndex;
	pClone->m_Rotation = m_Rotation;

	return pClone;
}

const char* GameInputRotationToR::Serialize(int* pLength)
{
	FBS::Data::Vector3 rotation(m_Rotation.x(), m_Rotation.y(), m_Rotation.z());

	FBS::GameInputRotationToRBuilder data_builder(m_Builder);
	data_builder.add_PlayerIndex(m_nPlayerIndex);
	data_builder.add_Rotation(&rotation);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool GameInputRotationToR::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<FBS::GameInputRotationToR>((const void*)pChar);

	m_nPlayerIndex = data->PlayerIndex();
	m_Rotation.setValue(data->Rotation()->x(), data->Rotation()->y(), data->Rotation()->z());

	return true;
}