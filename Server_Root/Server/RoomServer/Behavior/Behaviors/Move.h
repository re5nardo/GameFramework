#pragma once

#include "../IBehavior.h"
#include "../BehaviorIDs.h"
#include "btBulletCollisionCommon.h"

class BaeGameRoom;

class Move : public IBehavior
{
public:
	Move(IEntity* pEntity);
	virtual ~Move();

public:
	static const unsigned short BEHAVIOR_ID = Move_ID;

private:
	btVector3 m_vec3Dest;
	BaeGameRoom* m_pGameRoom = NULL;

public:
	int GetID() override;
	void Start(__int64 lStartTime, ...) override;
	void Initialize() override;
	void Update(__int64 lUpdateTime) override;
};