#pragma once

#include "../IBehavior.h"
#include "../BehaviorIDs.h"
#include "btBulletCollisionCommon.h"
#include <string>

class BaeGameRoom;

using namespace std;

class Move : public IBehavior
{
public:
	Move(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID);
	virtual ~Move();

public:
	static const unsigned short BEHAVIOR_ID = BehaviorID::MOVE;

private:
	btVector3 m_vec3Dest;

public:
	static const string NAME;

public:
	void Start(__int64 lStartTime, ...) override;
	void Initialize() override;
	void UpdateBody(__int64 lUpdateTime) override;
};