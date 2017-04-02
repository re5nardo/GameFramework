#include "stdafx.h"
#include "QuadTreePrerequisites.h"

bool AABB::ContainsPoint(XY point)
{
	if (center.x - halfDimension > point.x || center.x + halfDimension < point.x || center.y - halfDimension > point.y || center.y + halfDimension < point.y)
	{
		return false;
	}

	return true;
}


bool AABB::IntersectsAABB(AABB other)
{
	if (center.x - halfDimension > other.center.x + other.halfDimension || center.x + halfDimension < other.center.x - other.halfDimension
		|| center.y - halfDimension > other.center.y + other.halfDimension || center.y + halfDimension < other.center.y - other.halfDimension)
	{
		return false;
	}

	return true;
}