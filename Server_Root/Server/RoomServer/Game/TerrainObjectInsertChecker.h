#pragma once

#include "../../CommonSources/QuadTreePrerequisites.h"
#include "CollisionObject.h"

using namespace std;

class TerrainObjectInsertChecker
{
public:
	TerrainObjectInsertChecker();
	virtual ~TerrainObjectInsertChecker();

public:
	bool IsValidate(AABB boundary, CollisionObject* pCollisionObject);
	bool IsMine(AABB boundary, CollisionObject* pCollisionObject);
};