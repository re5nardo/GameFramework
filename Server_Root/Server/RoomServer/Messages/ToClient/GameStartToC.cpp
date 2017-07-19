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
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool GameStartToC::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<FBS::GameStartToC>((const void*)pChar);

	return true;
}