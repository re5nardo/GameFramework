#include "stdafx.h"
#include "CollisionObject.h"

CollisionObject::CollisionObject(int nID, btCollisionObject* pbtCollisionObject)
{
	m_nID = nID;
	m_pbtCollisionObject = pbtCollisionObject;
}

CollisionObject::~CollisionObject()
{
	if (m_pbtCollisionObject != NULL)
	{
		delete m_pbtCollisionObject->getRootCollisionShape();
		delete m_pbtCollisionObject;
	}
}

AABB CollisionObject::GetAABB()
{
	btTransform t = m_pbtCollisionObject->getWorldTransform();
	btVector3 min, max;
	m_pbtCollisionObject->getRootCollisionShape()->getAabb(t, min, max);

	XY center(t.getOrigin().x(), t.getOrigin().z());
	float halfDimension_x = (max.x() - min.x()) * 0.5f;
	float halfDimension_z = (max.z() - min.z()) * 0.5f;

	return AABB(center, halfDimension_x, halfDimension_z);
}