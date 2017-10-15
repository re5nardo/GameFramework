#include "stdafx.h"
#include "CollisionManager.h"
#include "BulletCollision/CollisionShapes/btBox2dShape.h"
#include "BulletCollision/CollisionShapes/btCapsuleShape.h"
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
	m_mapTypeCollisionObject.clear();

	m_mapQuadTree.clear();

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

	CollisionObject* pCollisionObject = new CollisionObject(m_nSequence, CollisionObject::Type::CollisionObjectType_Terrain, pbtCollisionObject);

	m_mapCollisionObject[m_nSequence] = pCollisionObject;
	m_mapTypeCollisionObject[CollisionObject::Type::CollisionObjectType_Terrain][m_nSequence] = pCollisionObject;

	if (m_mapQuadTree.count(CollisionObject::Type::CollisionObjectType_Terrain) == 0)
	{
		m_mapQuadTree[CollisionObject::Type::CollisionObjectType_Terrain].SetBoundary(AABB(XY(0, 0), m_vec3WorldBounds.x() * 0.5f, m_vec3WorldBounds.z() * 0.5f));
	}
	m_mapQuadTree[CollisionObject::Type::CollisionObjectType_Terrain].Insert(pCollisionObject);

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

	CollisionObject* pCollisionObject = new CollisionObject(m_nSequence, CollisionObject::Type::CollisionObjectType_Terrain, pbtCollisionObject);

	m_mapCollisionObject[m_nSequence] = pCollisionObject;
	m_mapTypeCollisionObject[CollisionObject::Type::CollisionObjectType_Terrain][m_nSequence] = pCollisionObject;

	if (m_mapQuadTree.count(CollisionObject::Type::CollisionObjectType_Terrain) == 0)
	{
		m_mapQuadTree[CollisionObject::Type::CollisionObjectType_Terrain].SetBoundary(AABB(XY(0, 0), m_vec3WorldBounds.x() * 0.5f, m_vec3WorldBounds.z() * 0.5f));
	}
	m_mapQuadTree[CollisionObject::Type::CollisionObjectType_Terrain].Insert(pCollisionObject);

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

	CollisionObject* pCollisionObject = new CollisionObject(m_nSequence, CollisionObject::Type::CollisionObjectType_Terrain, pbtCollisionObject);

	m_mapCollisionObject[m_nSequence] = pCollisionObject;
	m_mapTypeCollisionObject[CollisionObject::Type::CollisionObjectType_Terrain][m_nSequence] = pCollisionObject;

	if (m_mapQuadTree.count(CollisionObject::Type::CollisionObjectType_Terrain) == 0)
	{
		m_mapQuadTree[CollisionObject::Type::CollisionObjectType_Terrain].SetBoundary(AABB(XY(0, 0), m_vec3WorldBounds.x() * 0.5f, m_vec3WorldBounds.z() * 0.5f));
	}
	m_mapQuadTree[CollisionObject::Type::CollisionObjectType_Terrain].Insert(pCollisionObject);

	return m_nSequence++;
}

int CollisionManager::AddCharacter(btVector3& vec3Position, float fSize, float fHeight, Character* pCharacter)
{
	btCollisionObject* pbtCollisionObject = new btCollisionObject();
	pbtCollisionObject->setUserPointer(pCharacter);

	//	shape
	btCompoundShape* pCompoundShape = new btCompoundShape();
	btTransform trLocal;
	trLocal.setIdentity();
	trLocal.setOrigin(btVector3(0, fHeight * 0.5f, 0));
	pCompoundShape->addChildShape(trLocal, new btCylinderShape(btVector3(fSize * 0.5f, fHeight * 0.5f, fSize * 0.5f)));
	pbtCollisionObject->setCollisionShape(pCompoundShape);

	//	set position
	pbtCollisionObject->getWorldTransform().setOrigin(vec3Position);

	//	set rotation
	pbtCollisionObject->getWorldTransform().setRotation(Util::DegreesToQuaternion(btVector3(0, 0, 0)));

	CollisionObject* pCollisionObject = new CollisionObject(m_nSequence, CollisionObject::Type::CollisionObjectType_Character, pbtCollisionObject);

	m_mapCollisionObject[m_nSequence] = pCollisionObject;
	m_mapTypeCollisionObject[CollisionObject::Type::CollisionObjectType_Character][m_nSequence] = pCollisionObject;

	if (m_mapQuadTree.count(CollisionObject::Type::CollisionObjectType_Character) == 0)
	{
		m_mapQuadTree[CollisionObject::Type::CollisionObjectType_Character].SetBoundary(AABB(XY(0, 0), m_vec3WorldBounds.x() * 0.5f, m_vec3WorldBounds.z() * 0.5f));
	}
	m_mapQuadTree[CollisionObject::Type::CollisionObjectType_Character].Insert(pCollisionObject);

	return m_nSequence++;
}

