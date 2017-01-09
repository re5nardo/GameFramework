#include "stdafx.h"
#include "SelectNormalGameToC.h"
#include "SelectNormalGameToC_Data_generated.h"

SelectNormalGameToC::SelectNormalGameToC() : m_Builder(1024)
{
}

SelectNormalGameToC::~SelectNormalGameToC()
{
}

unsigned short SelectNormalGameToC::GetID()
{
	return MESSAGE_ID;
}

IMessage* SelectNormalGameToC::Clone()
{
	return NULL;
}

const char* SelectNormalGameToC::Serialize(int* pLength)
{
	SelectNormalGameToC_DataBuilder data_builder(m_Builder);
	data_builder.add_Result(m_nResult);
	data_builder.add_ExpectedTime(m_nExpectedTime);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool SelectNormalGameToC::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<SelectNormalGameToC_Data>((const void*)pChar);

	m_nResult = data->Result();
	m_nExpectedTime = data->ExpectedTime();

	return true;
}