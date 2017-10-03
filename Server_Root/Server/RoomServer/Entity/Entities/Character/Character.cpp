#include "stdafx.h"
#include "Character.h"
#include "../../../MasterData/MasterDataManager.h"
#include "../../../MasterData/Behavior.h"
#include "../../../MasterData/Character.h"
#include "../../../Factory.h"
#include "../Projectile/Projectile.h"
#include "../../../GameEvent/GameEvents/Collision.h"
#include "../../../Game/BaeGameRoom.h"

Character::Character(BaeGameRoom* pGameRoom, int nID, int nMasterDataID, Role role) : IEntity(pGameRoom, nID, nMasterDataID)
{
	m_Role = role;
}

Character::~Character()
{
	for (list<ISkill*>::iterator it = m_listSkill.begin(); it != m_listSkill.end(); ++it)
	{
		delete *it;
	}
	m_listSkill.clear();
}

void Character::SetRole(Role role)
{
	m_Role = role;
}

Character::Role Character::GetRole()
{
	return m_Role;
}

void Character::Initialize()
{
	MasterData::Character* pMasterCharacter = NULL;
	MasterDataManager::Instance()->GetData<MasterData::Character>(m_nMasterDataID, pMasterCharacter);

	SetStat(Stat(pMasterCharacter->m_fMoveSpeed, pMasterCharacter->m_fHP, pMasterCharacter->m_fMP));

	m_fSize = pMasterCharacter->m_fSize;
	m_fHeight = pMasterCharacter->m_fHeight;
	m_fDefault_Y = pMasterCharacter->m_fDefault_Y;
	m_nDefaultBehaviorID = pMasterCharacter->m_nDefaultBehaviorID;

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

bool Character::IsMovableOnCollision(IEntity* pOther)
{
	//	if has an almighty state,
	//	return true;

	if (m_Role == Character::Role::Challenger)
	{
		if (pOther->GetEntityType() == FBS::Data::EntityType::EntityType_Character)
		{
			Character* pCharacter = (Character*)pOther;
			if (pCharacter->GetRole() == Character::Role::Challenger)
			{
				//	if pCharacter has an destroyer state,
				//	return false;
			}
			else if (pCharacter->GetRole() == Character::Role::Disturber)
			{
				return false;
			}
		}
		else if (pOther->GetEntityType() == FBS::Data::EntityType::EntityType_Projectile)
		{
			Projectile* pProjectile = (Projectile*)pOther;
			//	if pProjectile has an destroyer state,
			//	return false;
		}
	}

	return true;
}

void Character::OnCollision(IEntity* pOther, long long lTime)
{
	//	if has an almighty state,
	//	return "Ignore";

	if (m_Role == Character::Role::Challenger)
	{
		if (pOther->GetEntityType() == FBS::Data::EntityType::EntityType_Character)
		{
			Character* pCharacter = (Character*)pOther;
			if (pCharacter->GetRole() == Character::Role::Challenger)
			{
				//	if pCharacter has an destroyer state,
				//	Stop All Behavior
				//	...
				//	GameEvent::Collision* pCollision = new GameEvent::Collision();
				//	...
			}
			else if (pCharacter->GetRole() == Character::Role::Disturber)
			{
				//	Stop All Behavior
				//	...

				GameEvent::Collision* pCollision = new GameEvent::Collision();
				pCollision->m_fEventTime = lTime / 1000.0f;
				pCollision->m_nEntityID = m_nID;
				pCollision->m_vec3Position = m_vec3Position;

				m_pGameRoom->AddGameEvent(pCollision);
			}
		}
		else if (pOther->GetEntityType() == FBS::Data::EntityType::EntityType_Projectile)
		{
			Projectile* pProjectile = (Projectile*)pOther;
			//	if pProjectile has an destroyer state,
			//	Stop All Behavior
			//	...
			//	GameEvent::Collision* pCollision = new GameEvent::Collision();
			//	...
		}
	}
	else if (m_Role == Character::Role::Disturber)
	{
		//return "Ignore";
	}
}

bool Character::IsTerrainPassable()
{
	return false;
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