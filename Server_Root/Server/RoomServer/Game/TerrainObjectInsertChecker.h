#pragma once

#include "../../CommonSources/QuadTreePrerequisites.h"
#include "btBulletCollisionCommon.h"

using namespace std;

class TerrainObjectInsertChecker
{
public:
	TerrainObjectInsertChecker();
	virtual ~TerrainObjectInsertChecker();

public:
	bool IsValidate(AABB boundary, btCollisionObject* collisionObject);
	bool IsMine(AABB boundary, btCollisionObject* collisionObject);
};