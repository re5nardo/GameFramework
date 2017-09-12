#include "stdafx.h"
#include "IEntity.h"
#include "../Game/BaeGameRoom.h"

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

void IEntity::AddState(IState* pState)
{
	m_listState.push_back(pState);
}

void IEntity::RemoveState(int nID)
{
	for (list<IState*>::iterator it = m_listState.begin(); it != m_listState.end();)
	{
		if ((*it)->GetID() == nID)
		{
			delete *it;
			it = m_listState.erase(it);
		}
		else
		{
			++it;
		}
	}
}