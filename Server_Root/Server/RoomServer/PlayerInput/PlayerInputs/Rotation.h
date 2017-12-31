#pragma once

#include "../IPlayerInput.h"
#include "btBulletCollisionCommon.h"

namespace PlayerInput
{
	class Rotation : public IPlayerInput
	{
	public:
		Rotation();
		virtual ~Rotation();

	public:
		int			m_nEntityID;
		btVector3	m_vec3Rotation;

	public:
		FBS::PlayerInputType GetType() override;
		IPlayerInput* Clone() override;
		const char* Serialize(int* pLength = NULL) override;
		bool Deserialize(const char* pChar) override;
	};
}