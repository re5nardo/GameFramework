#pragma once

#include "../IGameEvent.h"

namespace GameEvent
{
	class StateEnd : public IGameEvent
	{
	public:
		StateEnd();
		virtual ~StateEnd();

	public:
		int		m_nEntityID;
		float	m_fEndTime;
		int		m_nStateID;

	public:
		FBS::GameEventType GetType() override;
		const char* Serialize(int* pLength = NULL) override;
	};
}