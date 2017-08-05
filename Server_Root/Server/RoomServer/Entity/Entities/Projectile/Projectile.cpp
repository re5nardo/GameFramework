#include "stdafx.h"
#include "Projectile.h"
#include "../../../MasterData/MasterDataManager.h"
#include "../../../MasterData/Projectile.h"
#include "../../../Factory.h"

Projectile::Projectile(BaeGameRoom* pGameRoom, int nID, int nMasterDataID) : IEntity(pGameRoom, nID, nMasterDataID)
{
}

Projectile::~Projectile()
{
}

void Projectile::Initialize()
{
	MasterData::Projectile* pMasterProjectile = NULL;
	MasterDataManager::Instance()->GetData<MasterData::Projectile>(m_nMasterDataID, pMasterProjectile);

	SetPosition(btVector3(0, 0, 0));
	SetRotation(btVector3(0, 0, 0));

	for (vector<int>::iterator it = pMasterProjectile->m_vecBehaviorID.begin(); it != pMasterProjectile->m_vecBehaviorID.end(); ++it)
	{
		IBehavior* pBehavior = Factory::Instance()->CreateBehavior(m_pGameRoom, this, *it);
		if (pBehavior != NULL)
		{
			pBehavior->Initialize();
			m_listBehavior.push_back(pBehavior);
		}
	}
}

float Projectile::GetMoveSpeed()
{
	return 1;
}

void Projectile::Update(long long lUpdateTime)
{
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