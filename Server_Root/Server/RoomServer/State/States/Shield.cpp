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

void Shield::UpdateBody(__int64 lUpdateTime)
{
	if (m_fCurrentTime >= m_fLength)
	{
		Remove(lUpdateTime - (m_fCurrentTime - m_fLength) * 1000);
	}
}