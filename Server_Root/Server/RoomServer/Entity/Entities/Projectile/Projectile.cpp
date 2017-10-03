#include "stdafx.h"
#include "Projectile.h"
#include "../../../MasterData/MasterDataManager.h"
#include "../../../MasterData/Projectile.h"
#include "../../../MasterData/Behavior.h"
#include "../../../Factory.h"
#include "../../../Util.h"
#include "../../../GameEvent/GameEvents/BehaviorEnd.h"
#include "../../../Game/BaeGameRoom.h"
#include "../Character/Character.h"

Projectile::Projectile(BaeGameRoom* pGameRoom, int nProjectorID, int nID, int nMasterDataID) : IEntity(pGameRoom, nID, nMasterDataID)
{
	m_nProjectorID = nProjectorID;
}

Projectile::~Projectile()
{
}

void Projectile::Initialize()
{
	MasterData::Projectile* pMasterProjectile = NULL;
	MasterDataManager::Instance()->GetData<MasterData::Projectile>(m_nMasterDataID, pMasterProjectile);

	m_fSize = pMasterProjectile->m_fSize;
	m_fHeight = pMasterProjectile->m_fHeight;
	m_fDefault_Y = pMasterProjectile->m_fDefault_Y;
	m_nDefaultBehaviorID = pMasterProjectile->m_nDefaultBehaviorID;

	for (vector<int>::iterator it = pMasterProjectile->m_vecBehaviorID.begin(); it != pMasterProjectile->m_vecBehaviorID.end(); ++it)
	{
		IBehavior* pBehavior = Factory::Instance()->CreateBehavior(m_pGameRoom, this, *it);
		if (pBehavior != NULL)
		{
			pBehavior->Initialize();
			m_listBehavior.push_back(pBehavior);
		}
	}
	Util::Parse(pMasterProjectile->m_strLifeInfo, ':', &m_vecLifeInfo);
}

float Projectile::GetMoveSpeed()
{
	return 1;
}

FBS::Data::EntityType Projectile::GetEntityType()
{
	return FBS::Data::EntityType::EntityType_Projectile;
}

void Projectile::NotifyGameEvent(IGameEvent* pGameEvent)
{
	if (m_vecLifeInfo[0] == "Arrival" && pGameEvent->GetType() == FBS::GameEventType::GameEventType_BehaviorEnd)
	{
		GameEvent::BehaviorEnd* pBehaviorEnd = (GameEvent::BehaviorEnd*)pGameEvent;

		MasterData::Behavior* pMasterBehavior = NULL;
		MasterDataManager::Instance()->GetData<MasterData::Behavior>(pBehaviorEnd->m_nBehaviorID, pMasterBehavior);

		if (pMasterBehavior->m_strClassName == "Move")
		{
			m_pGameRoom->DestroyEntity(m_nID);
		}
	}
}

bool Projectile::IsMovableOnCollision(IEntity* pOther)
{
	Character* pProjector = GetProjector();
	Character::Role projectorRole = pProjector->GetRole();

	//	if has an almighty state,
	//	return false;

	if (projectorRole == Character::Role::Challenger)
	{

	}
	else if (projectorRole == Character::Role::Disturber)
	{

	}

	return true;
}

void Projectile::OnCollision(IEntity* pOther, long long lTime)
{
	Character* pProjector = GetProjector();
	Character::Role projectorRole = pProjector->GetRole();

	//	if has an almighty state,
	//	return "Ignore";

	if (projectorRole == Character::Role::Challenger)
	{

	}
	else if (projectorRole == Character::Role::Disturber)
	{
	
	}
}

bool Projectile::IsTerrainPassable()
{
	return false;
}

int Projectile::GetProjectorID()
{
	return m_nProjectorID;
}

Character* Projectile::GetProjector()
{
	return (Character*)m_pGameRoom->GetEntity(m_nProjectorID);
}