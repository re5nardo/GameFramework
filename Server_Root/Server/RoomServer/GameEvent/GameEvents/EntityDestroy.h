#pragma once

#include "../IGameEvent.h"

namespace GameEvent
{
	class EntityDestroy : public IGameEvent
	{
	public:
		EntityDestroy();
		virtual ~EntityDestroy();

	public:
		int m_nEntityID;

	public:
		FBS::GameEventType GetType() override;
		const char* Serialize(int* pLength = NULL) override;
	};
}