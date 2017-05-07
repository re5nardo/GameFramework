#include "stdafx.h"
#include "TerrainObjectInsertChecker.h"
#include "../Util.h"

TerrainObjectInsertChecker::TerrainObjectInsertChecker()
{
}

TerrainObjectInsertChecker::~TerrainObjectInsertChecker()
{
}

bool TerrainObjectInsertChecker::IsValidate(AABB boundary, btCollisionObject* collisionObject)
{
	btVector3 min, max;

	collisionObject->getRootCollisionShape()->getAabb(collisionObject->getWorldTransform(), min, max);

	if (min.x() < boundary.center.x - boundary.halfDimension)
	{
		return false;
	}
	else if (max.x() > boundary.center.x + boundary.halfDimension)
	{
		return false;
	}
	else if (min.z() < boundary.center.y - boundary.halfDimension)
	{
		return false;
	}
	else if (max.z() > boundary.center.y + boundary.halfDimension)
	{
		return false;
	}

	return true;
}

bool TerrainObjectInsertChecker::IsMine(AABB boundary, btCollisionObject* collisionObject)
{
	int nIntersectCnt = 0;

	AABB northWest(AABB(XY(boundary.center.x - boundary.halfDimension * 0.5f, boundary.center.y + boundary.halfDimension * 0.5f), boundary.halfDimension * 0.5f));
	if (Util::IsIntersect(northWest, collisionObject))
	{
		nIntersectCnt++;
	}

	AABB northEast(AABB(XY(boundary.center.x + boundary.halfDimension * 0.5f, boundary.center.y + boundary.halfDimension * 0.5f), boundary.halfDimension * 0.5f));
	if (Util::IsIntersect(northEast, collisionObject))
	{
		nIntersectCnt++;
		if (nIntersectCnt > 1)
			return true;
	}

	AABB southWest(AABB(XY(boundary.center.x - boundary.halfDimension * 0.5f, boundary.center.y - boundary.halfDimension * 0.5f), boundary.halfDimension * 0.5f));
	if (Util::IsIntersect(southWest, collisionObject))
	{
		nIntersectCnt++;
		if (nIntersectCnt > 1)
			return true;
	}

	AABB southEast(AABB(XY(boundary.center.x + boundary.halfDimension * 0.5f, boundary.center.y - boundary.halfDimension * 0.5f), boundary.halfDimension * 0.5f));
	if (Util::IsIntersect(southEast, collisionObject))
	{
		nIntersectCnt++;
		if (nIntersectCnt > 1)
			return true;
	}

	return false;
}
