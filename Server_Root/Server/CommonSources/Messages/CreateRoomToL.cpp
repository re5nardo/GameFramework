#include "stdafx.h"
#include "CreateRoomToL.h"
#include "CreateRoomToL_Data_generated.h"

CreateRoomToL::CreateRoomToL()
{
}

CreateRoomToL::~CreateRoomToL()
{
}

unsigned short CreateRoomToL::GetID()
{
	return MESSAGE_ID;
}

IMessage* CreateRoomToL::Clone()
{
	return NULL;
}

const char* CreateRoomToL::Serialize(int* pLength)
{
	vector<Offset<flatbuffers::String>> vecPlayers;

	for (vector<string>::iterator it = m_vecPlayers.begin(); it != m_vecPlayers.end(); ++it)
	{
		vecPlayers.push_back(m_Builder.CreateString(*it));
	}

	auto players = m_Builder.CreateVector(vecPlayers);

	CreateRoomToL_DataBuilder data_builder(m_Builder);
	data_builder.add_Result(m_nResult);
	data_builder.add_Players(players);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool CreateRoomToL::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<CreateRoomToL_Data>((const void*)pChar);

	m_nResult = data->Result();
	for (int i = 0; i < data->Players()->size(); ++i)
	{
		m_vecPlayers.push_back(data->Players()->Get(i)->str());
	}

	return true;
}