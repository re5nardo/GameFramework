#pragma once

#include "../IPlayerInput.h"
#include "btBulletCollisionCommon.h"

namespace PlayerInput
{
	class Position : public IPlayerInput
	{
	public:
		Position();
		virtual ~Position();

	public:
		int			m_nEntityID;
		btVector3	m_vec3Position;

	public:
		FBS::PlayerInputType GetType() override;
		IPlayerInput* Clone() override;
		const char* Serialize(int* pLength = NULL) override;
		bool Deserialize(const char* pChar) override;
	};
}