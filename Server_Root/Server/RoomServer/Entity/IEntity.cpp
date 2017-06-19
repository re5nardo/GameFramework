#include "stdafx.h"
#include "IEntity.h"

IEntity::IEntity(int nID, int nMasterDataID)
{
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

	for (list<ISkill*>::iterator it = m_listSkill.begin(); it != m_listSkill.end(); ++it)
	{
		delete *it;
	}
	m_listSkill.clear();
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
}

btVector3 IEntity::GetRotation()
{
	return m_vec3Rotation;
}

void IEntity::SetRotation(btVector3& vec3Rotation)
{
	m_vec3Rotation = vec3Rotation;
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

list<ISkill*> IEntity::GetAllSkills()
{
	return m_listSkill;
}

list<ISkill*> IEntity::GetActivatedSkills()
{
	list<ISkill*> listActivatedSkill;

	for (list<ISkill*>::iterator it = m_listSkill.begin(); it != m_listSkill.end(); ++it)
	{
		if ((*it)->IsActivated())
			listActivatedSkill.push_back(*it);
	}

	return listActivatedSkill;
}

bool IEntity::IsSkilling()
{
	for (list<ISkill*>::iterator it = m_listSkill.begin(); it != m_listSkill.end(); ++it)
	{
		if ((*it)->IsActivated())
			return true;
	}

	return false;
}