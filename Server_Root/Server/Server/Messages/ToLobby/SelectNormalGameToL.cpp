#include "stdafx.h"
#include "SelectNormalGameToL.h"
#include "../../../FBSFiles/SelectNormalGameToL_generated.h"

SelectNormalGameToL::SelectNormalGameToL()
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
	FBS::SelectNormalGameToLBuilder data_builder(m_Builder);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool SelectNormalGameToL::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<FBS::SelectNormalGameToL>((const void*)pChar);

	return true;
}