int CollisionManager::AddProjectile(btVector3& vec3Position, float fSize, float fHeight, Projectile* pProjectile)
{
	btCollisionObject* pbtCollisionObject = new btCollisionObject();
	pbtCollisionObject->setUserPointer(pProjectile);

	//	shape
	btCylinderShape* pCapsuleShape = new btCylinderShape(btVector3(fSize * 0.5f, fHeight * 0.5f, fSize * 0.5f));
	pbtCollisionObject->setCollisionShape(pCapsuleShape);

	//	set position
	pbtCollisionObject->getWorldTransform().setOrigin(vec3Position);

	//	set rotation
	pbtCollisionObject->getWorldTransform().setRotation(Util::DegreesToQuaternion(btVector3(0, 0, 0)));

	CollisionObject* pCollisionObject = new CollisionObject(m_nSequence, CollisionObject::Type::CollisionObjectType_Projectile, pbtCollisionObject);

	m_mapCollisionObject[m_nSequence] = pCollisionObject;
	m_mapTypeCollisionObject[CollisionObject::Type::CollisionObjectType_Projectile][m_nSequence] = pCollisionObject;

	if (m_mapQuadTree.count(CollisionObject::Type::CollisionObjectType_Projectile) == 0)
	{
		m_mapQuadTree[CollisionObject::Type::CollisionObjectType_Projectile].SetBoundary(AABB(XY(0, 0), m_vec3WorldBounds.x() * 0.5f, m_vec3WorldBounds.z() * 0.5f));
	}
	m_mapQuadTree[CollisionObject::Type::CollisionObjectType_Projectile].Insert(pCollisionObject);

	return m_nSequence++;
}

void CollisionManager::SetPosition(int nID, btVector3& vec3Position)
{
	CollisionObject* pCollisionObject = m_mapCollisionObject[nID];

	pCollisionObject->m_pbtCollisionObject->getWorldTransform().setOrigin(vec3Position);

	pCollisionObject->m_pQuadTree->Transform(pCollisionObject);
}

void CollisionManager::SetRotation(int nID, btVector3& vec3Rotation)
{
	CollisionObject* pCollisionObject = m_mapCollisionObject[nID];

	pCollisionObject->m_pbtCollisionObject->getWorldTransform().setRotation(Util::DegreesToQuaternion(vec3Rotation));

	pCollisionObject->m_pQuadTree->Transform(pCollisionObject);
}

