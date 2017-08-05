#include "stdafx.h"
#include "StateStart.h"
#include "../../../FBSFiles/StateStart_generated.h"

namespace GameEvent
{
	StateStart::StateStart()
	{
	}

	StateStart::~StateStart()
	{
	}

	FBS::GameEventType StateStart::GetType()
	{
		return FBS::GameEventType::GameEventType_StateStart;
	}

	const char* StateStart::Serialize(int* pLength)
	{
		FBS::GameEvent::StateStartBuilder data_builder(m_Builder);
		data_builder.add_EntityID(m_nEntityID);
		data_builder.add_StartTime(m_fStartTime);
		data_builder.add_StateID(m_nStateID);
		auto data = data_builder.Finish();

		m_Builder.Finish(data);

		*pLength = m_Builder.GetSize();

		return (char*)m_Builder.GetBufferPointer();
	}
}