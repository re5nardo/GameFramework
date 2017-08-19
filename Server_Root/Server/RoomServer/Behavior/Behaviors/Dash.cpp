#include "stdafx.h"
#include "Dash.h"
#include "../../Entity/IEntity.h"
#include "../BehaviorIDs.h"
#define _USE_MATH_DEFINES
#include <math.h>
#include "../../Game/BaeGameRoom.h"
#include "../../GameEvent/GameEvents/Position.h"
#include "../../GameEvent/GameEvents/Rotation.h"

const string Dash::NAME = "Dash";

Dash::Dash(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID) : IBehavior(pGameRoom, pEntity, nMasterDataID)
{
}

Dash::~Dash()
{
}

void Dash::Start(long long lStartTime, ...)
{
	__super::Start(lStartTime);

	va_list ap;
	va_start(ap, lStartTime);
	m_pGameRoom = va_arg(ap, BaeGameRoom*);
	va_end(ap);

	if (m_pEntity->GetBehavior(BehaviorID::IDLE)->IsActivated())
		m_pEntity->GetBehavior(BehaviorID::IDLE)->Stop(lStartTime);
}

void Dash::Initialize()
{
}

void Dash::UpdateBody(long long lUpdateTime)
{
	btVector3 vec3Pos = m_pEntity->GetPosition();
	float radian = m_pEntity->GetRotation().y() * M_PI / 180;
	float x = 2 * btSin(radian);
	float z = 2 * btCos(radian);
	btVector3 vec3PosOffset(x, 0, z);
	btVector3 vec3Target = vec3Pos + vec3PosOffset;

	bool bChallenger = m_pEntity->GetEntityType() == FBS::Data::EntityType::EntityType_Character && m_pGameRoom->IsChallenger(m_pEntity->GetID());
	int nTerrainTypes = bChallenger ? CollisionObject::Type::CollisionObjectType_Terrain : CollisionObject::Type::CollisionObjectType_None;

	//	Check terrain first
	pair<int, btVector3> hitTerrain;
	if (m_pGameRoom->CheckContinuousCollisionDectectionFirst(m_pEntity->GetID(), vec3Target, nTerrainTypes, &hitTerrain))
	{
		//	Check collision second
		int nCollistionTypes = bChallenger ? CollisionObject::Type::CollisionObjectType_Projectile : CollisionObject::Type::CollisionObjectType_None;
		pair<int, btVector3> hitCollision;
		if (m_pGameRoom->CheckContinuousCollisionDectectionFirst(m_pEntity->GetID(), hitTerrain.second, nCollistionTypes, &hitCollision))
		{
			m_pEntity->SetPosition(hitCollision.second);
			m_pGameRoom->SetCollisionObjectPosition(m_pEntity->GetID(), hitCollision.second);

			GameEvent::Position* pPosition = new GameEvent::Position();
			pPosition->m_fEventTime = m_lLastUpdateTime / 1000.0f;
			pPosition->m_nEntityID = m_pEntity->GetID();
			pPosition->m_fStartTime = m_lLastUpdateTime / 1000.0f;
			pPosition->m_fEndTime = lUpdateTime / 1000.0f;
			pPosition->m_vec3StartPosition = vec3Pos;
			pPosition->m_vec3EndPosition = hitCollision.second;

			m_pGameRoom->AddGameEvent(pPosition);

			Stop(lUpdateTime);

			//	Add Collision Event
			//	...
			//	...
		}
		else
		{
			m_pEntity->SetPosition(hitTerrain.second);
			m_pGameRoom->SetCollisionObjectPosition(m_pEntity->GetID(), hitTerrain.second);

			GameEvent::Position* pPosition = new GameEvent::Position();
			pPosition->m_fEventTime = m_lLastUpdateTime / 1000.0f;
			pPosition->m_nEntityID = m_pEntity->GetID();
			pPosition->m_fStartTime = m_lLastUpdateTime / 1000.0f;
			pPosition->m_fEndTime = lUpdateTime / 1000.0f;
			pPosition->m_vec3StartPosition = vec3Pos;
			pPosition->m_vec3EndPosition = hitTerrain.second;

			m_pGameRoom->AddGameEvent(pPosition);

			Stop(lUpdateTime);
		}
	}
	else
	{
		//	Check collision second
		int nCollistionTypes = bChallenger ? CollisionObject::Type::CollisionObjectType_Projectile : CollisionObject::Type::CollisionObjectType_None;
		pair<int, btVector3> hitCollision;
		if (m_pGameRoom->CheckContinuousCollisionDectectionFirst(m_pEntity->GetID(), hitTerrain.second, nCollistionTypes, &hitCollision))
		{
			m_pEntity->SetPosition(hitCollision.second);
			m_pGameRoom->SetCollisionObjectPosition(m_pEntity->GetID(), hitCollision.second);

			GameEvent::Position* pPosition = new GameEvent::Position();
			pPosition->m_fEventTime = m_lLastUpdateTime / 1000.0f;
			pPosition->m_nEntityID = m_pEntity->GetID();
			pPosition->m_fStartTime = m_lLastUpdateTime / 1000.0f;
			pPosition->m_fEndTime = lUpdateTime / 1000.0f;
			pPosition->m_vec3StartPosition = vec3Pos;
			pPosition->m_vec3EndPosition = hitCollision.second;

			m_pGameRoom->AddGameEvent(pPosition);

			Stop(lUpdateTime);

			//	Add Collision Event
			//	...
			//	...
		}
		else
		{
			m_pEntity->SetPosition(vec3Target);
			m_pGameRoom->SetCollisionObjectPosition(m_pEntity->GetID(), vec3Target);

			GameEvent::Position* pPosition = new GameEvent::Position();
			pPosition->m_fEventTime = m_lLastUpdateTime / 1000.0f;
			pPosition->m_nEntityID = m_pEntity->GetID();
			pPosition->m_fStartTime = m_lLastUpdateTime / 1000.0f;
			pPosition->m_fEndTime = lUpdateTime / 1000.0f;
			pPosition->m_vec3StartPosition = vec3Pos;
			pPosition->m_vec3EndPosition = vec3Target;

			m_pGameRoom->AddGameEvent(pPosition);
		}
	}
}