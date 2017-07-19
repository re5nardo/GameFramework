#include "stdafx.h"
#include "JoinLobbyToL.h"
#include "../../../FBSFiles/JoinLobbyToL_generated.h"

JoinLobbyToL::JoinLobbyToL()
{
}

JoinLobbyToL::~JoinLobbyToL()
{
}

unsigned short JoinLobbyToL::GetID()
{
	return MESSAGE_ID;
}

IMessage* JoinLobbyToL::Clone()
{
	return NULL;
}

const char* JoinLobbyToL::Serialize(int* pLength)
{
	auto playerKey = m_Builder.CreateString(m_strPlayerKey);

	FBS::JoinLobbyToLBuilder data_builder(m_Builder);
	data_builder.add_PlayerKey(playerKey);
	data_builder.add_AuthKey(m_nAuthKey);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool JoinLobbyToL::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<FBS::JoinLobbyToL>((const void*)pChar);

	m_strPlayerKey = data->PlayerKey()->str();
	m_nAuthKey = data->AuthKey();

	return true;
}