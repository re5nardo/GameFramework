#pragma once

#include "../IGameEvent.h"

namespace GameEvent
{
	class BehaviorEnd : public IGameEvent
	{
	public:
		BehaviorEnd();
		virtual ~BehaviorEnd();

	public:
		int		m_nEntityID;
		float	m_fEndTime;
		int		m_nBehaviorID;

	public:
		FBS::GameEventType GetType() override;
		const char* Serialize(int* pLength = NULL) override;
	};
}