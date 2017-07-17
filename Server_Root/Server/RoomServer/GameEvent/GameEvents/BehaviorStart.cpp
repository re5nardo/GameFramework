#include "stdafx.h"
#include "BehaviorStart.h"
#include "BehaviorStart_generated.h"

namespace GameEvent
{
	BehaviorStart::BehaviorStart()
	{
	}

	BehaviorStart::~BehaviorStart()
	{
	}

	FBS::GameEventType BehaviorStart::GetType()
	{
		return FBS::GameEventType::GameEventType_BehaviorStart;
	}

	const char* BehaviorStart::Serialize(int* pLength)
	{
		FBS::GameEvent::BehaviorStartBuilder data_builder(m_Builder);
		data_builder.add_EntityID(m_nEntityID);
		data_builder.add_StartTime(m_fStartTime);
		data_builder.add_BehaviorID(m_nBehaviorID);
		auto data = data_builder.Finish();

		m_Builder.Finish(data);

		*pLength = m_Builder.GetSize();

		return (char*)m_Builder.GetBufferPointer();
	}
}