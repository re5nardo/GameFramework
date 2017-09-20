#include "stdafx.h"
#include "Dash.h"
#include "../../Entity/IEntity.h"
#include "../BehaviorIDs.h"
#define _USE_MATH_DEFINES
#include <math.h>
#include "../../Game/BaeGameRoom.h"
#include "../../GameEvent/GameEvents/Position.h"
#include "../../GameEvent/GameEvents/Collision.h"

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

	m_bProlonged = true;

	if (m_pEntity->GetBehavior(BehaviorID::IDLE)->IsActivated())
		m_pEntity->GetBehavior(BehaviorID::IDLE)->Stop(lStartTime);

	IBehavior* pMove = m_pEntity->GetBehavior(BehaviorID::MOVE);
	if (pMove != NULL)
	{
		pMove->Update(lStartTime);
		if (pMove->IsActivated())
		{
			pMove->Stop(lStartTime);
		}
	}
}

void Dash::Initialize()
{
}

void Dash::UpdateBody(long long lUpdateTime)
{
	if (m_fDeltaTime == 0)
		return;

	btVector3 vec3Pos = m_pEntity->GetPosition();
	float radian = m_pEntity->GetRotation().y() * M_PI / 180;
	float x = btSin(radian);
	float z = btCos(radian);
	btVector3 vec3PosOffset(x, 0, z);
	btVector3 vec3Moved = vec3PosOffset.normalized() * m_pEntity->GetMoveSpeed() * 2 * m_fDeltaTime;
	btVector3 current = vec3Pos + vec3Moved;

	bool bChallenger = m_pEntity->GetEntityType() == FBS::Data::EntityType::EntityType_Character && m_pGameRoom->IsChallenger(m_pEntity->GetID());
	int nTerrainTypes = bChallenger ? CollisionObject::Type::CollisionObjectType_Terrain : CollisionObject::Type::CollisionObjectType_None;

	//	Check terrain first
	pair<int, btVector3> hitTerrain;
	if (m_pGameRoom->CheckContinuousCollisionDectectionFirst(m_pEntity->GetID(), current, nTerrainTypes, &hitTerrain))
	{
		//	Check collision second
		int nCollistionTypes = bChallenger ? CollisionObject::Type::CollisionObjectType_Projectile : CollisionObject::Type::CollisionObjectType_None;
		pair<int, btVector3> hitCollision;
		if (m_pGameRoom->CheckContinuousCollisionDectectionFirst(m_pEntity->GetID(), hitTerrain.second, nCollistionTypes, &hitCollision))
		{
			m_pEntity->SetPosition(hitCollision.second);

			GameEvent::Position* pPosition = new GameEvent::Position();
			pPosition->m_fEventTime = m_lLastUpdateTime / 1000.0f;
			pPosition->m_nEntityID = m_pEntity->GetID();
			pPosition->m_fStartTime = m_lLastUpdateTime / 1000.0f;
			pPosition->m_fEndTime = lUpdateTime / 1000.0f;
			pPosition->m_vec3StartPosition = vec3Pos;
			pPosition->m_vec3EndPosition = hitCollision.second;

			m_pGameRoom->AddGameEvent(pPosition);

			Stop(lUpdateTime);

			GameEvent::Collision* pCollision = new GameEvent::Collision();
			pCollision->m_fEventTime = lUpdateTime / 1000.0f;
			pCollision->m_nEntityID = bChallenger ? m_pEntity->GetID() : m_pGameRoom->GetEntityIDByCollisionObjectID(hitCollision.first);
			pCollision->m_vec3Position = hitCollision.second;

			m_pGameRoom->AddGameEvent(pCollision);
		}
		else
		{
			m_pEntity->SetPosition(hitTerrain.second);

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

			GameEvent::Position* pPosition = new GameEvent::Position();
			pPosition->m_fEventTime = m_lLastUpdateTime / 1000.0f;
			pPosition->m_nEntityID = m_pEntity->GetID();
			pPosition->m_fStartTime = m_lLastUpdateTime / 1000.0f;
			pPosition->m_fEndTime = lUpdateTime / 1000.0f;
			pPosition->m_vec3StartPosition = vec3Pos;
			pPosition->m_vec3EndPosition = hitCollision.second;

			m_pGameRoom->AddGameEvent(pPosition);

			Stop(lUpdateTime);

			GameEvent::Collision* pCollision = new GameEvent::Collision();
			pCollision->m_fEventTime = lUpdateTime / 1000.0f;
			pCollision->m_nEntityID = bChallenger ? m_pEntity->GetID() : m_pGameRoom->GetEntityIDByCollisionObjectID(hitCollision.first);
			pCollision->m_vec3Position = hitCollision.second;

			m_pGameRoom->AddGameEvent(pCollision);
		}
		else
		{
			m_pEntity->SetPosition(current);

			GameEvent::Position* pPosition = new GameEvent::Position();
			pPosition->m_fEventTime = m_lLastUpdateTime / 1000.0f;
			pPosition->m_nEntityID = m_pEntity->GetID();
			pPosition->m_fStartTime = m_lLastUpdateTime / 1000.0f;
			pPosition->m_fEndTime = lUpdateTime / 1000.0f;
			pPosition->m_vec3StartPosition = vec3Pos;
			pPosition->m_vec3EndPosition = current;

			m_pGameRoom->AddGameEvent(pPosition);

			if (m_bProlonged)
			{
				m_bProlonged = false;
			}
			else
			{
				Stop(lUpdateTime);
			}
		}
	}
}

void Dash::Prolong()
{
	m_bProlonged = true;
}