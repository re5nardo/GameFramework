#include "stdafx.h"
#include "ContinueSkill.h"
#include "../../Entity/IEntity.h"
#include "../../Entity/Entities/Character/Character.h"
#include "../../MasterData/MasterDataManager.h"
#include "../../MasterData/Skill.h"

const string ContinueSkill::NAME = "ContinueSkill";

ContinueSkill::ContinueSkill(IEntity* pEntity, int nMasterDataID) : ISkill(pEntity, nMasterDataID)
{
}

ContinueSkill::~ContinueSkill()
{
}

bool ContinueSkill::IsCoolTime(__int64 lTime)
{
	if (m_lEndTime == -1)
		return false;

	return (lTime - m_lEndTime) / 1000 <= m_fCooltime;
}

bool ContinueSkill::IsValidToStart(__int64 lTime)
{
	return !IsCoolTime(lTime) && ((Character*)m_pEntity)->GetCurrentMP() >= m_fMP;
}

void ContinueSkill::Start(__int64 lStartTime, ...)
{
	__super::Start(lStartTime);

	BaeGameRoom* pBaeGameRoom;

	va_list ap;
	va_start(ap, lStartTime);
	pBaeGameRoom = va_arg(ap, BaeGameRoom*);
	va_end(ap);

	m_bContinue = true;

	m_pTargetBehavior = m_pEntity->GetBehavior(m_nTargetBehaviorID);
	m_pTargetBehavior->Start(lStartTime, pBaeGameRoom);
}

void ContinueSkill::Initialize()
{
	MasterData::Skill* pMasterSkill = NULL;
	MasterDataManager::Instance()->GetData<MasterData::Skill>(m_nMasterDataID, pMasterSkill);

	m_nTargetBehaviorID = pMasterSkill->m_vecBehavior[0].first;
	m_fLength = pMasterSkill->m_fLength;
	m_fCooltime = pMasterSkill->m_fCoolTime;
	m_fMP = pMasterSkill->m_fMP;
}

void ContinueSkill::Update(__int64 lUpdateTime)
{
	if (!m_bActivated || (lUpdateTime != m_lStartTime && lUpdateTime <= m_lLastUpdateTime))
		return;

	if ((lUpdateTime - m_lStartTime) / 1000 >= m_fLength || !m_bContinue)
	{
		m_pTargetBehavior->Stop();
		m_pTargetBehavior = NULL;

		m_bContinue = false;
		m_bActivated = false;
		m_lEndTime = lUpdateTime;

		return;
	}

	m_bContinue = false;
	m_lLastUpdateTime = lUpdateTime;
}

void ContinueSkill::ProcessInput(__int64 lTime, BaeGameRoom* pBaeGameRoom)
{
	if (m_bActivated)
	{
		m_bContinue = true;
	}
	else
	{
		if (!IsValidToStart(lTime))
			return;

		Start(lTime, pBaeGameRoom);
	}
}