#include "stdafx.h"
#include "Die.h"
#include "../../Entity/Entities/Projectile/Projectile.h"
#include "../BehaviorIDs.h"
#include "../../MasterData/MasterDataManager.h"
#include "../../MasterData/Behavior.h"
#include "../../Factory.h"
#include "../../Game/BaeGameRoom.h"
#include "../../GameEvent/GameEvents/EntityCreate.h"
#include "../../GameEvent/GameEvents/EntityDestroy.h"
#include "../../Util.h"

const string Die::NAME = "Die";

Die::Die(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID) : IBehavior(pGameRoom, pEntity, nMasterDataID)
{
}

Die::~Die()
{
}

void Die::Initialize()
{
	MasterData::Behavior* pMasterBehavior = NULL;
	MasterDataManager::Instance()->GetData<MasterData::Behavior>(m_nMasterDataID, pMasterBehavior);

	float fValue = 1;
	m_fLength = pMasterBehavior->m_fLength * fValue;
	m_strStringParams = pMasterBehavior->m_strStringParams;
	m_vecAction = pMasterBehavior->m_vecAction;
}

void Die::Start(long long lStartTime, ...)
{
	__super::Start(lStartTime);

	list<IBehavior*> listBehavior = m_pEntity->GetActivatedBehaviors();
	for (list<IBehavior*>::iterator it = listBehavior.begin(); it != listBehavior.end(); ++it)
	{
		IBehavior* pBehavior = *it;
		if (pBehavior->GetMasterDataID() == m_nMasterDataID)
			continue;

		pBehavior->Stop(lStartTime);
	}
}

void Die::UpdateBody(long long lUpdateTime)
{
	if (m_fCurrentTime >= m_fLength)
	{
		Stop(lUpdateTime - (m_fCurrentTime - m_fLength) * 1000);

		m_pGameRoom->CharacterDieEnd(m_pEntity->GetID(), lUpdateTime - (m_fCurrentTime - m_fLength) * 1000);
	}
}