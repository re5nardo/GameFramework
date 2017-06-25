#include "stdafx.h"
#include "Acceleration.h"
#include "../../Entity/IEntity.h"
#include "../../Entity/Entities/Character/Character.h"
#include "../../MasterData/MasterDataManager.h"
#include "../../MasterData/State.h"

const string Acceleration::NAME = "Acceleration";

Acceleration::Acceleration(IEntity* pEntity, int nMasterDataID, __int64 lStartTime) : IState(pEntity, nMasterDataID, lStartTime)
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

void Acceleration::Update(__int64 lUpdateTime)
{
	if (m_lLastUpdateTime == lUpdateTime)
		return;

	float fLast = 0, fCur = 0;
	if (m_lStartTime != lUpdateTime)
	{
		fLast = (m_lLastUpdateTime - m_lStartTime) / 1000.0f;
		fCur = (lUpdateTime - m_lStartTime) / 1000.0f;
	}

	for (int i = 0; i <m_vecEvent.size(); ++i)
	{
		if (fLast < m_vecEvent[i].first && m_vecEvent[i].first <= fCur)
		{
			((Character*)m_pEntity)->PlusMoveSpeed(m_vecEvent[i].second);
		}
	}

	if (fCur >= m_fLength)
	{
		m_pEntity->RemoveState(STATE_ID);
	}
	else
	{
		m_lLastUpdateTime = lUpdateTime;
	}
}