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

bool FireSkill::IsCoolTime(long long lTime)
{
	if (m_lEndTime == -1)
		return false;

	return (lTime - m_lEndTime) / 1000 <= m_fCooltime;
}

bool FireSkill::IsValidToStart(long long lTime)
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

void FireSkill::UpdateBody(long long lUpdateTime)
{
	for (vector<pair<int, float>>::iterator it = m_vecBehavior.begin(); it != m_vecBehavior.end(); ++it)
	{
		float fTime = (*it).second;
		if ((m_fCurrentTime == 0 && fTime == 0) || (m_fPreviousTime < fTime && fTime <= m_fCurrentTime))
		{
			IBehavior* pBehavior = m_pEntity->GetBehavior((*it).first);
			pBehavior->Start(lUpdateTime);
			pBehavior->Update(lUpdateTime);
		}
	}

	for (vector<pair<int, float>>::iterator it = m_vecState.begin(); it != m_vecState.end(); ++it)
	{
		float fTime = (*it).second;
		if ((m_fCurrentTime == 0 && fTime == 0) || (m_fPreviousTime < fTime && fTime <= m_fCurrentTime))
		{
			IState* pState = Factory::Instance()->CreateState(m_pGameRoom, m_pEntity, m_nMasterDataID, lUpdateTime);
			pState->Initialize();
			m_pEntity->AddState(pState);
			pState->Update(lUpdateTime);
		}
	}

	if (m_fCurrentTime >= m_fLength)
	{
		Stop(lUpdateTime - (m_fCurrentTime - m_fLength) * 1000);
	}
}

void FireSkill::ProcessInput(long long lTime, BaeGameRoom* pBaeGameRoom, GameInputSkillToR* pMsg)
{
	if (m_bActivated || pMsg->m_InputType != GameInputSkillToR::InputType::Click)
		return;

	if (!IsValidToStart(lTime))
		return;

	Start(lTime);
}