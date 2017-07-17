#pragma once

#include "../IBehavior.h"
#include "../BehaviorIDs.h"
#include <string>

using namespace std;

class Idle : public IBehavior
{
public:
	Idle(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID);
	virtual ~Idle();

public:
	static const unsigned short BEHAVIOR_ID = BehaviorID::IDLE;

public:
	static const string NAME;

public:
	void Initialize() override;
	void Update(__int64 lUpdateTime) override;
};