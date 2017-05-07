#include "stdafx.h"
#include "Move.h"
#include "../../Entity/IEntity.h"
#include <math.h>
#include "../../Util.h"
#include "../../Game/BaeGameRoom.h"
#include "btBulletCollisionCommon.h"

Move::Move(IEntity* pEntity) : IBehavior(pEntity)
{
}

Move::~Move()
{
}

int Move::GetID()
{
	return BEHAVIOR_ID;
}

void Move::Start(__int64 lStartTime, ...)
{
	if (!m_bActivated)
	{
		IBehavior::Start(lStartTime);
	}

	va_list ap;
	va_start(ap, lStartTime);
	btVector3* pVec3Dest = va_arg(ap, btVector3*);
	m_vec3Dest = *pVec3Dest;
	m_pGameRoom = va_arg(ap, BaeGameRoom*);
	va_end(ap);

	if (m_pEntity->GetBehavior(Idle_ID)->IsActivated())
		m_pEntity->GetBehavior(Idle_ID)->Stop();
}

void Move::Initialize()
{
}

void Move::Update(__int64 lUpdateTime)
{
	if (!m_bActivated)
		return;

	float fDeltaTime = (lUpdateTime - m_lLastUpdateTime) / 1000.0f;
	m_lLastUpdateTime = lUpdateTime;

	btVector3 vec3Pos = m_pEntity->GetPosition();
	float fExpectedTime = sqrt(powf(m_vec3Dest.x() - vec3Pos.x(), 2.0f) /*+ powf(m_vec3Dest.y - vec3Pos.y, 2.0f)*/ + powf(m_vec3Dest.z() - vec3Pos.z(), 2.0f)) / m_pEntity->GetSpeed();

	btVector3 current = Util::Lerp(vec3Pos, m_vec3Dest, fDeltaTime / fExpectedTime);
	btTransform trHit;

	if (m_pGameRoom->TryMove(m_pEntity->GetID(), current, trHit))
	{
		m_pEntity->SetPosition(current);
		m_pGameRoom->SetCollisionObjectPosition(m_pEntity->GetID(), current);

		if (fDeltaTime >= fExpectedTime)
		{
			m_bActivated = false;
		}
	}
	else
	{
		m_pEntity->SetPosition(trHit.getOrigin());
		m_pGameRoom->SetCollisionObjectPosition(m_pEntity->GetID(), trHit.getOrigin());

		m_bActivated = false;
	}
}