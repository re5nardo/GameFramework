#include "stdafx.h"
#include "IBehavior.h"
#include "../GameEvent/GameEvents/BehaviorStart.h"
#include "../GameEvent/GameEvents/BehaviorEnd.h"
#include "../Entity/IEntity.h"
#include "../Game/BaeGameRoom.h"


IBehavior::IBehavior(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID)
{
	m_pGameRoom = pGameRoom;
	m_pEntity = pEntity;
	m_nMasterDataID = nMasterDataID;
}

IBehavior::~IBehavior()
{
}

int IBehavior::GetMasterDataID()
{
	return m_nMasterDataID;
}

void IBehavior::Start(__int64 lStartTime, ...)
{
	if (m_bActivated)
		return;

	m_bActivated = true;
	m_lStartTime = lStartTime;

	GameEvent::BehaviorStart* pBehaviorStart = new GameEvent::BehaviorStart();
	pBehaviorStart->m_fEventTime = lStartTime / 1000.0f;
	pBehaviorStart->m_nEntityID = m_pEntity->GetID();
	pBehaviorStart->m_fStartTime = lStartTime / 1000.0f;
	pBehaviorStart->m_nBehaviorID = m_nMasterDataID;

	m_pGameRoom->AddGameEvent(pBehaviorStart);
}

float IBehavior::GetTime()
{
	return (float)((m_lLastUpdateTime - m_lStartTime) / 1000.0f);
}

bool IBehavior::IsActivated()
{
	return m_bActivated;
}

void IBehavior::Stop(__int64 lTime)
{
	m_bActivated = false;

	GameEvent::BehaviorEnd* pBehaviorEnd = new GameEvent::BehaviorEnd();
	pBehaviorEnd->m_fEventTime = lTime / 1000.0f;
	pBehaviorEnd->m_nEntityID = m_pEntity->GetID();
	pBehaviorEnd->m_fEndTime = lTime / 1000.0f;
	pBehaviorEnd->m_nBehaviorID = m_nMasterDataID;

	m_pGameRoom->AddGameEvent(pBehaviorEnd);
}