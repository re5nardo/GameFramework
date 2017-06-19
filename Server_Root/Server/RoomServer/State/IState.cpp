#include "stdafx.h"
#include "IState.h"

IState::IState(IEntity* pEntity, __int64 lStartTime)
{
	m_pEntity = pEntity;

	m_lStartTime = lStartTime;
	m_lLastUpdateTime = lStartTime;
}

IState::~IState()
{
}