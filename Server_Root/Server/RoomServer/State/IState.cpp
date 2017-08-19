#include "stdafx.h"
#include "IState.h"
#include "../Entity/IEntity.h"

IState::IState(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID, long long lStartTime)
{
	m_pGameRoom = pGameRoom;
	m_pEntity = pEntity;
	m_nMasterDataID = nMasterDataID;
	m_lStartTime = lStartTime;
}

IState::~IState()
{
}

int IState::GetMasterDataID()
{
	return m_nMasterDataID;
}

void IState::Update(long long lUpdateTime)
{
	if (m_lLastUpdateTime == lUpdateTime)
		return;

	if (m_lStartTime == lUpdateTime)
	{
		m_fPreviousTime = 0;
		m_fCurrentTime = 0;
		m_fDeltaTime = 0;
	}
	else
	{
		m_fPreviousTime = (m_lLastUpdateTime - m_lStartTime) / 1000.0f;
		m_fCurrentTime = (lUpdateTime - m_lStartTime) / 1000.0f;
		m_fDeltaTime = m_fCurrentTime - m_fPreviousTime;
	}

	UpdateBody(lUpdateTime);

	m_lLastUpdateTime = lUpdateTime;
}

void IState::Remove(long long lTime)
{
	//	State End GameEvent..

	m_pEntity->RemoveState(GetID());
}