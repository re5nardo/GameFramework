#include "stdafx.h"
#include "CollisionManager.h"
#include "BulletCollision/CollisionShapes/btBox2dShape.h"
#include "BulletCollision/NarrowPhaseCollision/btContinuousConvexCollision.h"
#include "../Util.h"
#include <limits>

CollisionManager::CollisionManager()
{
}

CollisionManager::~CollisionManager()
{
	Reset();
}

void CollisionManager::Reset()
{	
	if (m_pCollisionConfiguration != NULL)
	{
		delete m_pCollisionConfiguration;
		m_pCollisionConfiguration = NULL;
	}
	
	if (m_pDispatcher != NULL)
	{
		delete m_pDispatcher;
		m_pDispatcher = NULL;
	}
	
	if (m_pBroadPhase != NULL)
	{
		delete m_pBroadPhase;
		m_pBroadPhase = NULL;
	}
	if (m_pCollisionWorld != NULL)
	{
		delete m_pCollisionWorld;
		m_pCollisionWorld = NULL;
	}

	for (map<int, CollisionObject*>::iterator it = m_mapCollisionObject.begin(); it != m_mapCollisionObject.end(); ++it)
	{
		delete it->second;
	}
	m_mapCollisionObject.clear();
	m_mapTerrainObject.clear();
	m_mapCharacter.clear();
	m_mapProjectile.clear();

	m_qtTerrainObject.Clear();
	m_qtCharacter.Clear();
	m_qtProjectile.Clear();

	m_nSequence = 0;
	m_vec3WorldBounds.setValue(0, 0, 0);
}

void CollisionManager::Init(btVector3& vec3WorldBounds)
{
	Reset();

	m_pCollisionConfiguration = new btDefaultCollisionConfiguration();
	m_pDispatcher = new btCollisionDispatcher(m_pCollisionConfiguration);

	btVector3	worldAabbMin(vec3WorldBounds.x() * -0.5f, 0, vec3WorldBounds.z() * -0.5f);
	btVector3	worldAabbMax(vec3WorldBounds.x() * 0.5f, 0, vec3WorldBounds.z() * 0.5f);
	m_pBroadPhase = new btAxisSweep3(worldAabbMin, worldAabbMax);

	m_pCollisionWorld = new btCollisionWorld(m_pDispatcher, m_pBroadPhase, m_pCollisionConfiguration);

	m_qtTerrainObject.Init(AABB(XY(0, 0), vec3WorldBounds.x() * 0.5f, vec3WorldBounds.z() * 0.5f));
	m_qtCharacter.Init(AABB(XY(0, 0), vec3WorldBounds.x() * 0.5f, vec3WorldBounds.z() * 0.5f));
	m_qtProjectile.Init(AABB(XY(0, 0), vec3WorldBounds.x() * 0.5f, vec3WorldBounds.z() * 0.5f));

	m_vec3WorldBounds = vec3WorldBounds;
}

int CollisionManager::AddBox2dShapeTerrainObject(btVector3& vec3Position, btVector3& vec3Rotation, btVector3& vec3HalfExtents)
{
	btCollisionObject* pbtCollisionObject = new btCollisionObject();

	//	shape
	btBox2dShape* pBox2dShape = new btBox2dShape(vec3HalfExtents);
	pbtCollisionObject->setCollisionShape(pBox2dShape);

	//	set position
	pbtCollisionObject->getWorldTransform().setOrigin(vec3Position);

	//	set rotation
	pbtCollisionObject->getWorldTransform().setRotation(Util::DegreesToQuaternion(vec3Rotation));

	CollisionObject* pCollisionObject = new CollisionObject(m_nSequence, pbtCollisionObject);

	m_mapCollisionObject[m_nSequence] = pCollisionObject;
	m_mapTerrainObject[m_nSequence] = pCollisionObject;

	m_qtTerrainObject.Insert(pCollisionObject);

	return m_nSequence++;
}

