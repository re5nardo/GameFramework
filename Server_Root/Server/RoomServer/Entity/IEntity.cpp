#include "stdafx.h"
#include "IEntity.h"
#include "../Game/BaeGameRoom.h"
#include "../Behavior/BehaviorIDs.h"
#include "../GameEvent/GameEvents/StateStart.h"
#include "../GameEvent/GameEvents/StateEnd.h"

IEntity::IEntity(BaeGameRoom* pGameRoom, int nID, int nMasterDataID)
{
	m_pGameRoom = pGameRoom;
	m_nID = nID;
	m_nMasterDataID = nMasterDataID;
}

IEntity::~IEntity()
{
	for (list<IBehavior*>::iterator it = m_listBehavior.begin(); it != m_listBehavior.end(); ++it)
	{
		delete *it;
	}
	m_listBehavior.clear();

	for (list<IState*>::iterator it = m_listState.begin(); it != m_listState.end(); ++it)
	{
		delete *it;
	}
	m_listState.clear();

	TrimState();
}

void IEntity::UpdateBehaviors(long long lUpdateTime)
{
	list<IBehavior*> listBehavior = GetActivatedBehaviors();
	for (list<IBehavior*>::iterator it = listBehavior.begin(); it != listBehavior.end(); ++it)
	{
		IBehavior* pBehavior = *it;
		if (pBehavior != NULL)
			pBehavior->Update(lUpdateTime);

		if (m_bDestroyReserved)
			break;
	}
}

void IEntity::UpdateStates(long long lUpdateTime)
{
	list<IState*> listState = GetStates();
	for (list<IState*>::iterator it = listState.begin(); it != listState.end(); ++it)
	{
		IState* pState = *it;
		if (pState != NULL)
			pState->Update(lUpdateTime);

		if (m_bDestroyReserved)
			break;
	}
}

void IEntity::LateUpdate(long long lUpdateTime)
{
	TrimState();

	if (m_nDefaultBehaviorID != -1 && !IsBehavioring() && GetBehavior(m_nDefaultBehaviorID) != NULL)
	{
		GetBehavior(m_nDefaultBehaviorID)->Start(lUpdateTime);
	}
}

int IEntity::GetID()
{
	return m_nID;
}

int IEntity::GetMasterDataID()
{
	return m_nMasterDataID;
}

btVector3 IEntity::GetPosition()
{
	return m_vec3Position;
}

void IEntity::SetPosition(btVector3& vec3Position)
{
	m_vec3Position = vec3Position;

	m_pGameRoom->SetCollisionObjectPosition(m_pGameRoom->GetCollisionObjectIDByEntityID(m_nID), vec3Position);
}

btVector3 IEntity::GetRotation()
{
	return m_vec3Rotation;
}

void IEntity::SetRotation(btVector3& vec3Rotation)
{
	m_vec3Rotation = vec3Rotation;

	m_pGameRoom->SetCollisionObjectRotation(m_pGameRoom->GetCollisionObjectIDByEntityID(m_nID), m_vec3Rotation);
}

float IEntity::GetSize()
{
	return m_fSize;
}

float IEntity::GetHeight()
{
	return m_fHeight;
}

float IEntity::GetDefault_Y()
{
	return m_fDefault_Y;
}

IBehavior* IEntity::GetBehavior(int nMasterDataID)
{
	for (list<IBehavior*>::iterator it = m_listBehavior.begin(); it != m_listBehavior.end(); ++it)
	{
		if ((*it)->GetMasterDataID() == nMasterDataID)
			return *it;
	}

	return NULL;
}

list<IBehavior*> IEntity::GetAllBehaviors()
{
	return m_listBehavior;
}

list<IBehavior*> IEntity::GetActivatedBehaviors()
{
	list<IBehavior*> listActivatedBehavior;

	for (list<IBehavior*>::iterator it = m_listBehavior.begin(); it != m_listBehavior.end(); ++it)
	{
		if ((*it)->IsActivated())
			listActivatedBehavior.push_back(*it);
	}

	return listActivatedBehavior;
}

bool IEntity::IsBehavioring()
{
	for (list<IBehavior*>::iterator it = m_listBehavior.begin(); it != m_listBehavior.end(); ++it)
	{
		if ((*it)->IsActivated())
			return true;
	}

	return false;
}

IState* IEntity::GetState(int nID)
{
	for (list<IState*>::iterator it = m_listState.begin(); it != m_listState.end(); ++it)
	{
		if ((*it)->GetID() == nID)
			return *it;
	}

	return NULL;
}

list<IState*> IEntity::GetStates()
{
	return m_listState;
}

void IEntity::AddState(IState* pState, long long lTime)
{
	GameEvent::StateStart* pStateStart = new GameEvent::StateStart();
	pStateStart->m_fEventTime = lTime / 1000.0f;
	pStateStart->m_nEntityID = m_nID;
	pStateStart->m_fStartTime = pState->GetStartTime() / 1000.0f;
	pStateStart->m_nStateID = pState->GetMasterDataID();

	m_pGameRoom->AddGameEvent(pStateStart);

	m_listState.push_back(pState);
}

void IEntity::RemoveState(int nMasterDataID, long long lTime)
{
	for (list<IState*>::iterator it = m_listState.begin(); it != m_listState.end();)
	{
		IState* pTarget = *it;

		if (pTarget->GetMasterDataID() == nMasterDataID)
		{
			GameEvent::StateEnd* pStateEnd = new GameEvent::StateEnd();
			pStateEnd->m_fEventTime = lTime / 1000.0f;
			pStateEnd->m_nEntityID = m_nID;
			pStateEnd->m_fEndTime = lTime / 1000.0f;
			pStateEnd->m_nStateID = pTarget->GetMasterDataID();

			m_pGameRoom->AddGameEvent(pStateEnd);

			pTarget->m_bDestroyReserved = true;
			m_listDestroyReserved.push_back(pTarget);

			m_listState.erase(it);

			return;
		}
		else
		{
			++it;
		}
	}
}

bool IEntity::HasCoreState(CoreState coreState)
{
	for (list<IState*>::iterator it = m_listState.begin(); it != m_listState.end(); ++it)
	{
		if ((*it)->HasCoreState(coreState))
			return true;
	}

	return false;
}

void IEntity::OnCollision(IEntity* pOther, long long lTime)
{
	for (list<IBehavior*>::iterator it = m_listBehavior.begin(); it != m_listBehavior.end(); ++it)
	{
		(*it)->OnCollision(pOther, lTime);
	}

	for (list<IState*>::iterator it = m_listState.begin(); it != m_listState.end(); ++it)
	{
		(*it)->OnCollision(pOther, lTime);
	}
}

void IEntity::TrimState()
{
	for (list<IState*>::iterator it = m_listDestroyReserved.begin(); it != m_listDestroyReserved.end(); ++it)
	{
		delete *it;
	}
	m_listDestroyReserved.clear();
}