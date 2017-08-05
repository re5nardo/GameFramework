#include "stdafx.h"
#include "CollisionManager.h"
#include "BulletCollision\CollisionShapes\btBox2dShape.h"
#include "BulletCollision/NarrowPhaseCollision/btContinuousConvexCollision.h"
#include "../Util.h"

CollisionManager::CollisionManager()
{
}

CollisionManager::~CollisionManager()
{
	Reset();
}

void CollisionManager::Reset()
{
	for (map<int, btCollisionObject*>::iterator it = m_mapCollisionObject.begin(); it != m_mapCollisionObject.end(); ++it)
	{
		delete it->second->getRootCollisionShape();
		delete it->second;
	}
	m_mapCollisionObject.clear();
	m_listTerrainObject.clear();
	m_listCharacter.clear();

	if (m_pQtTerrainObject != NULL)
	{
		delete m_pQtTerrainObject;
		m_pQtTerrainObject = NULL;
	}

	if (m_pQtCharacter != NULL)
	{
		delete m_pQtCharacter;
		m_pQtCharacter = NULL;
	}

	m_nSequence = 0;
	m_vec3WorldBounds.setValue(0, 0, 0);
}

void CollisionManager::Init(btVector3& vec3WorldBounds)
{
	Reset();

	m_pQtTerrainObject = new QuadTree<btCollisionObject*, TerrainObjectInsertChecker>(AABB(XY(0, 0), vec3WorldBounds.x() * 0.5));
	m_pQtCharacter = new QuadTree<btCollisionObject*, TerrainObjectInsertChecker>(AABB(XY(0, 0), vec3WorldBounds.z() * 0.5));

	m_vec3WorldBounds = vec3WorldBounds;
}

int CollisionManager::AddBox2dShapeTerrainObject(btVector3& vec3Position, btVector3& vec3Rotation, btVector3& vec3HalfExtents)
{
	btCollisionObject* pCollisionObject = new btCollisionObject();

	//	shape
	btBox2dShape* pBox2dShape = new btBox2dShape(vec3HalfExtents);
	pCollisionObject->setCollisionShape(pBox2dShape);

	//	set position
	pCollisionObject->getWorldTransform().setOrigin(vec3Position);

	//	set rotation
	pCollisionObject->getWorldTransform().setRotation(Util::DegreesToQuaternion(vec3Rotation));

	m_mapCollisionObject[m_nSequence] = pCollisionObject;
	m_listTerrainObject.push_back(pCollisionObject);
	m_pQtTerrainObject->Insert(pCollisionObject);

	return m_nSequence++;
}

int CollisionManager::AddSphere2dShapeTerrainObject(btVector3& vec3Position, float fRadius)
{
	btCollisionObject* pCollisionObject = new btCollisionObject();

	//	shape
	btSphereShape* pSphereShape = new btSphereShape(fRadius);
	pCollisionObject->setCollisionShape(pSphereShape);

	//	set position
	pCollisionObject->getWorldTransform().setOrigin(vec3Position);

	//	set rotation
	pCollisionObject->getWorldTransform().setRotation(Util::DegreesToQuaternion(btVector3(0, 0, 0)));

	m_mapCollisionObject[m_nSequence] = pCollisionObject;
	m_listTerrainObject.push_back(pCollisionObject);
	m_pQtTerrainObject->Insert(pCollisionObject);

	return m_nSequence++;
}

int CollisionManager::AddConvexPolygon2dShapeTerrainObject(btVector3& vec3Position, list<btVector3>& listPoint)
{
	btCollisionObject* pCollisionObject = new btCollisionObject();

	//	shape
	btConvexHullShape* pConvexHullShape = new btConvexHullShape();
	for (list<btVector3>::iterator it = listPoint.begin(); it != listPoint.end(); ++it)
	{
		pConvexHullShape->addPoint(*it);
	}
	pCollisionObject->setCollisionShape(pConvexHullShape);

	//	set position
	pCollisionObject->getWorldTransform().setOrigin(vec3Position);

	//	set rotation
	pCollisionObject->getWorldTransform().setRotation(Util::DegreesToQuaternion(btVector3(0, 0, 0)));

	m_mapCollisionObject[m_nSequence] = pCollisionObject;
	m_listTerrainObject.push_back(pCollisionObject);
	m_pQtTerrainObject->Insert(pCollisionObject);

	return m_nSequence++;
}

