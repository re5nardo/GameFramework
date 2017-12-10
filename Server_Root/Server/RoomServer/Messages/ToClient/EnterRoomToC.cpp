#include "stdafx.h"
#include "EnterRoomToC.h"
#include <list>

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
	auto players = m_Builder.CreateVectorOfStructs(m_vecPlayerInfo);

	FBS::EnterRoomToCBuilder data_builder(m_Builder);
	data_builder.add_Result(m_nResult);
	data_builder.add_UserPlayerIndex(m_nUserPlayerIndex);
	data_builder.add_Players(players);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool EnterRoomToC::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<FBS::EnterRoomToC>((const void*)pChar);

	//	Not necessary.. (Deserialize() is never called in server side..)

	return true;
}