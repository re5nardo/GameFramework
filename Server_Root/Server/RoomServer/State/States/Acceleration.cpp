#include "stdafx.h"
#include "Acceleration.h"
#include "../../Entity/IEntity.h"
#include "../../Entity/Entities/Character/Character.h"

Acceleration::Acceleration(IEntity* pEntity, __int64 lStartTime) : IState(pEntity, lStartTime)
{
}

Acceleration::~Acceleration()
{
}

int Acceleration::GetID()
{
	return STATE_ID;
}

void Acceleration::Update(__int64 lUpdateTime)
{
	//	Validation check
	if (lUpdateTime != m_lStartTime && lUpdateTime <= m_lLastUpdateTime)
		return;

	__int64 last = m_lLastUpdateTime - m_lStartTime;
	__int64 cur = lUpdateTime - m_lStartTime;

	if (last < 1000 && cur >= 1000)
	{
		((Character*)m_pEntity)->PlusMoveSpeed(1);
	}
	else if (last < 2000 && cur >= 2000)
	{
		((Character*)m_pEntity)->PlusMoveSpeed(1);
	}
	else if (last < 3000 && cur >= 3000)
	{
		((Character*)m_pEntity)->PlusMoveSpeed(1);
	}
	else if (last < 4000 && cur >= 4000)
	{
		((Character*)m_pEntity)->PlusMoveSpeed(1);
	}
	else if (last < 5000 && cur >= 5000)
	{
		((Character*)m_pEntity)->PlusMoveSpeed(1);
	}
	else if (last < 6000 && cur >= 6000)
	{
		((Character*)m_pEntity)->MinusMoveSpeed(1);
	}
	else if (last < 7000 && cur >= 7000)
	{
		((Character*)m_pEntity)->MinusMoveSpeed(1);
	}
	else if (last < 8000 && cur >= 8000)
	{
		((Character*)m_pEntity)->MinusMoveSpeed(1);
	}
	else if (last < 9000 && cur >= 9000)
	{
		((Character*)m_pEntity)->MinusMoveSpeed(1);
	}
	else if (last - m_lStartTime < 10000 && cur >= 10000)
	{
		((Character*)m_pEntity)->MinusMoveSpeed(1);
	}

	if (cur >= 10000)
	{
		m_pEntity->RemoveState(STATE_ID);
		return;
	}

	m_lLastUpdateTime = lUpdateTime;
}