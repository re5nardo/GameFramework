#pragma once

#include "../IGameEvent.h"
#include "btBulletCollisionCommon.h"

namespace GameEvent
{
	class Rotation : public IGameEvent
	{
	public:
		Rotation();
		virtual ~Rotation();

	public:
		int			m_nEntityID;
		float		m_fStartTime;
		float		m_fEndTime;
		btVector3	m_vec3StartRotation;
		btVector3	m_vec3EndRotation;

	public:
		FBS::GameEventType GetType() override;
		const char* Serialize(int* pLength = NULL) override;
	};
}