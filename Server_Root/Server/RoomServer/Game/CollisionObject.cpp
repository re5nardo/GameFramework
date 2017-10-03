#include "stdafx.h"
#include "CollisionObject.h"

CollisionObject::CollisionObject(int nID, Type type, btCollisionObject* pbtCollisionObject)
{
	m_nID = nID;
	m_Type = type;
	m_pbtCollisionObject = pbtCollisionObject;
}

CollisionObject::~CollisionObject()
{
	if (m_pbtCollisionObject != NULL)
	{
		btCollisionShape* pCollisionShape = m_pbtCollisionObject->getRootCollisionShape();

		if (pCollisionShape->getShapeType() == COMPOUND_SHAPE_PROXYTYPE)
		{
			btCompoundShape* pCompoundShape = (btCompoundShape*)pCollisionShape;

			int nNumChildShapes = pCompoundShape->getNumChildShapes();
			if (nNumChildShapes > 0)
			{
				for (int i = nNumChildShapes - 1; i >= 0; --i)
				{
					delete pCompoundShape->getChildShape(i);
					pCompoundShape->removeChildShapeByIndex(i);
				}
			}
		}

		delete pCollisionShape;
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