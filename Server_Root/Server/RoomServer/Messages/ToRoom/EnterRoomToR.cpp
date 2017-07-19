#include "stdafx.h"
#include "EnterRoomToR.h"
#include "../../../FBSFiles/EnterRoomToR_generated.h"

EnterRoomToR::EnterRoomToR()
{
}

EnterRoomToR::~EnterRoomToR()
{
}

unsigned short EnterRoomToR::GetID()
{
	return MESSAGE_ID;
}

IMessage* EnterRoomToR::Clone()
{
	return NULL;
}

const char* EnterRoomToR::Serialize(int* pLength)
{
	auto playerKey = m_Builder.CreateString(m_strPlayerKey);

	FBS::EnterRoomToRBuilder data_builder(m_Builder);
	data_builder.add_PlayerKey(playerKey);
	data_builder.add_AuthKey(m_nAuthKey);
	data_builder.add_MatchID(m_nMatchID);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool EnterRoomToR::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<FBS::EnterRoomToR>((const void*)pChar);

	m_strPlayerKey = data->PlayerKey()->str();
	m_nAuthKey = data->AuthKey();
	m_nMatchID = data->MatchID();

	return true;
}