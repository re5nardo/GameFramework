#pragma once

#include <list>
#include "../Behavior/IBehavior.h"
#include "../State/IState.h"
#include "../Skill/ISkill.h"
#include "btBulletCollisionCommon.h"
#include "../../FBSFiles/FBSData_generated.h"
#include "../State/CoreState.h"

class BaeGameRoom;
class IGameEvent;

using namespace std;

class IEntity
{
public:
	IEntity(BaeGameRoom* pGameRoom, int nID, int nMasterDataID);
	virtual ~IEntity();

protected:
	BaeGameRoom* m_pGameRoom;

protected:
	int m_nID = -1;
	int m_nMasterDataID = -1;

protected:
	float m_fSize;
	float m_fHeight;
	float m_fDefault_Y;
	int m_nDefaultBehaviorID;

protected:
	btVector3 m_vec3Position = btVector3(0, 0, 0);
	btVector3 m_vec3Rotation = btVector3(0, 0, 0);

protected:
	list<IBehavior*> m_listBehavior;
	list<IState*> m_listState;

protected:
	list<IState*> m_listDestroyReserved;

public:
	bool m_bDestroyReserved = false;

public:
	virtual void Initialize() = 0;
	virtual float GetSpeed() = 0;
	virtual float GetMaximumSpeed() = 0;
	virtual FBS::Data::EntityType GetEntityType() = 0;

public:
	void UpdateBehaviors(long long lUpdateTime);
	void UpdateStates(long long lUpdateTime);
	void virtual LateUpdate(long long lUpdateTime);

public:
	int GetID();
	int GetMasterDataID();

	btVector3 GetPosition();
	void SetPosition(btVector3& vec3Position);

	btVector3 GetRotation();
	void SetRotation(btVector3& vec3Rotation);

	float GetSize();
	float GetHeight();
	float GetDefault_Y();

public:
	IBehavior* GetBehavior(int nMasterDataID);
	list<IBehavior*> GetAllBehaviors();
	list<IBehavior*> GetActivatedBehaviors();
	bool IsBehavioring();

public:
	IState* GetState(int nID);
	list<IState*> GetStates();
	void AddState(IState* pState, long long lTime);
	void RemoveState(int nMasterDataID, long long lTime);
	bool HasCoreState(CoreState coreState);

public:
	virtual void NotifyGameEvent(IGameEvent* pGameEvent) = 0;

public:
	virtual bool IsTerrainPassable() = 0;
	virtual int GetMoveCollisionTypes() = 0;

public:
	virtual void OnCollision(IEntity* pOther, long long lTime);

protected:
	void TrimState();
};