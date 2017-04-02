#pragma once

// Simple coordinate object to represent points and vectors
struct XY
{
public:
	XY() {}
	XY(float x, float y) { this->x = x; this->y = y; }

public:
	float x = 0;
	float y = 0;
};

// Axis-aligned bounding box with half dimension and center
struct AABB
{
public:
	AABB() {}
	AABB(XY center, float halfDimension) { this->center = center; this->halfDimension = halfDimension; }

public:
	XY center;
	float halfDimension = 0;

public:
	bool ContainsPoint(XY point);
	bool IntersectsAABB(AABB other);
};