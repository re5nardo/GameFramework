#include "stdafx.h"
#include "IBehavior.h"

IBehavior::IBehavior(IEntity* pEntity, int nMasterDataID)
{
	m_pEntity = pEntity;
	m_nMasterDataID = nMasterDataID;
}

IBehavior::~IBehavior()
{
}

int IBehavior::GetMasterDataID()
{
	return m_nMasterDataID;
}

void IBehavior::Start(__int64 lStartTime, ...)
{
	if (m_bActivated) return;

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