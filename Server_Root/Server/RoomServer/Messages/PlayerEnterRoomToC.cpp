#include "stdafx.h"
#include "PlayerEnterRoomToC.h"
#include "PlayerEnterRoomToC_Data_generated.h"

PlayerEnterRoomToC::PlayerEnterRoomToC()
{
}

PlayerEnterRoomToC::~PlayerEnterRoomToC()
{
}

unsigned short PlayerEnterRoomToC::GetID()
{
	return MESSAGE_ID;
}

IMessage* PlayerEnterRoomToC::Clone()
{
	return NULL;
}

const char* PlayerEnterRoomToC::Serialize(int* pLength)
{
	auto characterID = m_Builder.CreateString(m_strCharacterID);

	PlayerEnterRoomToC_DataBuilder data_builder(m_Builder);
	data_builder.add_PlayerIndex(m_nPlayerIndex);
	data_builder.add_CharacterID(characterID);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool PlayerEnterRoomToC::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<PlayerEnterRoomToC_Data>((const void*)pChar);

	m_nPlayerIndex = data->PlayerIndex();
	m_strCharacterID = data->CharacterID()->str();

	return true;
}