#include "stdafx.h"
#include "ContinueSkill.h"
#include "../../Entity/IEntity.h"
#include "../../Entity/Entities/Character/Character.h"
#include "../../MasterData/MasterDataManager.h"
#include "../../MasterData/Skill.h"
#include "../../Messages/ToRoom/GameInputSkillToR.h"

const string ContinueSkill::NAME = "ContinueSkill";

ContinueSkill::ContinueSkill(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID) : ISkill(pGameRoom, pEntity, nMasterDataID)
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

	va_list ap;
	va_start(ap, lStartTime);
	m_pBaeGameRoom = va_arg(ap, BaeGameRoom*);
	va_end(ap);

	m_bContinue = true;
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
	if (!m_bActivated || (m_lLastUpdateTime == lUpdateTime))
		return;

	float fLast = 0, fCur = 0;
	if (m_lStartTime != lUpdateTime)
	{
		fLast = (m_lLastUpdateTime - m_lStartTime) / 1000.0f;
		fCur = (lUpdateTime - m_lStartTime) / 1000.0f;
	}
	
	if (fCur == 0)
	{
		m_pTargetBehavior = m_pEntity->GetBehavior(m_nTargetBehaviorID);
		m_pTargetBehavior->Start(lUpdateTime, m_pBaeGameRoom);
		m_pTargetBehavior->Update(lUpdateTime);
	}
	else
	{
		if (fCur >= m_fLength || !m_bContinue)
		{
			m_pTargetBehavior->Stop(lUpdateTime);
			m_pTargetBehavior = NULL;

			m_bContinue = false;
			m_bActivated = false;
			m_lEndTime = lUpdateTime;
		}
	}

	m_lLastUpdateTime = lUpdateTime;
}

void ContinueSkill::ProcessInput(__int64 lTime, BaeGameRoom* pBaeGameRoom, GameInputSkillToR* pMsg)
{
	if (m_bActivated)
	{
		if (pMsg->m_InputType == GameInputSkillToR::InputType::Release)
			m_bContinue = false;
	}
	else
	{
		if (pMsg->m_InputType == GameInputSkillToR::InputType::Press && IsValidToStart(lTime))
		{
			Start(lTime, pBaeGameRoom);
		}
	}
}