#include "stdafx.h"
#include "IBehavior.h"

IBehavior::IBehavior(IEntity* pEntity)
{
	m_pEntity = pEntity;
}

IBehavior::~IBehavior()
{
}

void IBehavior::Start(__int64 lStartTime, ...)
{
	m_bActivated = true;
	m_lStartTime = lStartTime;
	m_lLastUpdateTime = lStartTime;
}

float IBehavior::GetTime()
{
	return (float)((m_lLastUpdateTime - m_lStartTime) / 1000.0f);
}

bool IBehavior::IsActivated()
{
	return m_bActivated;
}

void IBehavior::Stop()
{
	m_bActivated = false;
}