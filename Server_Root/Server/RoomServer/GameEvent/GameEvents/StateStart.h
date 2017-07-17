#pragma once

#include "../IGameEvent.h"

namespace GameEvent
{
	class StateStart : public IGameEvent
	{
	public:
		StateStart();
		virtual ~StateStart();

	public:
		int		m_nEntityID;
		float	m_fStartTime;
		int		m_nStateID;

	public:
		FBS::GameEventType GetType() override;
		const char* Serialize(int* pLength = NULL) override;
	};
}