#include "stdafx.h"
#include "Idle.h"

const string Idle::NAME = "Idle";

Idle::Idle(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID) : IBehavior(pGameRoom, pEntity, nMasterDataID)
{
}

Idle::~Idle()
{
}

void Idle::Initialize()
{
}

void Idle::Update(__int64 lUpdateTime)
{
	if (!m_bActivated || (m_lLastUpdateTime == lUpdateTime))
		return;

	m_lLastUpdateTime = lUpdateTime;
}