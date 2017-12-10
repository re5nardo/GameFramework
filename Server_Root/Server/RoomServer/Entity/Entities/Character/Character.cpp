#include "stdafx.h"
#include "Character.h"
#include "../../../MasterData/MasterDataManager.h"
#include "../../../MasterData/Behavior.h"
#include "../../../MasterData/Character.h"
#include "../../../Factory.h"
#include "../Projectile/Projectile.h"
#include "../../../Game/BaeGameRoom.h"
#include "../../../Behavior/BehaviorIDs.h"
#include "../../../Behavior/Behaviors/Die.h"
#include "../../../State/StateIDs.h"
#include "../../../GameEvent/GameEvents/CharacterAttack.h"
#include "../../../GameEvent/GameEvents/CharacterRespawn.h"

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

float Character::GetSpeed()
{
	return m_CurrentStatus.m_fSpeed * (m_fSpeedPercent / 100);
}

float Character::GetMaximumSpeed()
{
	return m_CurrentStatus.m_fMaximumSpeed;
}

FBS::Data::EntityType Character::GetEntityType()
{
	return FBS::Data::EntityType::EntityType_Character;
}

void Character::NotifyGameEvent(IGameEvent* pGameEvent)
{
}

bool Character::IsTerrainPassable()
{
	return false;
}

int Character::GetMoveCollisionTypes()
{
	int nTypes = CollisionObject::Type::CollisionObjectType_None;

	if (m_Role == Character::Role::Challenger)
	{
		nTypes = CollisionObject::Type::CollisionObjectType_Terrain | CollisionObject::Type::CollisionObjectType_Character_Disturber | CollisionObject::Type::CollisionObjectType_Projectile
			| CollisionObject::Type::CollisionObjectType_Item;
	}
	else if (m_Role == Character::Role::Disturber)
	{
		nTypes = CollisionObject::Type::CollisionObjectType_Terrain| CollisionObject::Type::CollisionObjectType_Character_Challenger;
	}

	return nTypes;
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

void Character::LateUpdate(long long lUpdateTime)
{
	TrimState();

	if (m_nDefaultBehaviorID != -1 && !IsBehavioring() && GetBehavior(m_nDefaultBehaviorID) != NULL && IsAlive())
	{
		GetBehavior(m_nDefaultBehaviorID)->Start(lUpdateTime);
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

void Character::InitStatus(CharacterStatus status)
{
	m_OriginalStatus = status;
	m_CurrentStatus = status;
}

float Character::GetCurrentMP()
{
	return m_CurrentStatus.m_nMP;
}

void Character::SetCurrentMP(float fMP)
{
	m_CurrentStatus.m_nMP = fMP;
}

void Character::SetMoveSpeed(float fSpeed)
{
	m_CurrentStatus.m_fSpeed = fSpeed;
}

void Character::PlusMoveSpeed(float fValue)
{
	m_CurrentStatus.m_fSpeed += fValue;
}
void Character::MinusMoveSpeed(float fValue)
{
	m_CurrentStatus.m_fSpeed -= fValue;
}

bool Character::IsAlive()
{
	return m_CurrentStatus.m_nHP > 0;
}

void Character::OnMoved(int nDistance)
{
	int nBonusPoint = 100;

	m_CurrentStatus.m_fMovePoint += (nDistance * m_CurrentStatus.m_fMPChargeRate);

	if (m_CurrentStatus.m_fMovePoint >= nBonusPoint)
	{
		int nGetCount = m_CurrentStatus.m_fMovePoint / nBonusPoint;

		m_CurrentStatus.m_fMovePoint -= (nGetCount * nBonusPoint);
		m_CurrentStatus.m_nMP += nGetCount;
	}

	//	GameEvent status change
}

void Character::OnAttacked(int nAttackingEntityID, int nDamage, long long lTime)
{
	if (HasCoreState(CoreState::CoreState_Invincible) || !IsAlive())
		return;

	m_CurrentStatus.m_nHP -= nDamage;

	GameEvent::CharacterAttack* pCharacterAttack = new GameEvent::CharacterAttack();
	pCharacterAttack->m_fEventTime = lTime / 1000.0f;
	pCharacterAttack->m_nAttackingEntityID = nAttackingEntityID;
	pCharacterAttack->m_nAttackedEntityID = m_nID;
	pCharacterAttack->m_nDamage = nDamage;

	m_pGameRoom->AddGameEvent(pCharacterAttack);

	if (m_CurrentStatus.m_nHP <= 0)
	{
		IBehavior* pDieBehavior = GetBehavior(BehaviorID::DIE);

		pDieBehavior->Start(lTime);
		pDieBehavior->Update(lTime);
	}
}

void Character::OnRespawn(long long lTime)	//	last position?
{
	m_CurrentStatus.m_nHP = m_OriginalStatus.m_nHP;

	GameEvent::CharacterRespawn* pCharacterRespawn = new GameEvent::CharacterRespawn();
	pCharacterRespawn->m_fEventTime = lTime / 1000.0f;
	pCharacterRespawn->m_nEntityID = m_nID;
	pCharacterRespawn->m_vec3Position = GetPosition();

	m_pGameRoom->AddGameEvent(pCharacterRespawn);

	IState* pState = Factory::Instance()->CreateState(m_pGameRoom, this, StateID::RespawnInvincible, lTime);
	pState->Initialize();
	AddState(pState, lTime);
	pState->Update(lTime);
}