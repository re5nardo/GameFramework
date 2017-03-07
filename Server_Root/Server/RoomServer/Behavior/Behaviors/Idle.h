#pragma once

#include "../IBehavior.h"
#include "../BehaviorIDs.h"

class Idle : public IBehavior
{
public:
	Idle(IEntity* pEntity);
	virtual ~Idle();

public:
	static const unsigned short BEHAVIOR_ID = Idle_ID;

public:
	int GetID() override;
	void Initialize() override;
	void Update(__int64 lUpdateTime) override;
};