int CollisionManager::AddCharacter(btVector3& vec3Position, float fRadius)
{
	btCollisionObject* pCollisionObject = new btCollisionObject();

	//	shape
	btSphereShape* pSphereShape = new btSphereShape(fRadius);
	pCollisionObject->setCollisionShape(pSphereShape);

	//	set position
	pCollisionObject->getWorldTransform().setOrigin(vec3Position);

	//	set rotation
	pCollisionObject->getWorldTransform().setRotation(Util::DegreesToQuaternion(btVector3(0, 0, 0)));

	m_mapCollisionObject[m_nSequence] = pCollisionObject;
	m_listCharacter.push_back(pCollisionObject);
	m_pQtCharacter->Insert(pCollisionObject);

	return m_nSequence++;
}

void CollisionManager::SetPosition(int nID, btVector3& vec3Position)
{
	m_mapCollisionObject[nID]->getWorldTransform().setOrigin(vec3Position);
}

void CollisionManager::SetRotation(int nID, btVector3& vec3Rotation)
{
	m_mapCollisionObject[nID]->getWorldTransform().setRotation(Util::DegreesToQuaternion(vec3Rotation));
}

bool CollisionManager::ContinuousCollisionDectection(int nID, btVector3& vec3To, btTransform* trHit)
{
	btConvexCast::CastResult rayResult;
	btConvexPenetrationDepthSolver* penetrationDepthSolver = 0;
	btVoronoiSimplexSolver gGjkSimplexSolver;

	btCollisionObject* pTarget = m_mapCollisionObject[nID];

	for (list<btCollisionObject*>::iterator it = m_listTerrainObject.begin(); it != m_listTerrainObject.end(); ++it)
	{
		btCollisionObject* pTerrainObject = *it;

		btContinuousConvexCollision convexCaster((btConvexShape*)pTarget->getRootCollisionShape(), (btConvexShape*)pTerrainObject->getRootCollisionShape(), &gGjkSimplexSolver, penetrationDepthSolver);
		gGjkSimplexSolver.reset();

		btTransform trTargetFrom = pTarget->getWorldTransform();
		btTransform trTargetTo = pTarget->getWorldTransform();
		trTargetTo.setOrigin(btVector3(vec3To));

		btTransform trTerrainObjectFrom = pTerrainObject->getWorldTransform();
		btTransform trTerrainObjectTo = pTerrainObject->getWorldTransform();

		if (convexCaster.calcTimeOfImpact(trTargetFrom, trTargetTo, trTerrainObjectFrom, trTerrainObjectTo, rayResult))
		{
			btVector3 worldBoundsMin(-m_vec3WorldBounds.x() * 0.5f, -m_vec3WorldBounds.y() * 0.5f, -m_vec3WorldBounds.z() * 0.5f);
			btVector3 worldBoundsMax(m_vec3WorldBounds.x() * 0.5f, m_vec3WorldBounds.y() * 0.5f, m_vec3WorldBounds.z() * 0.5f);
			btTransform hitTrans;

			btVector3 linVel = trTargetTo.getOrigin() - trTargetFrom.getOrigin();
			btTransformUtil::integrateTransform(trTargetFrom, linVel, btVector3(0, 0, 0), rayResult.m_fraction, hitTrans);

			*trHit = hitTrans;

			return true;
		}
	}

	return false;
}

void CollisionManager::RemoveCollisionObject(int nID)
{
	delete m_mapCollisionObject[nID];
	m_mapCollisionObject.erase(nID);
}