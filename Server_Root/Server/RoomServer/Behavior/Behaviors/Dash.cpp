#include "stdafx.h"
#include "Dash.h"
#include "../../Entity/IEntity.h"
#include "../BehaviorIDs.h"
#define _USE_MATH_DEFINES
#include <math.h>
#include "../../Game/BaeGameRoom.h"
#include "../../GameEvent/GameEvents/Position.h"

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

	int nTypes = CollisionObject::Type::CollisionObjectType_Terrain | CollisionObject::Type::CollisionObjectType_Character | CollisionObject::Type::CollisionObjectType_Projectile;

	m_pGameRoom->EntityMove(m_pEntity->GetID(), this, current, nTypes, m_lLastUpdateTime, lUpdateTime);

	//	This behavior might be stopped already in EntityMove()
	if (!m_bActivated)
		return;

	if (m_bProlonged)
	{
		m_bProlonged = false;
	}
	else
	{
		Stop(lUpdateTime);
	}
}

void Dash::Prolong()
{
	m_bProlonged = true;
}