int CollisionManager::AddSphere2dShapeTerrainObject(btVector3& vec3Position, float fRadius)
{
	btCollisionObject* pbtCollisionObject = new btCollisionObject();

	//	shape
	btSphereShape* pSphereShape = new btSphereShape(fRadius);
	pbtCollisionObject->setCollisionShape(pSphereShape);

	//	set position
	pbtCollisionObject->getWorldTransform().setOrigin(vec3Position);

	//	set rotation
	pbtCollisionObject->getWorldTransform().setRotation(Util::DegreesToQuaternion(btVector3(0, 0, 0)));

	CollisionObject* pCollisionObject = new CollisionObject(m_nSequence, pbtCollisionObject);

	m_mapCollisionObject[m_nSequence] = pCollisionObject;
	m_mapTerrainObject[m_nSequence] = pCollisionObject;

	m_qtTerrainObject.Insert(pCollisionObject);

	return m_nSequence++;
}

int CollisionManager::AddConvexPolygon2dShapeTerrainObject(btVector3& vec3Position, list<btVector3>& listPoint)
{
	btCollisionObject* pbtCollisionObject = new btCollisionObject();

	//	shape
	btConvexHullShape* pConvexHullShape = new btConvexHullShape();
	for (list<btVector3>::iterator it = listPoint.begin(); it != listPoint.end(); ++it)
	{
		pConvexHullShape->addPoint(*it);
	}
	pbtCollisionObject->setCollisionShape(pConvexHullShape);

	//	set position
	pbtCollisionObject->getWorldTransform().setOrigin(vec3Position);

	//	set rotation
	pbtCollisionObject->getWorldTransform().setRotation(Util::DegreesToQuaternion(btVector3(0, 0, 0)));

	CollisionObject* pCollisionObject = new CollisionObject(m_nSequence, pbtCollisionObject);

	m_mapCollisionObject[m_nSequence] = pCollisionObject;
	m_mapTerrainObject[m_nSequence] = pCollisionObject;

	m_qtTerrainObject.Insert(pCollisionObject);

	return m_nSequence++;
}

int CollisionManager::AddCharacter(btVector3& vec3Position, float fRadius)
{
	btCollisionObject* pbtCollisionObject = new btCollisionObject();

	//	shape
	btSphereShape* pSphereShape = new btSphereShape(fRadius);
	pbtCollisionObject->setCollisionShape(pSphereShape);

	//	set position
	pbtCollisionObject->getWorldTransform().setOrigin(vec3Position);

	//	set rotation
	pbtCollisionObject->getWorldTransform().setRotation(Util::DegreesToQuaternion(btVector3(0, 0, 0)));

	CollisionObject* pCollisionObject = new CollisionObject(m_nSequence, pbtCollisionObject);

	m_mapCollisionObject[m_nSequence] = pCollisionObject;
	m_mapCharacter[m_nSequence] = pCollisionObject;

	m_qtCharacter.Insert(pCollisionObject);

	return m_nSequence++;
}

int CollisionManager::AddProjectile(btVector3& vec3Position, float fRadius)
{
	btCollisionObject* pbtCollisionObject = new btCollisionObject();

	//	shape
	btSphereShape* pSphereShape = new btSphereShape(fRadius);
	pbtCollisionObject->setCollisionShape(pSphereShape);

	//	set position
	pbtCollisionObject->getWorldTransform().setOrigin(vec3Position);

	//	set rotation
	pbtCollisionObject->getWorldTransform().setRotation(Util::DegreesToQuaternion(btVector3(0, 0, 0)));

	CollisionObject* pCollisionObject = new CollisionObject(m_nSequence, pbtCollisionObject);

	m_mapCollisionObject[m_nSequence] = pCollisionObject;
	m_mapProjectile[m_nSequence] = pCollisionObject;

	m_qtProjectile.Insert(pCollisionObject);

	return m_nSequence++;
}