bool CollisionManager::ContinuousCollisionDectection(int nTargetID, int nOtherID, btVector3& vec3To, btVector3& vec3Hit)
{
	btConvexCast::CastResult rayResult;
	btConvexPenetrationDepthSolver* penetrationDepthSolver = 0;
	btVoronoiSimplexSolver gGjkSimplexSolver;

	btCollisionObject* pTarget = m_mapCollisionObject[nTargetID]->m_pbtCollisionObject;
	btCollisionObject* pOther = m_mapCollisionObject[nOtherID]->m_pbtCollisionObject;

	btTransform trTargetOffset;
	trTargetOffset.setIdentity();
	if (pTarget->getRootCollisionShape()->getShapeType() == COMPOUND_SHAPE_PROXYTYPE)
	{
		btCompoundShape* pCompoundShape = (btCompoundShape*)pTarget->getRootCollisionShape();
		trTargetOffset = pCompoundShape->getChildTransform(0);
	}

	btTransform trOtherOffset;
	trOtherOffset.setIdentity();
	if (pOther->getRootCollisionShape()->getShapeType() == COMPOUND_SHAPE_PROXYTYPE)
	{
		btCompoundShape* pCompoundShape = (btCompoundShape*)pOther->getRootCollisionShape();
		trOtherOffset = pCompoundShape->getChildTransform(0);
	}

	btContinuousConvexCollision convexCaster(GetConvexShape(pTarget), GetConvexShape(pOther), &gGjkSimplexSolver, penetrationDepthSolver);

	btTransform trTargetFrom = pTarget->getWorldTransform();
	btTransform trTargetTo = pTarget->getWorldTransform();
	trTargetTo.setOrigin(vec3To);

	trTargetFrom *= trTargetOffset;
	trTargetTo *= trTargetOffset;

	btTransform trOtherFrom = pOther->getWorldTransform();
	btTransform trOtherTo = pOther->getWorldTransform();

	trOtherFrom *= trOtherOffset;
	trOtherTo *= trOtherOffset;

	if (convexCaster.calcTimeOfImpact(trTargetFrom, trTargetTo, trOtherFrom, trOtherTo, rayResult))
	{
		btVector3 worldBoundsMin(-m_vec3WorldBounds.x() * 0.5f, -m_vec3WorldBounds.y() * 0.5f, -m_vec3WorldBounds.z() * 0.5f);
		btVector3 worldBoundsMax(m_vec3WorldBounds.x() * 0.5f, m_vec3WorldBounds.y() * 0.5f, m_vec3WorldBounds.z() * 0.5f);
		btTransform hitTrans;

		btVector3 linVel, angVel;
		btTransformUtil::calculateVelocity(trTargetFrom, trTargetTo, 1.0, linVel, angVel);
		btTransformUtil::integrateTransform(trTargetFrom, linVel, angVel, rayResult.m_fraction, hitTrans);

		hitTrans *= trTargetOffset.inverse();

		vec3Hit = hitTrans.getOrigin();

		return true;
	}

	return false;
}

bool CollisionManager::DiscreteCollisionDectection(int nTargetID, int nOtherID, btVector3& vec3Hit)
{
	btCollisionObject* pTarget = m_mapCollisionObject[nTargetID]->m_pbtCollisionObject;
	btCollisionObject* pOther = m_mapCollisionObject[nOtherID]->m_pbtCollisionObject;

	btCollisionAlgorithm* pCollisionAlgorithm = m_pCollisionWorld->getDispatcher()->findAlgorithm(pTarget, pOther);
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

			vec3Hit = ptA;

			return true;	//	Take just one
		}
	}

	return false;
}

btVector3 vec3Target;
bool CollisionManager::GetCollisionObjectsInRange(int nTargetID, btVector3& vec3To, int nTypes, list<CollisionObject*>* pObjects)
{
	btCollisionObject* pTarget = m_mapCollisionObject[nTargetID]->m_pbtCollisionObject;

	btTransform trOffset;
	trOffset.setIdentity();
	if (pTarget->getRootCollisionShape()->getShapeType() == COMPOUND_SHAPE_PROXYTYPE)
	{
		btCompoundShape* pCompoundShape = (btCompoundShape*)pTarget->getRootCollisionShape();
		trOffset = pCompoundShape->getChildTransform(0);
	}

	btTransform t = pTarget->getWorldTransform() * trOffset;
	btVector3 min, max;
	GetConvexShape(pTarget)->getAabb(t, min, max);

	XY center((t.getOrigin().x() + vec3To.x()) * 0.5f, (t.getOrigin().z() + vec3To.z()) * 0.5f);
	float halfDimension_x = (abs(t.getOrigin().x() - vec3To.x()) + (max.x() - min.x())) * 0.5f;
	float halfDimension_z = (abs(t.getOrigin().z() - vec3To.z()) + (max.z() - min.z())) * 0.5f;

	*pObjects = GetCollisionObjects(nTypes, AABB(center, halfDimension_x, halfDimension_z));

	CollisionObject* remove = NULL;
	for (list<CollisionObject*>::iterator it = pObjects->begin(); it != pObjects->end(); ++it)
	{
		if ((*it)->m_nID == nTargetID)
		{
			remove = *it;
		}
	}
	pObjects->remove(remove);

	//	Sort distance
	vec3Target = pTarget->getWorldTransform().getOrigin();
	auto cmp = [](CollisionObject* a, CollisionObject* b)
	{
		float distance_a = Util::GetDistance2(vec3Target, a->m_pbtCollisionObject->getWorldTransform().getOrigin());
		float distance_b = Util::GetDistance2(vec3Target, b->m_pbtCollisionObject->getWorldTransform().getOrigin());

		return distance_a < distance_b;
	};
	pObjects->sort(cmp);

	return pObjects->size() > 0;
}

