#include "stdafx.h"
#include "SelectNormalGameToL.h"
#include "SelectNormalGameToL_Data_generated.h"

SelectNormalGameToL::SelectNormalGameToL() : m_Builder(1024)
{
}

SelectNormalGameToL::~SelectNormalGameToL()
{
}

unsigned short SelectNormalGameToL::GetID()
{
	return MESSAGE_ID;
}

IMessage* SelectNormalGameToL::Clone()
{
	return NULL;
}

const char* SelectNormalGameToL::Serialize(int* pLength)
{
	SelectNormalGameToL_DataBuilder data_builder(m_Builder);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool SelectNormalGameToL::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<SelectNormalGameToL_Data>((const void*)pChar);

	return true;
}