void CollisionManager::SetPosition(int nID, btVector3& vec3Position)
{
	m_mapCollisionObject[nID]->m_pbtCollisionObject->getWorldTransform().setOrigin(vec3Position);
}

void CollisionManager::SetRotation(int nID, btVector3& vec3Rotation)
{
	m_mapCollisionObject[nID]->m_pbtCollisionObject->getWorldTransform().setRotation(Util::DegreesToQuaternion(vec3Rotation));
}

btVector3 vec3Target;
bool CompareDistance(CollisionObject* a, CollisionObject* b)
{
	float distance_a = Util::GetDistance2(vec3Target, a->m_pbtCollisionObject->getWorldTransform().getOrigin());
	float distance_b = Util::GetDistance2(vec3Target, b->m_pbtCollisionObject->getWorldTransform().getOrigin());

	return distance_a < distance_b;
}

//	This shows poor performance where the distance between start to dest is long
bool CollisionManager::ContinuousCollisionDectectionFirst(int nID, btVector3& vec3To, int nTypes, pair<int, btVector3>* hit)
{
	btConvexCast::CastResult rayResult;
	btConvexPenetrationDepthSolver* penetrationDepthSolver = 0;
	btVoronoiSimplexSolver gGjkSimplexSolver;

	btCollisionObject* pTarget = m_mapCollisionObject[nID]->m_pbtCollisionObject;

	btTransform t = pTarget->getWorldTransform();
	btVector3 min, max;
	pTarget->getRootCollisionShape()->getAabb(t, min, max);

	XY center((t.getOrigin().x() + vec3To.x()) * 0.5f, (t.getOrigin().z() + vec3To.z()) * 0.5f);
	float halfDimension_x = (abs(t.getOrigin().x() - vec3To.x()) + (max.x() - min.x())) * 0.5f;
	float halfDimension_z = (abs(t.getOrigin().z() - vec3To.z()) + (max.z() - min.z())) * 0.5f;

	list<CollisionObject*> listCollisionObject = GetCollisionObjects(nTypes, AABB(center, halfDimension_x, halfDimension_z));

	//	Sort distance
	vec3Target = pTarget->getWorldTransform().getOrigin();
	listCollisionObject.sort(CompareDistance);

	for (list<CollisionObject*>::iterator it = listCollisionObject.begin(); it != listCollisionObject.end(); ++it)
	{
		btCollisionObject* pCollisionObject = (*it)->m_pbtCollisionObject;

		btContinuousConvexCollision convexCaster((btConvexShape*)pTarget->getRootCollisionShape(), (btConvexShape*)pCollisionObject->getRootCollisionShape(), &gGjkSimplexSolver, penetrationDepthSolver);
		gGjkSimplexSolver.reset();

		btTransform trTargetFrom = pTarget->getWorldTransform();
		btTransform trTargetTo = pTarget->getWorldTransform();
		trTargetTo.setOrigin(btVector3(vec3To));

		btTransform trCollisionObjectFrom = pCollisionObject->getWorldTransform();
		btTransform trCollisionObjectTo = pCollisionObject->getWorldTransform();

		if (convexCaster.calcTimeOfImpact(trTargetFrom, trTargetTo, trCollisionObjectFrom, trCollisionObjectTo, rayResult))
		{
			btVector3 worldBoundsMin(-m_vec3WorldBounds.x() * 0.5f, -m_vec3WorldBounds.y() * 0.5f, -m_vec3WorldBounds.z() * 0.5f);
			btVector3 worldBoundsMax(m_vec3WorldBounds.x() * 0.5f, m_vec3WorldBounds.y() * 0.5f, m_vec3WorldBounds.z() * 0.5f);
			btTransform hitTrans;

			btVector3 linVel = trTargetTo.getOrigin() - trTargetFrom.getOrigin();
			btTransformUtil::integrateTransform(trTargetFrom, linVel, btVector3(0, 0, 0), rayResult.m_fraction, hitTrans);

			hit->first = (*it)->m_nID;
			hit->second = hitTrans.getOrigin();

			return true;
		}
	}

	return false;
}

