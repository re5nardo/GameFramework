#include "stdafx.h"
#include "IState.h"

IState::IState(IEntity* pEntity, int nMasterDataID, __int64 lStartTime)
{
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