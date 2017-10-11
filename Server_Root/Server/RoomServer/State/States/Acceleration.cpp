#include "stdafx.h"
#include "Acceleration.h"
#include "../../Entity/IEntity.h"
#include "../../Entity/Entities/Character/Character.h"
#include "../../MasterData/MasterDataManager.h"
#include "../../MasterData/State.h"

const string Acceleration::NAME = "Acceleration";

Acceleration::Acceleration(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID, long long lStartTime) : IState(pGameRoom, pEntity, nMasterDataID, lStartTime)
{
}

Acceleration::~Acceleration()
{
}

int Acceleration::GetID()
{
	return STATE_ID;
}

void Acceleration::Initialize()
{
	MasterData::State* pMasterState = NULL;
	MasterDataManager::Instance()->GetData<MasterData::State>(m_nMasterDataID, pMasterState);

	m_fLength = pMasterState->m_fLength;

	for (int i = 0; i < pMasterState->m_vecDoubleParam1.size(); ++i)
	{
		m_vecEvent.push_back(make_pair(pMasterState->m_vecDoubleParam1[i], pMasterState->m_vecDoubleParam2[i]));
	}
}

void Acceleration::UpdateBody(long long lUpdateTime)
{
	for (int i = 0; i <m_vecEvent.size(); ++i)
	{
		if (m_fPreviousTime < m_vecEvent[i].first && m_vecEvent[i].first <= m_fCurrentTime)
		{
			((Character*)m_pEntity)->PlusMoveSpeed(m_vecEvent[i].second);
		}
	}

	if (m_fCurrentTime >= m_fLength)
	{
		m_pEntity->RemoveState(m_nMasterDataID, lUpdateTime - (m_fCurrentTime - m_fLength) * 1000);
	}
}