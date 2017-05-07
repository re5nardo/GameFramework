#pragma once

#include "btBulletCollisionCommon.h"
#include "../../CommonSources/QuadTree.h"
#include "TerrainObjectInsertChecker.h"
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
	map<int, btCollisionObject*>		m_mapCollisionObject;
	list<btCollisionObject*>			m_listTerrainObject;
	list<btCollisionObject*>			m_listCharacter;
	QuadTree<btCollisionObject*, TerrainObjectInsertChecker>*	m_pQtTerrainObject;
	QuadTree<btCollisionObject*, TerrainObjectInsertChecker>*	m_pQtCharacter;

public:
	void Init(btVector3& vec3WorldBounds);

public:
	int AddBox2dShapeTerrainObject(btVector3& vec3Position, btVector3& vec3Rotation, btVector3& vec3HalfExtents);
	int AddSphere2dShapeTerrainObject(btVector3& vec3Position, float fRadius);
	int AddConvexPolygon2dShapeTerrainObject(btVector3& vec3Position, list<btVector3>& listPoint);
	int AddCharacter(btVector3& vec3Position, float fRadius);

public:
	void SetPosition(int nID, btVector3& vec3Position);
	void SetRotation(int nID, btVector3& vec3Rotation);

public:
	bool ContinuousCollisionDectection(int nID, btVector3& vec3To, btTransform* trHit);
	void Reset();
};