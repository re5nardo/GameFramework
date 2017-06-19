#pragma once

#include <list>
#include "../Behavior/IBehavior.h"
#include "../State/IState.h"
#include "../Skill/ISkill.h"
#include "btBulletCollisionCommon.h"

using namespace std;

class IEntity
{
public:
	IEntity(int nID, int nMasterDataID);
	virtual ~IEntity();

protected:
	int m_nID = -1;
	int m_nMasterDataID = -1;

private:
	btVector3 m_vec3Position = btVector3(0, 0, 0);
	btVector3 m_vec3Rotation = btVector3(0, 0, 0);

protected:
	list<IBehavior*> m_listBehavior;
	list<IState*> m_listState;
	list<ISkill*> m_listSkill;

public:
	virtual void Initialize() = 0;
	virtual float GetMoveSpeed() = 0;

public:
	int GetID();
	int GetMasterDataID();

	btVector3 GetPosition();
	void SetPosition(btVector3& vec3Position);

	btVector3 GetRotation();
	void SetRotation(btVector3& vec3Rotation);

public:
	IBehavior* GetBehavior(int nMasterDataID);
	list<IBehavior*> GetAllBehaviors();
	list<IBehavior*> GetActivatedBehaviors();
	bool IsBehavioring();

public:
	IState* GetState(int nID);
	list<IState*> GetStates();
	void AddState(IState* pState);
	void RemoveState(int nID);

public:
	ISkill* GetSkill(int nID);
	list<ISkill*> GetAllSkills();
	list<ISkill*> GetActivatedSkills();
	bool IsSkilling();
};