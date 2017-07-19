#include "stdafx.h"
#include "PreparationStateToC.h"
#include "../../../FBSFiles/PreparationStateToC_generated.h"

PreparationStateToC::PreparationStateToC()
{
}

PreparationStateToC::~PreparationStateToC()
{
}

unsigned short PreparationStateToC::GetID()
{
	return MESSAGE_ID;
}

IMessage* PreparationStateToC::Clone()
{
	return NULL;
}

const char* PreparationStateToC::Serialize(int* pLength)
{
	FBS::PreparationStateToCBuilder data_builder(m_Builder);
	data_builder.add_PlayerIndex(m_nPlayerIndex);
	data_builder.add_State(m_fState);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool PreparationStateToC::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<FBS::PreparationStateToC>((const void*)pChar);

	m_nPlayerIndex = data->PlayerIndex();
	m_fState = data->State();

	return true;
}