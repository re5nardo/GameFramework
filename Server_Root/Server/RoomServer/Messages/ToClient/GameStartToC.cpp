#include "stdafx.h"
#include "GameStartToC.h"
#include "../../../FBSFiles/GameStartToC_generated.h"

GameStartToC::GameStartToC()
{
}

GameStartToC::~GameStartToC()
{
}

unsigned short GameStartToC::GetID()
{
	return MESSAGE_ID;
}

IMessage* GameStartToC::Clone()
{
	return NULL;
}

const char* GameStartToC::Serialize(int* pLength)
{
	FBS::GameStartToCBuilder data_builder(m_Builder);
	data_builder.add_TickInterval(m_fTickInterval);
	data_builder.add_RandomSeed(m_nRandomSeed);
	data_builder.add_TimeLimit(m_nTimeLimit);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool GameStartToC::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<FBS::GameStartToC>((const void*)pChar);

	m_fTickInterval = data->TickInterval();
	m_nRandomSeed = data->RandomSeed();
	m_nTimeLimit = data->TimeLimit();

	return true;
}