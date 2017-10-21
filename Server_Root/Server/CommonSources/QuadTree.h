#pragma once

#include <list>
#include <vector>
#include "QuadTreePrerequisites.h"

using namespace std;

template <typename T, typename U>
class QuadTree
{
public:
	QuadTree()
	{
	}
	QuadTree(QuadTree* parent)
	{
		this->parent = parent;
	}
	QuadTree(QuadTree* parent, AABB boundary)
	{
		this->parent = parent;
		this->boundary = boundary;
	}
	virtual ~QuadTree()
	{
		Clear();
	}

private:
	// Arbitrary constant to indicate how many elements can be stored in this quad tree node
	const int QT_NODE_CAPACITY = 4;

	U InsertChecker;

	// Axis-aligned bounding box stored as a center with half-dimensions
	// to represent the boundaries of this quad tree
	AABB boundary;

	// Points in this quad tree node
	vector<T> points;

	//	QueryRange(AABB range) output
	list<T> pointsInRange;

	// Children
	QuadTree* northWest = NULL;
	QuadTree* northEast = NULL;
	QuadTree* southWest = NULL;
	QuadTree* southEast = NULL;

	//	Parent
	QuadTree* parent = NULL;

public:
	void SetBoundary(AABB boundary)
	{
		this->boundary = boundary;
	}

	void Clear()
	{
		points.clear();

		if (northWest == NULL)
		{
			delete northWest;
			delete northEast;
			delete southWest;
			delete southEast;

			northWest = NULL;
			northEast = NULL;
			southWest = NULL;
			southEast = NULL;
		}

		parent = NULL;
	}

	bool Insert(T p)
	{
		if (!InsertChecker.IsValidate(boundary, p))
			return false;

		if (InsertChecker.IsMine(boundary, p) || points.size() < QT_NODE_CAPACITY)
		{
			points.push_back(p);

			p.m_pQuadTree = this;

			return true;
		}

		// Otherwise, subdivide and then add the point to whichever node will accept it
		if (northWest == NULL)
			Subdivide();

		if (northWest->Insert(p)) return true;
		if (northEast->Insert(p)) return true;
		if (southWest->Insert(p)) return true;
		if (southEast->Insert(p)) return true;

		// Otherwise, the point cannot be inserted for some unknown reason (this should never happen)
		return false;
	}

	void Subdivide()
	{
		northWest = new QuadTree(this, AABB(XY(boundary.center.x - boundary.halfDimension_x * 0.5f, boundary.center.y + boundary.halfDimension_y * 0.5f), boundary.halfDimension_x * 0.5f, boundary.halfDimension_y * 0.5f));
		northEast = new QuadTree(this, AABB(XY(boundary.center.x + boundary.halfDimension_x * 0.5f, boundary.center.y + boundary.halfDimension_y * 0.5f), boundary.halfDimension_x * 0.5f, boundary.halfDimension_y * 0.5f));
		southWest = new QuadTree(this, AABB(XY(boundary.center.x - boundary.halfDimension_x * 0.5f, boundary.center.y - boundary.halfDimension_y * 0.5f), boundary.halfDimension_x * 0.5f, boundary.halfDimension_y * 0.5f));
		southEast = new QuadTree(this, AABB(XY(boundary.center.x + boundary.halfDimension_x * 0.5f, boundary.center.y - boundary.halfDimension_y * 0.5f), boundary.halfDimension_x * 0.5f, boundary.halfDimension_y * 0.5f));
	}

	list<T>* QueryRange(AABB range)
	{
		pointsInRange.clear();

		// Automatically abort if the range does not intersect this quad
		if (!boundary.IntersectsAABB(range))
			return &pointsInRange; // empty list

		// Check objects at this quad level
		for (int i = 0; i < points.size(); ++i)
		{
			if (range.IntersectsAABB(points[i].GetAABB()))
				pointsInRange.push_back(points[i]);
		}

		// Terminate here, if there are no children
		if (northWest == NULL)
			return &pointsInRange;

		// Otherwise, add the points from the children
		list<T>* plistQueried = northWest->QueryRange(range);
		pointsInRange.insert(pointsInRange.end(), plistQueried->begin(), plistQueried->end());

		plistQueried = northEast->QueryRange(range);
		pointsInRange.insert(pointsInRange.end(), plistQueried->begin(), plistQueried->end());

		plistQueried = southWest->QueryRange(range);
		pointsInRange.insert(pointsInRange.end(), plistQueried->begin(), plistQueried->end());

		plistQueried = southEast->QueryRange(range);
		pointsInRange.insert(pointsInRange.end(), plistQueried->begin(), plistQueried->end());

		return &pointsInRange;
	}

	bool Remove(T p)
	{
		for (int i = 0; i < points.size(); ++i)
		{
			if (p == points[i])
			{
				points.erase(points.begin() + i);
				return true;
			}
		}

		return false;
	}

	void Transform(T p)
	{
		for (int i = 0; i < points.size(); ++i)
		{
			if (p == points[i])
			{
				points.erase(points.begin() + i);
				break;
			}
		}

		if (InsertChecker.IsValidate(boundary, p))
		{
			if (InsertChecker.IsMine(boundary, p) || points.size() < QT_NODE_CAPACITY)
			{
				points.push_back(p);

				p.m_pQuadTree = this;
			}
			else
			{
				if (northWest == NULL)
					Subdivide();

				if (northWest->Insert(p)) return;
				if (northEast->Insert(p)) return;
				if (southWest->Insert(p)) return;
				if (southEast->Insert(p)) return;
			}
		}
		else
		{
			if (parent != NULL)
				parent->TryMoveToParent(p);
		}
	}

