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

void ISkill::Update(__int64 lUpdateTime)
{
	if (!m_bActivated || (m_lLastUpdateTime == lUpdateTime))
		return;

	if (m_lStartTime == lUpdateTime)
	{
		m_fPreviousTime = 0;
		m_fCurrentTime = 0;
		m_fDeltaTime = 0;
	}
	else
	{
		m_fPreviousTime = (m_lLastUpdateTime - m_lStartTime) / 1000.0f;
		m_fCurrentTime = (lUpdateTime - m_lStartTime) / 1000.0f;
		m_fDeltaTime = m_fCurrentTime - m_fPreviousTime;
	}

	UpdateBody(lUpdateTime);

	m_lLastUpdateTime = lUpdateTime;
}

float ISkill::GetTime()
{
	return (float)((m_lLastUpdateTime - m_lStartTime) / 1000.0f);
}

bool ISkill::IsActivated()
{
	return m_bActivated;
}

void ISkill::Stop(__int64 lTime)
{
	m_bActivated = false;
	m_lEndTime = lTime;
}