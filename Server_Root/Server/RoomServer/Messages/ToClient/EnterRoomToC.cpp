#include "stdafx.h"
#include "EnterRoomToC.h"
#include "../../../FBSFiles/EnterRoomToC_generated.h"

EnterRoomToC::EnterRoomToC()
{
}

EnterRoomToC::~EnterRoomToC()
{
}

unsigned short EnterRoomToC::GetID()
{
	return MESSAGE_ID;
}

IMessage* EnterRoomToC::Clone()
{
	return NULL;
}

const char* EnterRoomToC::Serialize(int* pLength)
{
	vector<int> vecKeys;
	vector<Offset<flatbuffers::String>> vecValues;

	for (map<int, string>::iterator it = m_mapPlayers.begin(); it != m_mapPlayers.end(); ++it)
	{
		vecKeys.push_back(it->first);
		vecValues.push_back(m_Builder.CreateString(it->second));
	}

	auto playersMapKey = m_Builder.CreateVector(vecKeys);
	auto playersMapValue = m_Builder.CreateVector(vecValues);

	FBS::EnterRoomToCBuilder data_builder(m_Builder);
	data_builder.add_Result(m_nResult);
	data_builder.add_PlayerIndex(m_nPlayerIndex);
	data_builder.add_PlayerEntityID(m_nPlayerEntityID);
	data_builder.add_PlayersMapKey(playersMapKey);
	data_builder.add_PlayersMapValue(playersMapValue);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool EnterRoomToC::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<FBS::EnterRoomToC>((const void*)pChar);

	m_nResult = data->Result();
	m_nPlayerIndex = data->PlayerIndex();
	m_nPlayerEntityID = data->PlayerEntityID();
	for (int i = 0; i < data->PlayersMapKey()->size(); ++i)
	{
		m_mapPlayers[data->PlayersMapKey()->Get(i)] = data->PlayersMapValue()->Get(i)->str();
	}

	return true;
}