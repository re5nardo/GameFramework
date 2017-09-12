#include "stdafx.h"
#include "Character.h"
#include "../../../MasterData/MasterDataManager.h"
#include "../../../MasterData/Behavior.h"
#include "../../../MasterData/Character.h"
#include "../../../Factory.h"

Character::Character(BaeGameRoom* pGameRoom, int nID, int nMasterDataID) : IEntity(pGameRoom, nID, nMasterDataID)
{
}

Character::~Character()
{
	for (list<ISkill*>::iterator it = m_listSkill.begin(); it != m_listSkill.end(); ++it)
	{
		delete *it;
	}
	m_listSkill.clear();
}

void Character::Initialize()
{
	MasterData::Character* pMasterCharacter = NULL;
	MasterDataManager::Instance()->GetData<MasterData::Character>(m_nMasterDataID, pMasterCharacter);

	SetStat(Stat(pMasterCharacter->m_fMoveSpeed, pMasterCharacter->m_fHP, pMasterCharacter->m_fMP));

	m_fSize = pMasterCharacter->m_fSize;
	m_fHeight = pMasterCharacter->m_fHeight;
	m_fDefault_Y = pMasterCharacter->m_fDefault_Y;

	for (vector<int>::iterator it = pMasterCharacter->m_vecSkillID.begin(); it != pMasterCharacter->m_vecSkillID.end(); ++it)
	{
		ISkill* pSkill = Factory::Instance()->CreateSkill(m_pGameRoom, this, *it);
		if (pSkill != NULL)
		{
			pSkill->Initialize();
			m_listSkill.push_back(pSkill);
		}
	}

	for (vector<int>::iterator it = pMasterCharacter->m_vecBehaviorID.begin(); it != pMasterCharacter->m_vecBehaviorID.end(); ++it)
	{
		IBehavior* pBehavior = Factory::Instance()->CreateBehavior(m_pGameRoom, this, *it);
		if (pBehavior != NULL)
		{
			pBehavior->Initialize();
			m_listBehavior.push_back(pBehavior);
		}
	}
}

float Character::GetMoveSpeed()
{
	return m_CurrentStat.m_fMoveSpeed * (fMoveSpeedPercent / 100);
}

FBS::Data::EntityType Character::GetEntityType()
{
	return FBS::Data::EntityType::EntityType_Character;
}

void Character::NotifyGameEvent(IGameEvent* pGameEvent)
{
}

void Character::UpdateSkills(long long lUpdateTime)
{
	list<ISkill*> listSkill = GetActivatedSkills();
	for (list<ISkill*>::iterator it = listSkill.begin(); it != listSkill.end(); ++it)
	{
		ISkill* pSkill = *it;
		if (pSkill != NULL)
			pSkill->Update(lUpdateTime);

		if (m_bDestroyReserved)
			break;
	}
}

list<ISkill*> Character::GetAllSkills()
{
	return m_listSkill;
}

list<ISkill*> Character::GetActivatedSkills()
{
	list<ISkill*> listActivatedSkill;

	for (list<ISkill*>::iterator it = m_listSkill.begin(); it != m_listSkill.end(); ++it)
	{
		if ((*it)->IsActivated())
			listActivatedSkill.push_back(*it);
	}

	return listActivatedSkill;
}

bool Character::IsSkilling()
{
	for (list<ISkill*>::iterator it = m_listSkill.begin(); it != m_listSkill.end(); ++it)
	{
		if ((*it)->IsActivated())
			return true;
	}

	return false;
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