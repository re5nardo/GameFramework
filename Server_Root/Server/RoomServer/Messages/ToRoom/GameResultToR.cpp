#include "stdafx.h"
#include "GameResultToR.h"

GameResultToR::GameResultToR()
{
}

GameResultToR::~GameResultToR()
{
}

unsigned short GameResultToR::GetID()
{
	return MESSAGE_ID;
}

IMessage* GameResultToR::Clone()
{
	GameResultToR* pClone = new GameResultToR();

	for (int i = 0; i < m_vecPlayerRankInfo.size(); ++i)
	{
		pClone->m_vecPlayerRankInfo.push_back(FBS::Data::PlayerRankInfo(m_vecPlayerRankInfo[i].PlayerIndex(), m_vecPlayerRankInfo[i].Rank(), m_vecPlayerRankInfo[i].Height()));
	}

	return pClone;
}

const char* GameResultToR::Serialize(int* pLength)
{
	auto playerRankInfos = m_Builder.CreateVectorOfStructs(m_vecPlayerRankInfo);

	FBS::GameResultToRBuilder data_builder(m_Builder);
	data_builder.add_PlayerRanks(playerRankInfos);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool GameResultToR::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<FBS::GameResultToR>((const void*)pChar);

	for (int i = 0; i < data->PlayerRanks()->size(); ++i)
	{
		auto playerRank = data->PlayerRanks()->Get(i);
		m_vecPlayerRankInfo.push_back(FBS::Data::PlayerRankInfo(playerRank->PlayerIndex(), playerRank->Rank(), playerRank->Height()));
	}

	return true;
}