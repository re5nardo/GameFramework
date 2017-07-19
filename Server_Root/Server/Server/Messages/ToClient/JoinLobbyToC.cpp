#include "stdafx.h"
#include "JoinLobbyToC.h"
#include "../../../FBSFiles/JoinLobbyToC_generated.h"

JoinLobbyToC::JoinLobbyToC()
{
}

JoinLobbyToC::~JoinLobbyToC()
{
}

unsigned short JoinLobbyToC::GetID()
{
	return MESSAGE_ID;
}

IMessage* JoinLobbyToC::Clone()
{
	return NULL;
}

const char* JoinLobbyToC::Serialize(int* pLength)
{
	FBS::JoinLobbyToCBuilder data_builder(m_Builder);
	data_builder.add_Result(m_nResult);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool JoinLobbyToC::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<FBS::JoinLobbyToC>((const void*)pChar);

	m_nResult = data->Result();

	return true;
}