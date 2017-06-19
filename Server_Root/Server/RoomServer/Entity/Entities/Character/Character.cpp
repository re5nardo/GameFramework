#include "stdafx.h"
#include "Character.h"
#include "../../../MasterData/MasterDataManager.h"
#include "../../../MasterData/Behavior.h"
#include "../../../MasterData/Character.h"
#include "../../../Factory.h"

Character::Character(int nID, int nMasterDataID) : IEntity(nID, nMasterDataID)
{
}

Character::~Character()
{
}

void Character::Initialize()
{
	MasterData::Character* pMasterCharacter = NULL;
	MasterDataManager::Instance()->GetData<MasterData::Character>(m_nMasterDataID, pMasterCharacter);

	SetStat(Stat(pMasterCharacter->m_fMoveSpeed, pMasterCharacter->m_fHP, pMasterCharacter->m_fMP));
	SetPosition(btVector3(0, 0, 0));
	SetRotation(btVector3(0, 0, 0));

	for (vector<int>::iterator it = pMasterCharacter->m_vecSkillID.begin(); it != pMasterCharacter->m_vecSkillID.end(); ++it)
	{
		ISkill* pSkill = Factory::Instance()->CreateSkill(this, *it);
		if (pSkill != NULL)
		{
			pSkill->Initialize();
			m_listSkill.push_back(pSkill);
		}
	}

	for (vector<int>::iterator it = pMasterCharacter->m_vecBehaviorID.begin(); it != pMasterCharacter->m_vecBehaviorID.end(); ++it)
	{
		IBehavior* pBehavior = Factory::Instance()->CreateBehavior(this, *it);
		if (pBehavior != NULL)
		{
			pBehavior->Initialize();
			m_listBehavior.push_back(pBehavior);
		}
	}
}

void Character::Update(__int64 lUpdateTime)
{
	list<ISkill*> listSkill = GetActivatedSkills();
	for (list<ISkill*>::iterator it = listSkill.begin(); it != listSkill.end(); ++it)
	{
		ISkill* pSkill = *it;
		if (pSkill != NULL)
			pSkill->Update(lUpdateTime);
	}

	list<IState*> listState = GetStates();
	for (list<IState*>::iterator it = listState.begin(); it != listState.end(); ++it)
	{
		IState* pState = *it;
		if (pState != NULL)
			pState->Update(lUpdateTime);
	}

	list<IBehavior*> listBehavior = GetActivatedBehaviors();
	for (list<IBehavior*>::iterator it = listBehavior.begin(); it != listBehavior.end(); ++it)
	{
		IBehavior* pBehavior = *it;
		if (pBehavior != NULL)
			pBehavior->Update(lUpdateTime);
	}
}

float Character::GetMoveSpeed()
{
	return m_CurrentStat.m_fMoveSpeed * (fMoveSpeedPercent / 100);
}

void Character::SetStat(Stat stat)
{
	m_DefaultStat = stat;
	m_CurrentStat = stat;
}

float Character::GetCurrentMP()
{
	return m_CurrentStat.m_nMP;
}

void Character::SetCurrentMP(float fMP)
{
	m_CurrentStat.m_nMP = fMP;
}

void Character::PlusMoveSpeed(float fValue)
{
	m_CurrentStat.m_fMoveSpeed += fValue;
}
void Character::MinusMoveSpeed(float fValue)
{
	m_CurrentStat.m_fMoveSpeed -= fValue;
}