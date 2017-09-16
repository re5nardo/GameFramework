#pragma once

#include "btBulletCollisionCommon.h"
#include "../../CommonSources/QuadTree.h"
#include "TerrainObjectInsertChecker.h"

class CollisionObject
{
public:
	CollisionObject(int nID, btCollisionObject* pbtCollisionObject);
	virtual ~CollisionObject();

public:
	enum Type
	{
		CollisionObjectType_None = 0,
		CollisionObjectType_Terrain = 1,
		CollisionObjectType_Character = 2,
		CollisionObjectType_Projectile = 4,
	};

public:
	int m_nID;
	btCollisionObject* m_pbtCollisionObject = NULL;

public:
	QuadTree<CollisionObject*, TerrainObjectInsertChecker>* m_pQuadTree = NULL;

public:
	AABB GetAABB();
};