#pragma once

#include "../IGameEvent.h"
#include "btBulletCollisionCommon.h"

namespace GameEvent
{
	class Collision : public IGameEvent
	{
	public:
		Collision();
		virtual ~Collision();

	public:
		int			m_nEntityID;
		btVector3	m_vec3Position;

	public:
		FBS::GameEventType GetType() override;
		const char* Serialize(int* pLength = NULL) override;
	};
}