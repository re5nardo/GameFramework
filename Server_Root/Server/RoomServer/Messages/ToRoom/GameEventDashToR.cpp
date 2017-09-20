#include "stdafx.h"
#include "GameEventDashToR.h"
#include "../../../FBSFiles/GameEventDashToR_generated.h"

GameEventDashToR::GameEventDashToR()
{
}

GameEventDashToR::~GameEventDashToR()
{
}

unsigned short GameEventDashToR::GetID()
{
	return MESSAGE_ID;
}

IMessage* GameEventDashToR::Clone()
{
	GameEventDashToR* pClone = new GameEventDashToR();

	pClone->m_nPlayerIndex = m_nPlayerIndex;

	return pClone;
}

const char* GameEventDashToR::Serialize(int* pLength)
{
	FBS::GameEventDashToRBuilder data_builder(m_Builder);
	data_builder.add_PlayerIndex(m_nPlayerIndex);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool GameEventDashToR::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<FBS::GameEventDashToR>((const void*)pChar);

	m_nPlayerIndex = data->PlayerIndex();

	return true;
}