#include "stdafx.h"
#include "DashState.h"
#include "../../Entity/IEntity.h"
#include "../../MasterData/MasterDataManager.h"
#include "../../MasterData/State.h"

const string DashState::NAME = "DashState";

DashState::DashState(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID, long long lStartTime) : IState(pGameRoom, pEntity, nMasterDataID, lStartTime)
{
	m_lExpiredTime = lStartTime + 200;
}

DashState::~DashState()
{
}

int DashState::GetID()
{
	return STATE_ID;
}

void DashState::Initialize()
{
}

void DashState::UpdateBody(long long lUpdateTime)
{
	if (m_fDeltaTime == 0)
		return;

	if (lUpdateTime >= m_lExpiredTime)
	{
		m_pEntity->RemoveState(m_nMasterDataID, m_lExpiredTime);
	}
}

void DashState::Prolong(long long lTime)
{
	m_lExpiredTime = lTime + 200;
}