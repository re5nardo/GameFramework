#pragma once

#include "../IGameEvent.h"
#include "btBulletCollisionCommon.h"

namespace GameEvent
{
	class Position : public IGameEvent
	{
	public:
		Position();
		virtual ~Position();

	public:
		int			m_nEntityID;
		float		m_fStartTime;
		float		m_fEndTime;
		btVector3	m_vec3StartPosition;
		btVector3	m_vec3EndPosition;

	public:
		FBS::GameEventType GetType() override;
		const char* Serialize(int* pLength = NULL) override;
	};
}