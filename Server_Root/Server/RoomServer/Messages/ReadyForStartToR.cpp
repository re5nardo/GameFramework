#include "stdafx.h"
#include "ReadyForStartToR.h"
#include "ReadyForStartToR_Data_generated.h"

ReadyForStartToR::ReadyForStartToR() : m_Builder(1024)
{
}

ReadyForStartToR::~ReadyForStartToR()
{
}

unsigned short ReadyForStartToR::GetID()
{
	return MESSAGE_ID;
}

IMessage* ReadyForStartToR::Clone()
{
	return NULL;
}

const char* ReadyForStartToR::Serialize(int* pLength)
{
	ReadyForStartToR_DataBuilder data_builder(m_Builder);
	data_builder.add_PlayerIndex(m_nPlayerIndex);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool ReadyForStartToR::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<ReadyForStartToR_Data>((const void*)pChar);

	m_nPlayerIndex = data->PlayerIndex();

	return true;
}