//	This shows poor performance where the distance between start to dest is long
bool CollisionManager::ContinuousCollisionDectection(int nID, btVector3& vec3To, int nTypes, list<pair<int, btVector3>>* listHit)
{
	btConvexCast::CastResult rayResult;
	btConvexPenetrationDepthSolver* penetrationDepthSolver = 0;
	btVoronoiSimplexSolver gGjkSimplexSolver;

	btCollisionObject* pTarget = m_mapCollisionObject[nID]->m_pbtCollisionObject;

	btTransform t = pTarget->getWorldTransform();
	btVector3 min, max;
	pTarget->getRootCollisionShape()->getAabb(t, min, max);

	XY center((t.getOrigin().x() + vec3To.x()) * 0.5f, (t.getOrigin().z() + vec3To.z()) * 0.5f);
	float halfDimension_x = (abs(t.getOrigin().x() - vec3To.x()) + (max.x() - min.x())) * 0.5f;
	float halfDimension_z = (abs(t.getOrigin().z() - vec3To.z()) + (max.z() - min.z())) * 0.5f;

	list<CollisionObject*> listCollisionObject = GetCollisionObjects(nTypes, AABB(center, halfDimension_x, halfDimension_z));

	for (list<CollisionObject*>::iterator it = listCollisionObject.begin(); it != listCollisionObject.end(); ++it)
	{
		btCollisionObject* pCollisionObject = (*it)->m_pbtCollisionObject;

		btContinuousConvexCollision convexCaster((btConvexShape*)pTarget->getRootCollisionShape(), (btConvexShape*)pCollisionObject->getRootCollisionShape(), &gGjkSimplexSolver, penetrationDepthSolver);
		gGjkSimplexSolver.reset();

		btTransform trTargetFrom = pTarget->getWorldTransform();
		btTransform trTargetTo = pTarget->getWorldTransform();
		trTargetTo.setOrigin(btVector3(vec3To));

		btTransform trCollisionObjectFrom = pCollisionObject->getWorldTransform();
		btTransform trCollisionObjectTo = pCollisionObject->getWorldTransform();

		if (convexCaster.calcTimeOfImpact(trTargetFrom, trTargetTo, trCollisionObjectFrom, trCollisionObjectTo, rayResult))
		{
			btVector3 worldBoundsMin(-m_vec3WorldBounds.x() * 0.5f, -m_vec3WorldBounds.y() * 0.5f, -m_vec3WorldBounds.z() * 0.5f);
			btVector3 worldBoundsMax(m_vec3WorldBounds.x() * 0.5f, m_vec3WorldBounds.y() * 0.5f, m_vec3WorldBounds.z() * 0.5f);
			btTransform hitTrans;

			btVector3 linVel = trTargetTo.getOrigin() - trTargetFrom.getOrigin();
			btTransformUtil::integrateTransform(trTargetFrom, linVel, btVector3(0, 0, 0), rayResult.m_fraction, hitTrans);

			listHit->push_back(make_pair((*it)->m_nID, btVector3(hitTrans.getOrigin())));
		}
	}
	
	return listHit->size() > 0;
}

