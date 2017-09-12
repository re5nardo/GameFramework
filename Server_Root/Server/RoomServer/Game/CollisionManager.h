#pragma once

#include "btBulletCollisionCommon.h"
#include "../../CommonSources/QuadTree.h"
#include "TerrainObjectInsertChecker.h"
#include "CollisionObject.h"
#include <map>

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
	map<int, CollisionObject*>		m_mapCollisionObject;
	map<int, CollisionObject*>		m_mapTerrainObject;
	map<int, CollisionObject*>		m_mapCharacter;
	map<int, CollisionObject*>		m_mapProjectile;

private:
	QuadTree<CollisionObject*, TerrainObjectInsertChecker>	m_qtTerrainObject;
	QuadTree<CollisionObject*, TerrainObjectInsertChecker>	m_qtCharacter;
	QuadTree<CollisionObject*, TerrainObjectInsertChecker>	m_qtProjectile;

public:
	void Init(btVector3& vec3WorldBounds);
	void Reset();

public:
	int AddBox2dShapeTerrainObject(btVector3& vec3Position, btVector3& vec3Rotation, btVector3& vec3HalfExtents);
	int AddSphere2dShapeTerrainObject(btVector3& vec3Position, float fRadius);
	int AddConvexPolygon2dShapeTerrainObject(btVector3& vec3Position, list<btVector3>& listPoint);
	int AddCharacter(btVector3& vec3Position, float fSize, float fHeight);
	int AddProjectile(btVector3& vec3Position, float fSize, float fHeight);
	void RemoveCollisionObject(int nID);

public:
	void SetPosition(int nID, btVector3& vec3Position);
	void SetRotation(int nID, btVector3& vec3Rotation);

public:
	bool ContinuousCollisionDectectionFirst(int nID, btVector3& vec3To, int nTypes, pair<int, btVector3>* hit);
	bool ContinuousCollisionDectection(int nID, btVector3& vec3To, int nTypes, list<pair<int, btVector3>>* listHit);
	bool DiscreteCollisionDectection(int nID, int nTypes, list<pair<int, btVector3>>* listHit);

private:
	list<CollisionObject*> GetCollisionObjects(int nTypes, AABB range);
	btConvexShape* GetConvexShape(btCollisionObject* pbtCollisionObject);
};