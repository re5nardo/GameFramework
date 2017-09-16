#pragma once

#include "../../CommonSources/QuadTreePrerequisites.h"

class CollisionObject;

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