#include "stdafx.h"
#include "Move.h"
#include "../../Entity/IEntity.h"
#include <math.h>
#include "../../Util.h"
#include "../../Game/BaeGameRoom.h"
#include "btBulletCollisionCommon.h"
#include "../../GameEvent/GameEvents/Position.h"
#include "../../GameEvent/GameEvents/Rotation.h"
#include "../../GameEvent/GameEvents/Collision.h"
#include "../BehaviorIDs.h"

const string Move::NAME = "Move";

Move::Move(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID) : IBehavior(pGameRoom, pEntity, nMasterDataID)
{
}

Move::~Move()
{
}

void Move::Start(long long lStartTime, ...)
{
	__super::Start(lStartTime);

	va_list ap;
	va_start(ap, lStartTime);
	btVector3* pVec3Dest = va_arg(ap, btVector3*);
	m_vec3Dest = *pVec3Dest;
	va_end(ap);

	if (m_pEntity->GetBehavior(BehaviorID::IDLE) != NULL && m_pEntity->GetBehavior(BehaviorID::IDLE)->IsActivated())
		m_pEntity->GetBehavior(BehaviorID::IDLE)->Stop(lStartTime);
}

void Move::Initialize()
{
}

void Move::UpdateBody(long long lUpdateTime)
{
	if (m_fDeltaTime == 0)
		return;

	//	Rotation
	btVector3 vec3StartRotation = m_pEntity->GetRotation();
	btVector3 vec3Move = m_vec3Dest - m_pEntity->GetPosition();
	float fTargetRotation_Y = Util::GetAngle_Y(vec3Move);
	btVector3 vec3Rotation = m_pEntity->GetRotation();

	if (fTargetRotation_Y < vec3Rotation.y())
	{
		fTargetRotation_Y += 360;
	}

	//	For lerp calculation
	//	Clockwise rotation
	if (fTargetRotation_Y - vec3Rotation.y() <= 180)
	{
		if (vec3Rotation.y() > fTargetRotation_Y)
			fTargetRotation_Y += 360;
	}
	//	CounterClockwise rotation
	else
	{
		if (vec3Rotation.y() < fTargetRotation_Y)
			vec3Rotation.setY(vec3Rotation.y() + 360);
	}

	float RotationTime = 0.05f;
	float fCurrent_Y = Util::Lerp(vec3Rotation.y(), fTargetRotation_Y, m_fDeltaTime / RotationTime);
	if (fCurrent_Y >= 360) fCurrent_Y -= 360;
	m_pEntity->SetRotation(btVector3(vec3Rotation.x(), fCurrent_Y, vec3Rotation.z()));

	GameEvent::Rotation* pRotation = new GameEvent::Rotation();
	pRotation->m_fEventTime = m_lLastUpdateTime / 1000.0f;
	pRotation->m_nEntityID = m_pEntity->GetID();
	pRotation->m_fStartTime = m_lLastUpdateTime / 1000.0f;
	pRotation->m_fEndTime = lUpdateTime / 1000.0f;
	pRotation->m_vec3StartRotation = vec3StartRotation;
	pRotation->m_vec3EndRotation = m_pEntity->GetRotation();

	m_pGameRoom->AddGameEvent(pRotation);
	//	Rotation end

	btVector3 vec3Pos = m_pEntity->GetPosition();
	btVector3 vec3ToMove = m_vec3Dest - vec3Pos;
	btVector3 vec3Moved = vec3ToMove.normalized() * m_pEntity->GetMoveSpeed() * m_fDeltaTime;
	btVector3 current = vec3Pos + vec3Moved;
	if (Util::IsEqual(vec3Moved, vec3ToMove) || vec3Moved.length2() > vec3ToMove.length2())
	{
		current = m_vec3Dest;
	}

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
		if (m_pGameRoom->CheckContinuousCollisionDectectionFirst(m_pEntity->GetID(), current, nCollistionTypes, &hitCollision))
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
			bool bEnd = Util::IsEqual(vec3Moved, vec3ToMove) || vec3Moved.length2() > vec3ToMove.length2();
			float fEndTime = lUpdateTime / 1000.0f;
			if (bEnd)
			{
				current = m_vec3Dest;
				float elapsed = (current - vec3Pos).length() / m_pEntity->GetMoveSpeed();
				fEndTime = m_lLastUpdateTime / 1000.0f + elapsed;
			}

			m_pEntity->SetPosition(current);

			GameEvent::Position* pPosition = new GameEvent::Position();
			pPosition->m_fEventTime = m_lLastUpdateTime / 1000.0f;
			pPosition->m_nEntityID = m_pEntity->GetID();
			pPosition->m_fStartTime = m_lLastUpdateTime / 1000.0f;
			pPosition->m_fEndTime = fEndTime;
			pPosition->m_vec3StartPosition = vec3Pos;
			pPosition->m_vec3EndPosition = current;

			m_pGameRoom->AddGameEvent(pPosition);

			if (bEnd)
			{
				Stop(fEndTime * 1000.0f);
			}
		}
	}
}