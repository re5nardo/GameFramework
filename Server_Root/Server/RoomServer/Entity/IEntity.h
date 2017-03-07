#pragma once

#include <list>
#include "../Behavior/IBehavior.h"
#include "../Data.h"

using namespace std;

class IEntity
{
public:
	IEntity();
	virtual ~IEntity();

private:
	Vector3 m_vec3Position;
	Vector3 m_vec3Rotation;

protected:
	list<IBehavior*> m_listBehavior;

protected:
	virtual void InitializeBehavior() = 0;

public:
	virtual float GetSpeed() = 0;

public:
	Vector3 GetPosition();
	void SetPosition(Vector3 vec3Position);

	Vector3 GetRotation();
	void SetRotation(Vector3 vec3Rotation);

	IBehavior* GetBehavior(int nID);
	list<IBehavior*> GetAllBehaviors();
	list<IBehavior*> GetActivatedBehaviors();
	bool IsBehavioring();
};