#include "stdafx.h"
#include "Item.h"
#include "../../../MasterData/MasterDataManager.h"
#include "../../../MasterData/Item.h"
#include "../../../Factory.h"
#include "../../../Game/CollisionObject.h"
#include "../../../Game/BaeGameRoom.h"
#include "../../../Util.h"

Item::Item(BaeGameRoom* pGameRoom, long long lTime, int nID, int nMasterDataID) : IEntity(pGameRoom, nID, nMasterDataID)
{
	m_lSpawnedTime = lTime;
}

Item::~Item()
{
}

void Item::Initialize()
{
	MasterData::Item* pMasterItem = NULL;
	MasterDataManager::Instance()->GetData<MasterData::Item>(m_nMasterDataID, pMasterItem);

	m_fSize = pMasterItem->m_fSize;
	m_fHeight = pMasterItem->m_fHeight;
	m_fDefault_Y = pMasterItem->m_fDefault_Y;
	m_nDefaultBehaviorID = pMasterItem->m_nDefaultBehaviorID;

	for (vector<int>::iterator it = pMasterItem->m_vecBehaviorID.begin(); it != pMasterItem->m_vecBehaviorID.end(); ++it)
	{
		IBehavior* pBehavior = Factory::Instance()->CreateBehavior(m_pGameRoom, this, *it);
		if (pBehavior != NULL)
		{
			pBehavior->Initialize();
			m_listBehavior.push_back(pBehavior);
		}
	}

	m_fLifespan = pMasterItem->m_fLifespan;
	m_strEffectType = pMasterItem->m_strEffectType;
	m_nEffectParam = pMasterItem->m_nEffectParam;
}

float Item::GetSpeed()
{
	return 0;
}

float Item::GetMaximumSpeed()
{
	return 0;
}

FBS::Data::EntityType Item::GetEntityType()
{
	return FBS::Data::EntityType::EntityType_Item;
}

void Item::NotifyGameEvent(IGameEvent* pGameEvent)
{
}

bool Item::IsTerrainPassable()
{
	return false;
}

int Item::GetMoveCollisionTypes()
{
	return CollisionObject::Type::CollisionObjectType_Character_Challenger;
}

int Item::GetAttackTargetTypes()
{
	return CollisionObject::Type::CollisionObjectType_None;
}

void Item::LateUpdate(long long lUpdateTime)
{
	__super::LateUpdate(lUpdateTime);

	if (m_lSpawnedTime + m_fLifespan * 1000 <= lUpdateTime)
	{
		m_pGameRoom->DestroyEntity(m_nID);
	}
}

void Item::OnCollision(IEntity* pOther, long long lTime)
{
	__super::OnCollision(pOther, lTime);

	if (m_strEffectType == "AddState")
	{
		if (pOther->GetState(m_nEffectParam) != NULL)
		{
			pOther->RemoveState(m_nEffectParam, lTime);
		}

		IState* pState = Factory::Instance()->CreateState(m_pGameRoom, pOther, m_nEffectParam, lTime);
		pState->Initialize();
		pOther->AddState(pState, lTime);
		pState->Update(lTime);
	}
	else if (m_strEffectType == "MPCharge")
	{
		m_pGameRoom->AddCharacterStatusChangeGameEvent(lTime / 1000.0f, pOther->GetID(), "MP", "Item", 1);
	}

	m_pGameRoom->DestroyEntity(m_nID);
}