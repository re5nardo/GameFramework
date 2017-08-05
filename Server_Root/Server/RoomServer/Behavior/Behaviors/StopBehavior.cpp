#include "stdafx.h"
#include "StopBehavior.h"
#include "../../Entity/IEntity.h"

const string StopBehavior::NAME = "StopBehavior";

StopBehavior::StopBehavior(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID) : IBehavior(pGameRoom, pEntity, nMasterDataID)
{
}

StopBehavior::~StopBehavior()
{
}

void StopBehavior::Start(long long lStartTime, ...)
{
	__super::Start(lStartTime);

	list<IBehavior*> listBehavior = m_pEntity->GetActivatedBehaviors();
	for (list<IBehavior*>::iterator it = listBehavior.begin(); it != listBehavior.end(); ++it)
	{
		(*it)->Stop(lStartTime);
	}
}

void StopBehavior::Initialize()
{
}

void StopBehavior::UpdateBody(long long lUpdateTime)
{
	if (m_fCurrentTime >= m_fLength)
	{
		Stop(lUpdateTime - (m_fCurrentTime - m_fLength) * 1000);
	}
}