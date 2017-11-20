#pragma once

#include "btBulletCollisionCommon.h"
#include "../../CommonSources/QuadTree.h"
#include "CollisionObjectInsertChecker.h"
#include "CollisionObject.h"
#include <map>
#include <list>

class Character;
class Projectile;

using namespace std;

class CollisionManager
{
public:
	CollisionManager();
	virtual ~CollisionManager();

private:
	int m_nSequence = 0;
	btVector3 m_vec3WorldBounds;

private:
	btDefaultCollisionConfiguration* m_pCollisionConfiguration = NULL;
	btCollisionDispatcher* m_pDispatcher = NULL;
	btAxisSweep3* m_pBroadPhase = NULL;
	btCollisionWorld* m_pCollisionWorld = NULL;

private:
	map<int, CollisionObject*>								m_mapCollisionObject;		//	All Collision Objects
	map<CollisionObject::Type, map<int, CollisionObject*>>	m_mapTypeCollisionObject;	//	Typed Collision Objects

private:
	map<CollisionObject::Type, QuadTree<CollisionObject*, CollisionObjectInsertChecker>> m_mapQuadTree;

public:
	void Init(btVector3& vec3WorldBounds);
	void Reset();

public:
	int AddBox2dShapeTerrainObject(btVector3& vec3Position, btVector3& vec3Rotation, btVector3& vec3HalfExtents);
	int AddSphere2dShapeTerrainObject(btVector3& vec3Position, float fRadius);
	int AddConvexPolygon2dShapeTerrainObject(btVector3& vec3Position, list<btVector3>& listPoint);
	int AddCharacter(btVector3& vec3Position, float fSize, float fHeight, Character* pCharacter);
	int AddProjectile(btVector3& vec3Position, float fSize, float fHeight, Projectile* pProjectile);
	int AddItem(btVector3& vec3Position, float fSize, float fHeight);
	void RemoveCollisionObject(int nID);

public:
	void SetPosition(int nID, btVector3& vec3Position);
	void SetRotation(int nID, btVector3& vec3Rotation);

public:
	bool ContinuousCollisionDectection(int nTargetID, int nOtherID, btVector3& vec3To, btVector3& vec3Hit);
	bool DiscreteCollisionDectection(int nTargetID, int nOtherID, btVector3& vec3Hit);
	bool GetCollisionObjectsInRange(int nTargetID, btVector3& vec3To, int nTypes, list<CollisionObject*>* pObjects);
	bool ContinuousCollisionDectectionFirst(int nID, btVector3& vec3To, int nTypes, pair<int, btVector3>* hit);
	bool ContinuousCollisionDectection(int nID, btVector3& vec3To, int nTypes, list<pair<int, btVector3>>* listHit);
	bool DiscreteCollisionDectection(int nID, int nTypes, list<pair<int, btVector3>>* listHit);
	bool CehckExistInRange(btVector3& vec3Center, float fRadius, int nTypes, list<pair<int, btVector3>>* listItem);	//	Ignore Y-axis
	bool CehckExistInRange(int nID, float fRadius, int nTypes, list<pair<int, btVector3>>* listItem);	//	Ignore Y-axis

private:
	list<CollisionObject*> GetCollisionObjects(int nTypes, AABB range);
	btConvexShape* GetConvexShape(btCollisionObject* pbtCollisionObject);

public:
	CollisionObject* GetCollisionObject(int nID);
};