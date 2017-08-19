#include "stdafx.h"
#include "PreparationStateToR.h"
#include "../../../FBSFiles/PreparationStateToR_generated.h"

PreparationStateToR::PreparationStateToR()
{
}

PreparationStateToR::~PreparationStateToR()
{
}

unsigned short PreparationStateToR::GetID()
{
	return MESSAGE_ID;
}

IMessage* PreparationStateToR::Clone()
{
	return NULL;
}

const char* PreparationStateToR::Serialize(int* pLength)
{
	FBS::PreparationStateToRBuilder data_builder(m_Builder);
	data_builder.add_State(m_fState);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool PreparationStateToR::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<FBS::PreparationStateToR>((const void*)pChar);

	m_fState = data->State();

	return true;
}