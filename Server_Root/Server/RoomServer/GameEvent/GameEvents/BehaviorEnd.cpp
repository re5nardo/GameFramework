#include "stdafx.h"
#include "BehaviorEnd.h"
#include "../../../FBSFiles/BehaviorEnd_generated.h"

namespace GameEvent
{
	BehaviorEnd::BehaviorEnd()
	{
	}

	BehaviorEnd::~BehaviorEnd()
	{
	}

	FBS::GameEventType BehaviorEnd::GetType()
	{
		return FBS::GameEventType::GameEventType_BehaviorEnd;
	}

	const char* BehaviorEnd::Serialize(int* pLength)
	{
		FBS::GameEvent::BehaviorEndBuilder data_builder(m_Builder);
		data_builder.add_EntityID(m_nEntityID);
		data_builder.add_EndTime(m_fEndTime);
		data_builder.add_BehaviorID(m_nBehaviorID);
		auto data = data_builder.Finish();

		m_Builder.Finish(data);

		*pLength = m_Builder.GetSize();

		return (char*)m_Builder.GetBufferPointer();
	}
}