#pragma once

#include <list>
#include "../Behavior/IBehavior.h"
#include "btBulletCollisionCommon.h"

using namespace std;

class IEntity
{
public:
	IEntity(int nID);
	virtual ~IEntity();

private:
	int m_nID;

private:
	btVector3 m_vec3Position = btVector3(0, 0, 0);
	btVector3 m_vec3Rotation = btVector3(0, 0, 0);

protected:
	list<IBehavior*> m_listBehavior;

protected:
	virtual void InitializeBehavior() = 0;

public:
	virtual float GetSpeed() = 0;

public:
	int GetID();

	btVector3 GetPosition();
	void SetPosition(btVector3& vec3Position);

	btVector3 GetRotation();
	void SetRotation(btVector3& vec3Rotation);

	IBehavior* GetBehavior(int nID);
	list<IBehavior*> GetAllBehaviors();
	list<IBehavior*> GetActivatedBehaviors();
	bool IsBehavioring();
};