//	This shows poor performance where the distance between start to dest is long
bool CollisionManager::ContinuousCollisionDectectionFirst(int nID, btVector3& vec3To, int nTypes, pair<int, btVector3>* hit)
{
	btConvexCast::CastResult rayResult;
	btConvexPenetrationDepthSolver* penetrationDepthSolver = 0;
	btVoronoiSimplexSolver gGjkSimplexSolver;

	btCollisionObject* pTarget = m_mapCollisionObject[nID]->m_pbtCollisionObject;

	btTransform trOffset;
	trOffset.setIdentity();
	if (pTarget->getRootCollisionShape()->getShapeType() == COMPOUND_SHAPE_PROXYTYPE)
	{
		btCompoundShape* pCompoundShape = (btCompoundShape*)pTarget->getRootCollisionShape();
		trOffset = pCompoundShape->getChildTransform(0);
	}

	btTransform t = pTarget->getWorldTransform() * trOffset;
	btVector3 min, max;
	GetConvexShape(pTarget)->getAabb(t, min, max);

	XY center((t.getOrigin().x() + vec3To.x()) * 0.5f, (t.getOrigin().z() + vec3To.z()) * 0.5f);
	float halfDimension_x = (abs(t.getOrigin().x() - vec3To.x()) + (max.x() - min.x())) * 0.5f;
	float halfDimension_z = (abs(t.getOrigin().z() - vec3To.z()) + (max.z() - min.z())) * 0.5f;

	list<CollisionObject*> listCollisionObject = GetCollisionObjects(nTypes, AABB(center, halfDimension_x, halfDimension_z));

	//	Sort distance
	vec3Target = pTarget->getWorldTransform().getOrigin();
	auto cmp = [](CollisionObject* a, CollisionObject* b)
	{
		float distance_a = Util::GetDistance2(vec3Target, a->m_pbtCollisionObject->getWorldTransform().getOrigin());
		float distance_b = Util::GetDistance2(vec3Target, b->m_pbtCollisionObject->getWorldTransform().getOrigin());

		return distance_a < distance_b;
	};
	listCollisionObject.sort(cmp);

	for (list<CollisionObject*>::iterator it = listCollisionObject.begin(); it != listCollisionObject.end(); ++it)
	{
		if ((*it)->m_nID == nID)
			continue;

		btCollisionObject* pCollisionObject = (*it)->m_pbtCollisionObject;

		btContinuousConvexCollision convexCaster(GetConvexShape(pTarget), GetConvexShape(pCollisionObject), &gGjkSimplexSolver, penetrationDepthSolver);
		gGjkSimplexSolver.reset();

		btTransform trTargetFrom = pTarget->getWorldTransform();
		btTransform trTargetTo = pTarget->getWorldTransform();
		trTargetTo.setOrigin(vec3To);

		trTargetFrom *= trOffset;
		trTargetTo *= trOffset;

		btTransform trCollisionObjectFrom = pCollisionObject->getWorldTransform();
		btTransform trCollisionObjectTo = pCollisionObject->getWorldTransform();

		if (convexCaster.calcTimeOfImpact(trTargetFrom, trTargetTo, trCollisionObjectFrom, trCollisionObjectTo, rayResult))
		{
			btVector3 worldBoundsMin(-m_vec3WorldBounds.x() * 0.5f, -m_vec3WorldBounds.y() * 0.5f, -m_vec3WorldBounds.z() * 0.5f);
			btVector3 worldBoundsMax(m_vec3WorldBounds.x() * 0.5f, m_vec3WorldBounds.y() * 0.5f, m_vec3WorldBounds.z() * 0.5f);
			btTransform hitTrans;

			btVector3 linVel, angVel;
			btTransformUtil::calculateVelocity(trTargetFrom, trTargetTo, 1.0, linVel, angVel);
			btTransformUtil::integrateTransform(trTargetFrom, linVel, angVel, rayResult.m_fraction, hitTrans);

			hitTrans *= trOffset.inverse();

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

	btTransform trOffset;
	trOffset.setIdentity();
	if (pTarget->getRootCollisionShape()->getShapeType() == COMPOUND_SHAPE_PROXYTYPE)
	{
		btCompoundShape* pCompoundShape = (btCompoundShape*)pTarget->getRootCollisionShape();
		trOffset = pCompoundShape->getChildTransform(0);
	}

	btTransform t = pTarget->getWorldTransform() * trOffset;
	btVector3 min, max;
	GetConvexShape(pTarget)->getAabb(t, min, max);

	XY center((t.getOrigin().x() + vec3To.x()) * 0.5f, (t.getOrigin().z() + vec3To.z()) * 0.5f);
	float halfDimension_x = (abs(t.getOrigin().x() - vec3To.x()) + (max.x() - min.x())) * 0.5f;
	float halfDimension_z = (abs(t.getOrigin().z() - vec3To.z()) + (max.z() - min.z())) * 0.5f;

	list<CollisionObject*> listCollisionObject = GetCollisionObjects(nTypes, AABB(center, halfDimension_x, halfDimension_z));

	for (list<CollisionObject*>::iterator it = listCollisionObject.begin(); it != listCollisionObject.end(); ++it)
	{
		if ((*it)->m_nID == nID)
			continue;

		btCollisionObject* pCollisionObject = (*it)->m_pbtCollisionObject;

		btContinuousConvexCollision convexCaster(GetConvexShape(pTarget), GetConvexShape(pCollisionObject), &gGjkSimplexSolver, penetrationDepthSolver);
		gGjkSimplexSolver.reset();

		btTransform trTargetFrom = pTarget->getWorldTransform();
		btTransform trTargetTo = pTarget->getWorldTransform();
		trTargetTo.setOrigin(vec3To);

		trTargetFrom *= trOffset;
		trTargetTo *= trOffset;

		btTransform trCollisionObjectFrom = pCollisionObject->getWorldTransform();
		btTransform trCollisionObjectTo = pCollisionObject->getWorldTransform();

		if (convexCaster.calcTimeOfImpact(trTargetFrom, trTargetTo, trCollisionObjectFrom, trCollisionObjectTo, rayResult))
		{
			btVector3 worldBoundsMin(-m_vec3WorldBounds.x() * 0.5f, -m_vec3WorldBounds.y() * 0.5f, -m_vec3WorldBounds.z() * 0.5f);
			btVector3 worldBoundsMax(m_vec3WorldBounds.x() * 0.5f, m_vec3WorldBounds.y() * 0.5f, m_vec3WorldBounds.z() * 0.5f);
			btTransform hitTrans;

			btVector3 linVel, angVel;
			btTransformUtil::calculateVelocity(trTargetFrom, trTargetTo, 1.0, linVel, angVel);
			btTransformUtil::integrateTransform(trTargetFrom, linVel, angVel, rayResult.m_fraction, hitTrans);

			hitTrans *= trOffset.inverse();

			listHit->push_back(make_pair((*it)->m_nID, btVector3(hitTrans.getOrigin())));
		}
	}
	
	return listHit->size() > 0;
}

bool CollisionManager::DiscreteCollisionDectection(int nID, int nTypes, list<pair<int, btVector3>>* listHit)
{
	btCollisionObject* pTarget = m_mapCollisionObject[nID]->m_pbtCollisionObject;

	btTransform trOffset;
	trOffset.setIdentity();
	if (pTarget->getRootCollisionShape()->getShapeType() == COMPOUND_SHAPE_PROXYTYPE)
	{
		btCompoundShape* pCompoundShape = (btCompoundShape*)pTarget->getRootCollisionShape();
		trOffset = pCompoundShape->getChildTransform(0);
	}

	btTransform t = pTarget->getWorldTransform() * trOffset;
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

bool CollisionManager::CehckExistInRange(btVector3& vec3Center, float fRadius, int nTypes, list<pair<int, btVector3>>* listItem)
{
	listItem->clear();

	list<CollisionObject*> listCollisionObject = GetCollisionObjects(nTypes, AABB(XY(vec3Center.x(), vec3Center.z()), fRadius, fRadius));

	btCollisionObject* pOther = NULL;
	for (list<CollisionObject*>::iterator it = listCollisionObject.begin(); it != listCollisionObject.end(); ++it)
	{
		pOther = (*it)->m_pbtCollisionObject;

		btTransform t = pOther->getWorldTransform();
		btVector3 min, max;
		pOther->getRootCollisionShape()->getAabb(t, min, max);

		XY center(t.getOrigin().x(), t.getOrigin().z());
		float halfDimension_x = (max.x() - min.x()) * 0.5f;
		float halfDimension_z = (max.z() - min.z()) * 0.5f;

		if (Util::CircleRectangleCollisionDectection(vec3Center, fRadius, AABB(center, halfDimension_x, halfDimension_z)))
		{
			listItem->push_back(make_pair((*it)->m_nID, pOther->getWorldTransform().getOrigin()));
		}
	}

	return listItem->size() > 0;
}

bool CollisionManager::CehckExistInRange(int nID, float fRadius, int nTypes, list<pair<int, btVector3>>* listItem)
{
	listItem->clear();

	btVector3& vec3Center = m_mapCollisionObject[nID]->m_pbtCollisionObject->getWorldTransform().getOrigin();

	list<CollisionObject*> listCollisionObject = GetCollisionObjects(nTypes, AABB(XY(vec3Center.x(), vec3Center.z()), fRadius, fRadius));

	btCollisionObject* pOther = NULL;
	for (list<CollisionObject*>::iterator it = listCollisionObject.begin(); it != listCollisionObject.end(); ++it)
	{
		if ((*it)->m_nID == nID)
			continue;

		pOther = (*it)->m_pbtCollisionObject;

		btTransform t = pOther->getWorldTransform();
		btVector3 min, max;
		pOther->getRootCollisionShape()->getAabb(t, min, max);

		XY center(t.getOrigin().x(), t.getOrigin().z());
		float halfDimension_x = (max.x() - min.x()) * 0.5f;
		float halfDimension_z = (max.z() - min.z()) * 0.5f;

		if (Util::CircleRectangleCollisionDectection(vec3Center, fRadius, AABB(center, halfDimension_x, halfDimension_z)))
		{
			listItem->push_back(make_pair((*it)->m_nID, pOther->getWorldTransform().getOrigin()));
		}
	}

	return listItem->size() > 0;
}

void CollisionManager::RemoveCollisionObject(int nID)
{
	CollisionObject* pCollisionObject = m_mapCollisionObject[nID];

	pCollisionObject->m_pQuadTree->Remove(pCollisionObject);

	delete pCollisionObject;
	m_mapCollisionObject.erase(nID);

	for (map<CollisionObject::Type, map<int, CollisionObject*>>::iterator it = m_mapTypeCollisionObject.begin(); it != m_mapTypeCollisionObject.end(); ++it)
	{
		if (it->second.count(nID) > 0)
		{
			it->second.erase(nID);
			break;
		}
	}
}

list<CollisionObject*> CollisionManager::GetCollisionObjects(int nTypes, AABB range)
{
	list<CollisionObject*> listCollisionObject;
	list<CollisionObject*> listQueried;
	for (map<CollisionObject::Type, QuadTree<CollisionObject*, CollisionObjectInsertChecker>>::iterator it = m_mapQuadTree.begin(); it != m_mapQuadTree.end(); ++it)
	{
		if ((it->first & nTypes) == it->first)
		{
			listQueried = it->second.QueryRange(range);

			listCollisionObject.insert(listCollisionObject.end(), listQueried.begin(), listQueried.end());
		}
	}

	return listCollisionObject;
}

btConvexShape* CollisionManager::GetConvexShape(btCollisionObject* pbtCollisionObject)
{
	btCollisionShape* pCollisionShape = pbtCollisionObject->getRootCollisionShape();

	if (pCollisionShape->getShapeType() == COMPOUND_SHAPE_PROXYTYPE)
	{
		btCompoundShape* pCompoundShape = (btCompoundShape*)pCollisionShape;

		return (btConvexShape*)pCompoundShape->getChildShape(0);
	}
	else
	{
		return (btConvexShape*)pCollisionShape;
	}
}

CollisionObject* CollisionManager::GetCollisionObject(int nID)
{
	return m_mapCollisionObject[nID];
}