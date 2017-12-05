#include "stdafx.h"
#include "GameInputMoveToR.h"

GameInputMoveToR::GameInputMoveToR()
{
}

GameInputMoveToR::~GameInputMoveToR()
{
}

unsigned short GameInputMoveToR::GetID()
{
	return MESSAGE_ID;
}

IMessage* GameInputMoveToR::Clone()
{
	GameInputMoveToR* pClone = new GameInputMoveToR();

	pClone->m_nPlayerIndex = m_nPlayerIndex;
	pClone->m_Direction = m_Direction;

	return pClone;
}

const char* GameInputMoveToR::Serialize(int* pLength)
{
	FBS::GameInputMoveToRBuilder data_builder(m_Builder);
	data_builder.add_PlayerIndex(m_nPlayerIndex);
	data_builder.add_Direction(m_Direction);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool GameInputMoveToR::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<FBS::GameInputMoveToR>((const void*)pChar);

	m_nPlayerIndex = data->PlayerIndex();
	m_Direction = data->Direction();

	return true;
}