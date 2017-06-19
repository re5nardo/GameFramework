#pragma once

#include "../IState.h"
#include "../StateIDs.h"

class Acceleration : public IState
{
public:
	Acceleration(IEntity* pEntity, __int64 lStartTime);
	virtual ~Acceleration();

public:
	static const unsigned short STATE_ID = StateID::Acceleration;

public:
	int GetID() override;
	void Update(__int64 lUpdateTime) override;
};