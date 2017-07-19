#include "stdafx.h"
#include "CreateRoomToR.h"
#include "../../../FBSFiles/CreateRoomToR_generated.h"

CreateRoomToR::CreateRoomToR()
{
}

CreateRoomToR::~CreateRoomToR()
{
}

unsigned short CreateRoomToR::GetID()
{
	return MESSAGE_ID;
}

IMessage* CreateRoomToR::Clone()
{
	return NULL;
}

const char* CreateRoomToR::Serialize(int* pLength)
{
	vector<Offset<flatbuffers::String>> vecPlayers;

	for (vector<string>::iterator it = m_vecPlayers.begin(); it != m_vecPlayers.end(); ++it)
	{
		vecPlayers.push_back(m_Builder.CreateString(*it));
	}

	auto players = m_Builder.CreateVector(vecPlayers);

	FBS::CreateRoomToRBuilder data_builder(m_Builder);
	data_builder.add_MatchID(m_nMatchID);
	data_builder.add_Players(players);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool CreateRoomToR::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<FBS::CreateRoomToR>((const void*)pChar);

	m_nMatchID = data->MatchID();
	for (int i = 0; i < data->Players()->size(); ++i)
	{
		m_vecPlayers.push_back(data->Players()->Get(i)->str());
	}

	return true;
}