	void TryMoveToParent(T p)
	{
		if (Insert(p) == false && parent != NULL)
		{
			parent->TryMoveToParent(p);
		}
	}
};

template <typename T, typename U>
class QuadTree<T*, U>
{
public:
	QuadTree()
	{
	}
	QuadTree(QuadTree* parent)
	{
		this->parent = parent;
	}
	QuadTree(QuadTree* parent, AABB boundary)
	{
		this->parent = parent;
		this->boundary = boundary;
	}
	virtual ~QuadTree()
	{
		Clear();
	}

private:
	// Arbitrary constant to indicate how many elements can be stored in this quad tree node
	const int QT_NODE_CAPACITY = 4;

	U InsertChecker;

	// Axis-aligned bounding box stored as a center with half-dimensions
	// to represent the boundaries of this quad tree
	AABB boundary;

	// Points in this quad tree node
	vector<T*> points;

	//	QueryRange(AABB range) output
	list<T*> pointsInRange;

	// Children
	QuadTree* northWest = NULL;
	QuadTree* northEast = NULL;
	QuadTree* southWest = NULL;
	QuadTree* southEast = NULL;

	//	Parent
	QuadTree* parent = NULL;

public:
	void SetBoundary(AABB boundary)
	{
		this->boundary = boundary;
	}

	void Clear()
	{
		points.clear();

		if (northWest == NULL)
		{
			delete northWest;
			delete northEast;
			delete southWest;
			delete southEast;

			northWest = NULL;
			northEast = NULL;
			southWest = NULL;
			southEast = NULL;
		}

		parent = NULL;
	}

	bool Insert(T* p)
	{
		if (!InsertChecker.IsValidate(boundary, p))
			return false;

		if (InsertChecker.IsMine(boundary, p) || points.size() < QT_NODE_CAPACITY)
		{
			points.push_back(p);

			p->m_pQuadTree = this;

			return true;
		}

		// Otherwise, subdivide and then add the point to whichever node will accept it
		if (northWest == NULL)
			Subdivide();

		if (northWest->Insert(p)) return true;
		if (northEast->Insert(p)) return true;
		if (southWest->Insert(p)) return true;
		if (southEast->Insert(p)) return true;

		// Otherwise, the point cannot be inserted for some unknown reason (this should never happen)
		return false;
	}

	void Subdivide()
	{
		northWest = new QuadTree(this, AABB(XY(boundary.center.x - boundary.halfDimension_x * 0.5f, boundary.center.y + boundary.halfDimension_y * 0.5f), boundary.halfDimension_x * 0.5f, boundary.halfDimension_y * 0.5f));
		northEast = new QuadTree(this, AABB(XY(boundary.center.x + boundary.halfDimension_x * 0.5f, boundary.center.y + boundary.halfDimension_y * 0.5f), boundary.halfDimension_x * 0.5f, boundary.halfDimension_y * 0.5f));
		southWest = new QuadTree(this, AABB(XY(boundary.center.x - boundary.halfDimension_x * 0.5f, boundary.center.y - boundary.halfDimension_y * 0.5f), boundary.halfDimension_x * 0.5f, boundary.halfDimension_y * 0.5f));
		southEast = new QuadTree(this, AABB(XY(boundary.center.x + boundary.halfDimension_x * 0.5f, boundary.center.y - boundary.halfDimension_y * 0.5f), boundary.halfDimension_x * 0.5f, boundary.halfDimension_y * 0.5f));
	}

	list<T*>* QueryRange(AABB range)
	{
		pointsInRange.clear();

		// Automatically abort if the range does not intersect this quad
		if (!boundary.IntersectsAABB(range))
			return &pointsInRange; // empty list

		// Check objects at this quad level
		for (int i = 0; i < points.size(); ++i)
		{
			if (range.IntersectsAABB(points[i]->GetAABB()))
				pointsInRange.push_back(points[i]);
		}

		// Terminate here, if there are no children
		if (northWest == NULL)
			return &pointsInRange;

		// Otherwise, add the points from the children
		list<T*>* plistQueried = northWest->QueryRange(range);
		pointsInRange.insert(pointsInRange.end(), plistQueried->begin(), plistQueried->end());

		plistQueried = northEast->QueryRange(range);
		pointsInRange.insert(pointsInRange.end(), plistQueried->begin(), plistQueried->end());

		plistQueried = southWest->QueryRange(range);
		pointsInRange.insert(pointsInRange.end(), plistQueried->begin(), plistQueried->end());

		plistQueried = southEast->QueryRange(range);
		pointsInRange.insert(pointsInRange.end(), plistQueried->begin(), plistQueried->end());

		return &pointsInRange;
	}

	bool Remove(T* p)
	{
		for (int i = 0; i < points.size(); ++i)
		{
			if (p == points[i])
			{
				points.erase(points.begin() + i);
				return true;
			}
		}

		return false;
	}

	void Transform(T* p)
	{
		for (int i = 0; i < points.size(); ++i)
		{
			if (p == points[i])
			{
				points.erase(points.begin() + i);
				break;
			}
		}

		if (InsertChecker.IsValidate(boundary, p))
		{
			if (InsertChecker.IsMine(boundary, p) || points.size() < QT_NODE_CAPACITY)
			{
				points.push_back(p);

				p->m_pQuadTree = this;
			}
			else
			{
				if (northWest == NULL)
					Subdivide();

				if (northWest->Insert(p)) return;
				if (northEast->Insert(p)) return;
				if (southWest->Insert(p)) return;
				if (southEast->Insert(p)) return;
			}
		}
		else
		{
			if (parent != NULL)
				parent->TryMoveToParent(p);
		}
	}

	void TryMoveToParent(T* p)
	{
		if (Insert(p) == false && parent != NULL)
		{
			parent->TryMoveToParent(p);
		}
	}
};