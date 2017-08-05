#include "stdafx.h"
#include "StateEnd.h"
#include "../../../FBSFiles/StateEnd_generated.h"

namespace GameEvent
{
	StateEnd::StateEnd()
	{
	}

	StateEnd::~StateEnd()
	{
	}

	FBS::GameEventType StateEnd::GetType()
	{
		return FBS::GameEventType::GameEventType_StateEnd;
	}

	const char* StateEnd::Serialize(int* pLength)
	{
		FBS::GameEvent::StateEndBuilder data_builder(m_Builder);
		data_builder.add_EntityID(m_nEntityID);
		data_builder.add_EndTime(m_fEndTime);
		data_builder.add_StateID(m_nStateID);
		auto data = data_builder.Finish();

		m_Builder.Finish(data);

		*pLength = m_Builder.GetSize();

		return (char*)m_Builder.GetBufferPointer();
	}
}