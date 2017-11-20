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
		CollisionObjectType_Character_Challenger = 2,
		CollisionObjectType_Character_Disturber = 4,
		CollisionObjectType_Character = CollisionObjectType_Character_Challenger | CollisionObjectType_Character_Disturber,
		CollisionObjectType_Projectile_Challenger = 8,
		CollisionObjectType_Projectile_Disturber = 16,
		CollisionObjectType_Projectile = CollisionObjectType_Projectile_Challenger | CollisionObjectType_Projectile_Disturber,
		CollisionObjectType_Item = 32,
	};

public:
	CollisionObject(int nID, Type type, btCollisionObject* pbtCollisionObject);
	virtual ~CollisionObject();

private:
	int m_nID;
	Type m_Type = Type::CollisionObjectType_None;
	btCollisionObject* m_pbtCollisionObject = NULL;
	AABB m_AABB;

public:
	QuadTree<CollisionObject*, CollisionObjectInsertChecker>* m_pQuadTree = NULL;

public:
	int GetID();
	CollisionObject::Type GetCollisionObjectType();
	btCollisionObject* GetbtCollisionObject();
	AABB GetAABB();
	void RefreshAABB();
};