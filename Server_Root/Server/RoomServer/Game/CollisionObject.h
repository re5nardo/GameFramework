#pragma once

#include "btBulletCollisionCommon.h"
#include "../../CommonSources/QuadTree.h"
#include "CollisionObjectInsertChecker.h"

class CollisionObject
{
public:
	enum Type
	{
		CollisionObjectType_None = 0,
		CollisionObjectType_Terrain = 1,
		CollisionObjectType_Character = 2,
		CollisionObjectType_Projectile = 4,
	};

public:
	CollisionObject(int nID, Type type, btCollisionObject* pbtCollisionObject);
	virtual ~CollisionObject();

public:
	int m_nID;
	Type m_Type = Type::CollisionObjectType_None;
	btCollisionObject* m_pbtCollisionObject = NULL;

public:
	QuadTree<CollisionObject*, CollisionObjectInsertChecker>* m_pQuadTree = NULL;

public:
	AABB GetAABB();
};