#include "stdafx.h"
#include "IState.h"
#include "../Entity/IEntity.h"

IState::IState(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID, long long lStartTime)
{
	m_pGameRoom = pGameRoom;
	m_pEntity = pEntity;
	m_nMasterDataID = nMasterDataID;
	m_lStartTime = lStartTime;
	m_lLastUpdateTime = -1;
}

IState::~IState()
{
}

int IState::GetMasterDataID()
{
	return m_nMasterDataID;
}

long long IState::GetStartTime()
{
	return m_lStartTime;
}

void IState::Update(long long lUpdateTime)
{
	if (m_lLastUpdateTime == lUpdateTime)
		return;

	if (m_lLastUpdateTime == -1)
	{
		m_lLastUpdateTime = m_lStartTime;
	}

	m_fPreviousTime = (m_lLastUpdateTime - m_lStartTime) / 1000.0f;
	m_fCurrentTime = (lUpdateTime - m_lStartTime) / 1000.0f;
	m_fDeltaTime = m_fCurrentTime - m_fPreviousTime;

	UpdateBody(lUpdateTime);

	m_lLastUpdateTime = lUpdateTime;
}

bool IState::HasCoreState(CoreState coreState)
{
	for (vector<CoreState>::iterator it = m_vecCoreState.begin(); it != m_vecCoreState.end(); ++it)
	{
		if ((*it) == coreState)
			return true;
	}
	
	return false;
}

void IState::OnCollision(IEntity* pOther, long long lTime)
{
}