#pragma once

#include "../IGameEvent.h"

namespace GameEvent
{
	class BehaviorStart : public IGameEvent
	{
	public:
		BehaviorStart();
		virtual ~BehaviorStart();

	public:
		int		m_nEntityID;
		float	m_fStartTime;
		int		m_nBehaviorID;

	public:
		FBS::GameEventType GetType() override;
		const char* Serialize(int* pLength = NULL) override;
	};
}