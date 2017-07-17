#include "stdafx.h"
#include "ISkill.h"

ISkill::ISkill(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID)
{
	m_pGameRoom = pGameRoom;
	m_pEntity = pEntity;
	m_nMasterDataID = nMasterDataID;
}

ISkill::~ISkill()
{
}

int ISkill::GetMasterDataID()
{
	return m_nMasterDataID;
}

void ISkill::Start(__int64 lStartTime, ...)
{
	if (m_bActivated)
		return;

	m_bActivated = true;
	m_lStartTime = lStartTime;
}

float ISkill::GetTime()
{
	return (float)((m_lLastUpdateTime - m_lStartTime) / 1000.0f);
}

bool ISkill::IsActivated()
{
	return m_bActivated;
}