#include "stdafx.h"
#include "FireSkill.h"
#include "../../Entity/IEntity.h"
#include "../../Entity/Entities/Character/Character.h"
#include "../../MasterData/MasterDataManager.h"
#include "../../MasterData/Skill.h"
#include "../../Factory.h"
#include "../../Messages/ToRoom/GameInputSkillToR.h"

const string FireSkill::NAME = "FireSkill";

FireSkill::FireSkill(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID) : ISkill(pGameRoom, pEntity, nMasterDataID)
{
}

FireSkill::~FireSkill()
{
}

bool FireSkill::IsCoolTime(__int64 lTime)
{
	if (m_lEndTime == -1)
		return false;

	return (lTime - m_lEndTime) / 1000 <= m_fCooltime;
}

bool FireSkill::IsValidToStart(__int64 lTime)
{
	return !IsCoolTime(lTime) && ((Character*)m_pEntity)->GetCurrentMP() >= m_fMP;
}

void FireSkill::Initialize()
{
	MasterData::Skill* pMasterSkill = NULL;
	MasterDataManager::Instance()->GetData<MasterData::Skill>(m_nMasterDataID, pMasterSkill);

	m_vecBehavior = pMasterSkill->m_vecBehavior;
	m_vecState = pMasterSkill->m_vecState;
	m_fLength = pMasterSkill->m_fLength;
	m_fCooltime = pMasterSkill->m_fCoolTime;
	m_fMP = pMasterSkill->m_fMP;
}

void FireSkill::Update(__int64 lUpdateTime)
{
	if (!m_bActivated || (m_lLastUpdateTime == lUpdateTime))
		return;

	float fLast = 0, fCur = 0;
	if (m_lStartTime != lUpdateTime)
	{
		fLast = (m_lLastUpdateTime - m_lStartTime) / 1000.0f;
		fCur = (lUpdateTime - m_lStartTime) / 1000.0f;
	}

	for (vector<pair<int, float>>::iterator it = m_vecBehavior.begin(); it != m_vecBehavior.end(); ++it)
	{
		float fTime = (*it).second;
		if ((fCur == 0 && fTime == 0) || (fLast < fTime && fTime <= fCur))
		{
			IBehavior* pBehavior = m_pEntity->GetBehavior((*it).first);
			pBehavior->Start(lUpdateTime);
			pBehavior->Update(lUpdateTime);
		}
	}

	for (vector<pair<int, float>>::iterator it = m_vecState.begin(); it != m_vecState.end(); ++it)
	{
		float fTime = (*it).second;
		if ((fCur == 0 && fTime == 0) || (fLast < fTime && fTime <= fCur))
		{
			IState* pState = Factory::Instance()->CreateState(m_pGameRoom, m_pEntity, m_nMasterDataID, lUpdateTime);
			pState->Initialize();
			m_pEntity->AddState(pState);
			pState->Update(lUpdateTime);
		}
	}

	if (fCur >= m_fLength)
	{
		m_bActivated = false;
		m_lEndTime = lUpdateTime;
	}

	m_lLastUpdateTime = lUpdateTime;
}

void FireSkill::ProcessInput(__int64 lTime, BaeGameRoom* pBaeGameRoom, GameInputSkillToR* pMsg)
{
	if (m_bActivated || pMsg->m_InputType != GameInputSkillToR::InputType::Click)
		return;

	if (!IsValidToStart(lTime))
		return;

	Start(lTime);
}