bool CollisionManager::DiscreteCollisionDectection(int nID, int nTypes, list<pair<int, btVector3>>* listHit)
{
	btCollisionObject* pTarget = m_mapCollisionObject[nID]->m_pbtCollisionObject;

	btTransform t = pTarget->getWorldTransform();
	btVector3 min, max;
	pTarget->getRootCollisionShape()->getAabb(t, min, max);

	XY center(t.getOrigin().x(), t.getOrigin().z());
	float halfDimension_x = (max.x() - min.x()) * 0.5f;
	float halfDimension_z = (max.z() - min.z()) * 0.5f;

	list<CollisionObject*> listCollisionObject = GetCollisionObjects(nTypes, AABB(center, halfDimension_x, halfDimension_z));

	btCollisionObject* pOther = NULL;
	btCollisionAlgorithm* pCollisionAlgorithm = NULL;
	for (list<CollisionObject*>::iterator it = listCollisionObject.begin(); it != listCollisionObject.end(); ++it)
	{
		if ((*it)->m_nID == nID)
			continue;

		pOther = (*it)->m_pbtCollisionObject;

		pCollisionAlgorithm = m_pCollisionWorld->getDispatcher()->findAlgorithm(pTarget, pOther);
		btManifoldResult contactPointResult(pTarget, pOther);

		pCollisionAlgorithm->processCollision(pTarget, pOther, m_pCollisionWorld->getDispatchInfo(), &contactPointResult);

		btManifoldArray manifoldArray;
		pCollisionAlgorithm->getAllContactManifolds(manifoldArray);	//	All contact possible cases? (NarrowPhase)

		int numManifolds = manifoldArray.size();
		for (int i = 0; i < numManifolds; ++i)
		{
			btPersistentManifold* contactManifold = manifoldArray[i];
			btCollisionObject* body0 = static_cast<btCollisionObject*>(contactManifold->getBody0());

			int numContacts = contactManifold->getNumContacts();
			bool swap = body0 == pTarget;

			for (int j = 0; j < numContacts; ++j)
			{
				btManifoldPoint& pt = contactManifold->getContactPoint(j);

				btVector3 ptA = swap ? pt.getPositionWorldOnA() : pt.getPositionWorldOnB();
				btVector3 ptB = swap ? pt.getPositionWorldOnB() : pt.getPositionWorldOnA();

				listHit->push_back(make_pair((*it)->m_nID, btVector3(ptA)));

				break;		//	Take just one
			}

			contactManifold->clearManifold();
		}
	}

	return false;
}

void CollisionManager::RemoveCollisionObject(int nID)
{
	if (m_mapTerrainObject.count(nID) > 0)
	{
		m_qtTerrainObject.Remove(m_mapTerrainObject[nID]);
		m_mapTerrainObject.erase(nID);
	}
	else if (m_mapCharacter.count(nID) > 0)
	{
		m_qtCharacter.Remove(m_mapCharacter[nID]);
		m_mapCharacter.erase(nID);
	}
	else if (m_mapProjectile.count(nID) > 0)
	{
		m_qtProjectile.Remove(m_mapProjectile[nID]);
		m_mapProjectile.erase(nID);
	}

	delete m_mapCollisionObject[nID];
	m_mapCollisionObject.erase(nID);
}

list<CollisionObject*> CollisionManager::GetCollisionObjects(int nTypes, AABB range)
{
	list<CollisionObject*> listCollisionObject;

	if ((nTypes & CollisionObject::Type::CollisionObjectType_Terrain) == CollisionObject::Type::CollisionObjectType_Terrain)
	{
		list<CollisionObject*> listQueried = m_qtTerrainObject.QueryRange(range);

		listCollisionObject.insert(listCollisionObject.end(), listQueried.begin(), listQueried.end());
	}

	if ((nTypes & CollisionObject::Type::CollisionObjectType_Character) == CollisionObject::Type::CollisionObjectType_Character)
	{
		list<CollisionObject*> listQueried = m_qtCharacter.QueryRange(range);

		listCollisionObject.insert(listCollisionObject.end(), listQueried.begin(), listQueried.end());
	}

	if ((nTypes & CollisionObject::Type::CollisionObjectType_Projectile) == CollisionObject::Type::CollisionObjectType_Projectile)
	{
		list<CollisionObject*> listQueried = m_qtProjectile.QueryRange(range);

		listCollisionObject.insert(listCollisionObject.end(), listQueried.begin(), listQueried.end());
	}

	return listCollisionObject;
}