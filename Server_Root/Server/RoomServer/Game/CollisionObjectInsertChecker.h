#pragma once

#include "../../CommonSources/QuadTreePrerequisites.h"

class CollisionObject;

using namespace std;

class CollisionObjectInsertChecker
{
public:
	CollisionObjectInsertChecker();
	virtual ~CollisionObjectInsertChecker();

public:
	bool IsValidate(AABB boundary, CollisionObject* pCollisionObject);
	bool IsMine(AABB boundary, CollisionObject* pCollisionObject);
};