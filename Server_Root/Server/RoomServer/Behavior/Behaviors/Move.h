#pragma once

#include "../IBehavior.h"
#include "../BehaviorIDs.h"
#include "../../Data.h"

class Move : public IBehavior
{
public:
	Move(IEntity* pEntity);
	virtual ~Move();

public:
	static const unsigned short BEHAVIOR_ID = Move_ID;

private:
	Vector3 m_vec3Dest;

public:
	int GetID() override;
	void Start(__int64 lStartTime, ...) override;
	void Initialize() override;
	void Update(__int64 lUpdateTime) override;
};