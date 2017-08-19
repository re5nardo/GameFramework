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
	AABB(XY center, float halfDimension_x, float halfDimension_y) { this->center = center; this->halfDimension_x = halfDimension_x; this->halfDimension_y = halfDimension_y; }

public:
	XY center;
	float halfDimension_x = 0;
	float halfDimension_y = 0;

public:
	bool ContainsPoint(XY point);
	bool IntersectsAABB(AABB other);
};