#pragma once

#include "../IGameEvent.h"
#include "../../../FBSFiles/FBSData_generated.h"
#include "btBulletCollisionCommon.h"

namespace GameEvent
{
	class EntityCreate : public IGameEvent
	{
	public:
		EntityCreate();
		virtual ~EntityCreate();

	public:
		int m_nEntityID;
		int m_nMasterDataID;
		FBS::Data::EntityType m_EntityType;
		btVector3 m_vec3Position;
		btVector3 m_vec3Rotation;

	public:
		FBS::GameEventType GetType() override;
		const char* Serialize(int* pLength = NULL) override;
	};
}