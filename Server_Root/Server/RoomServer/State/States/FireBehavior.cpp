#include "stdafx.h"
#include "FireBehavior.h"
#include "../../Entity/IEntity.h"
#include "../../MasterData/MasterDataManager.h"
#include "../../MasterData/State.h"

const string FireBehavior::NAME = "Shield";

FireBehavior::FireBehavior(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID, long long lStartTime) : IState(pGameRoom, pEntity, nMasterDataID, lStartTime)
{
}

FireBehavior::~FireBehavior()
{
}

int FireBehavior::GetID()
{
	return STATE_ID;
}

void FireBehavior::Initialize()
{
	MasterData::State* pMasterState = NULL;
	MasterDataManager::Instance()->GetData<MasterData::State>(m_nMasterDataID, pMasterState);

	m_fLength = pMasterState->m_fLength;
	m_nTargetBehaviorID = pMasterState->m_vecDoubleParam1[0];
}

void FireBehavior::UpdateBody(long long lUpdateTime)
{
	if (m_fCurrentTime >= m_fLength)
	{
		IBehavior* pTargetBehavior = m_pEntity->GetBehavior(m_nTargetBehaviorID);
		pTargetBehavior->Start(lUpdateTime, m_pGameRoom);
		pTargetBehavior->Update(lUpdateTime);

		m_pEntity->RemoveState(m_nMasterDataID, lUpdateTime - (m_fCurrentTime - m_fLength) * 1000);
	}
}