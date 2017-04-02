#pragma once

#include "../IBehavior.h"
#include "../BehaviorIDs.h"
#include "../../../CommonSources/MathematicalData.h"

class MapManager;

using namespace MathematicalData;

class Move : public IBehavior
{
public:
	Move(IEntity* pEntity);
	virtual ~Move();

public:
	static const unsigned short BEHAVIOR_ID = Move_ID;

private:
	Vector3 m_vec3Dest;
	MapManager* m_pMapManager = NULL;

public:
	int GetID() override;
	void Start(__int64 lStartTime, ...) override;
	void Initialize() override;
	void Update(__int64 lUpdateTime) override;
};