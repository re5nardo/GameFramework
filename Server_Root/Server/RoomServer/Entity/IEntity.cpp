#include "stdafx.h"
#include "IEntity.h"

IEntity::IEntity(int nID)
{
	m_nID = nID;
}

IEntity::~IEntity()
{
	for (list<IBehavior*>::iterator it = m_listBehavior.begin(); it != m_listBehavior.end(); ++it)
	{
		delete *it;
	}
	m_listBehavior.clear();
}

int IEntity::GetID()
{
	return m_nID;
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

IBehavior* IEntity::GetBehavior(int nID)
{
	for (list<IBehavior*>::iterator it = m_listBehavior.begin(); it != m_listBehavior.end(); ++it)
	{
		if ((*it)->GetID() == nID)
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