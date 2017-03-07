#include "stdafx.h"
#include "Move.h"
#include "../../Entity/IEntity.h"
#include <math.h>
#include "../../Util.h"

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
	m_vec3Dest = va_arg(ap, Vector3);
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

	Vector3 vec3Pos = m_pEntity->GetPosition();
	float fExpectedTime = sqrt(powf(m_vec3Dest.x - vec3Pos.x, 2.0f) /*+ powf(m_vec3Dest.y - vec3Pos.y, 2.0f)*/ + powf(m_vec3Dest.z - vec3Pos.z, 2.0f)) / m_pEntity->GetSpeed();

	if (fDeltaTime >= fExpectedTime)
	{
		m_pEntity->SetPosition(m_vec3Dest);

		m_bActivated = false;
	}
	else
	{
		Vector3 current = Util::Lerp(vec3Pos, m_vec3Dest, fDeltaTime / fExpectedTime);
		
		m_pEntity->SetPosition(current);
	}
}