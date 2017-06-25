#include "stdafx.h"
#include "Shield.h"
#include "../../Entity/IEntity.h"
#include "../../MasterData/MasterDataManager.h"
#include "../../MasterData/State.h"

const string Shield::NAME = "Shield";

Shield::Shield(IEntity* pEntity, int nMasterDataID, __int64 lStartTime) : IState(pEntity, nMasterDataID, lStartTime)
{
}

Shield::~Shield()
{
}

int Shield::GetID()
{
	return STATE_ID;
}

void Shield::Initialize()
{
	MasterData::State* pMasterState = NULL;
	MasterDataManager::Instance()->GetData<MasterData::State>(m_nMasterDataID, pMasterState);

	m_fLength = pMasterState->m_fLength;
}

void Shield::Update(__int64 lUpdateTime)
{
	if (m_lLastUpdateTime == lUpdateTime)
		return;

	float fLast = 0, fCur = 0;
	if (m_lStartTime != lUpdateTime)
	{
		fLast = (m_lLastUpdateTime - m_lStartTime) / 1000.0f;
		fCur = (lUpdateTime - m_lStartTime) / 1000.0f;
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