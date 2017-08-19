#include "stdafx.h"
#include "QuadTreePrerequisites.h"

bool AABB::ContainsPoint(XY point)
{
	if (center.x - halfDimension_x > point.x || center.x + halfDimension_x < point.x || center.y - halfDimension_y > point.y || center.y + halfDimension_y < point.y)
	{
		return false;
	}

	return true;
}


bool AABB::IntersectsAABB(AABB other)
{
	if (center.x - halfDimension_x > other.center.x + other.halfDimension_x || center.x + halfDimension_x < other.center.x - other.halfDimension_x
		|| center.y - halfDimension_y > other.center.y + other.halfDimension_y || center.y + halfDimension_y < other.center.y - other.halfDimension_y)
	{
		return false;
	}

	return true;
}