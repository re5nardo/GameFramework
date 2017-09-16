#include "stdafx.h"
#include "TerrainObjectInsertChecker.h"
#include "CollisionObject.h"

TerrainObjectInsertChecker::TerrainObjectInsertChecker()
{
}

TerrainObjectInsertChecker::~TerrainObjectInsertChecker()
{
}

bool TerrainObjectInsertChecker::IsValidate(AABB boundary, CollisionObject* pCollisionObject)
{
	btTransform t = pCollisionObject->m_pbtCollisionObject->getWorldTransform();
	btVector3 min, max;

	pCollisionObject->m_pbtCollisionObject->getRootCollisionShape()->getAabb(t, min, max);

	if (min.x() < boundary.center.x - boundary.halfDimension_x)
	{
		return false;
	}
	else if (max.x() > boundary.center.x + boundary.halfDimension_x)
	{
		return false;
	}
	else if (min.z() < boundary.center.y - boundary.halfDimension_y)
	{
		return false;
	}
	else if (max.z() > boundary.center.y + boundary.halfDimension_y)
	{
		return false;
	}

	return true;
}

bool TerrainObjectInsertChecker::IsMine(AABB boundary, CollisionObject* pCollisionObject)
{
	int nIntersectCnt = 0;

	AABB northWest(AABB(XY(boundary.center.x - boundary.halfDimension_x * 0.5f, boundary.center.y + boundary.halfDimension_y * 0.5f), boundary.halfDimension_x * 0.5f, boundary.halfDimension_y * 0.5f));
	if (northWest.IntersectsAABB(pCollisionObject->GetAABB()))
	{
		nIntersectCnt++;
	}

	AABB northEast(AABB(XY(boundary.center.x + boundary.halfDimension_x * 0.5f, boundary.center.y + boundary.halfDimension_y * 0.5f), boundary.halfDimension_x * 0.5f, boundary.halfDimension_y * 0.5f));
	if (northEast.IntersectsAABB(pCollisionObject->GetAABB()))
	{
		nIntersectCnt++;
		if (nIntersectCnt > 1)
			return true;
	}

	AABB southWest(AABB(XY(boundary.center.x - boundary.halfDimension_x * 0.5f, boundary.center.y - boundary.halfDimension_y * 0.5f), boundary.halfDimension_x * 0.5f, boundary.halfDimension_y * 0.5f));
	if (southWest.IntersectsAABB(pCollisionObject->GetAABB()))
	{
		nIntersectCnt++;
		if (nIntersectCnt > 1)
			return true;
	}

	AABB southEast(AABB(XY(boundary.center.x + boundary.halfDimension_x * 0.5f, boundary.center.y - boundary.halfDimension_y * 0.5f), boundary.halfDimension_x * 0.5f, boundary.halfDimension_y * 0.5f));
	if (southEast.IntersectsAABB(pCollisionObject->GetAABB()))
	{
		nIntersectCnt++;
		if (nIntersectCnt > 1)
			return true;
	}

	return false;
}
