#include "stdafx.h"
#include "GeneralState.h"
#include "../../Entity/IEntity.h"
#include "../../MasterData/MasterDataManager.h"
#include "../../MasterData/State.h"

const string GeneralState::NAME = "GeneralState";

GeneralState::GeneralState(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID, long long lStartTime) : IState(pGameRoom, pEntity, nMasterDataID, lStartTime)
{
}

GeneralState::~GeneralState()
{
}

int GeneralState::GetID()
{
	return STATE_ID;
}

void GeneralState::Initialize()
{
	MasterData::State* pMasterState = NULL;
	MasterDataManager::Instance()->GetData<MasterData::State>(m_nMasterDataID, pMasterState);

	m_fLength = pMasterState->m_fLength;

	for (vector<string>::iterator it = pMasterState->m_vecCoreState.begin(); it != pMasterState->m_vecCoreState.end(); ++it)
	{
		if ((*it) == "Invincible")
		{
			m_vecCoreState.push_back(CoreState::CoreState_Invincible);
		}
	}
}

void GeneralState::UpdateBody(long long lUpdateTime)
{
	if (m_fCurrentTime >= m_fLength)
	{
		m_pEntity->RemoveState(m_nMasterDataID, lUpdateTime - (m_fCurrentTime - m_fLength) * 1000);
	}
}