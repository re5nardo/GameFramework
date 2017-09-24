#pragma once

#include <list>
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
	list<T> points;

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

	list<T> QueryRange(AABB range)
	{
		// Prepare an array of results
		list<T> pointsInRange;

		// Automatically abort if the range does not intersect this quad
		if (!boundary.IntersectsAABB(range))
			return pointsInRange; // empty list

		// Check objects at this quad level
		for (list<T>::iterator it = points.begin(); it != points.end(); ++it)
		{
			if (range.IntersectsAABB((*it).GetAABB()))
				pointsInRange.push_back(*it);
		}

		// Terminate here, if there are no children
		if (northWest == NULL)
			return pointsInRange;

		// Otherwise, add the points from the children
		list<T> listQueried = northWest->QueryRange(range);
		pointsInRange.insert(pointsInRange.end(), listQueried.begin(), listQueried.end());

		listQueried = northEast->QueryRange(range);
		pointsInRange.insert(pointsInRange.end(), listQueried.begin(), listQueried.end());

		listQueried = southWest->QueryRange(range);
		pointsInRange.insert(pointsInRange.end(), listQueried.begin(), listQueried.end());

		listQueried = southEast->QueryRange(range);
		pointsInRange.insert(pointsInRange.end(), listQueried.begin(), listQueried.end());

		return pointsInRange;
	}

	bool Remove(T p)
	{
		for (list<T>::iterator it = points.begin(); it != points.end(); ++it)
		{
			if (p == (*it))
			{
				points.erase(it);
				return true;
			}
		}

		return false;
	}

	void Transform(T p)
	{
		points.remove(p);

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
	list<T*> points;

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

	list<T*> QueryRange(AABB range)
	{
		// Prepare an array of results
		list<T*> pointsInRange;

		// Automatically abort if the range does not intersect this quad
		if (!boundary.IntersectsAABB(range))
			return pointsInRange; // empty list

		// Check objects at this quad level
		for (list<T*>::iterator it = points.begin(); it != points.end(); ++it)
		{
			if (range.IntersectsAABB((*it)->GetAABB()))
				pointsInRange.push_back(*it);
		}

		// Terminate here, if there are no children
		if (northWest == NULL)
			return pointsInRange;

		// Otherwise, add the points from the children
		list<T*> listQueried = northWest->QueryRange(range);
		pointsInRange.insert(pointsInRange.end(), listQueried.begin(), listQueried.end());

		listQueried = northEast->QueryRange(range);
		pointsInRange.insert(pointsInRange.end(), listQueried.begin(), listQueried.end());

		listQueried = southWest->QueryRange(range);
		pointsInRange.insert(pointsInRange.end(), listQueried.begin(), listQueried.end());

		listQueried = southEast->QueryRange(range);
		pointsInRange.insert(pointsInRange.end(), listQueried.begin(), listQueried.end());

		return pointsInRange;
	}

	bool Remove(T* p)
	{
		for (list<T*>::iterator it = points.begin(); it != points.end(); ++it)
		{
			if (p == (*it))
			{
				points.erase(it);
				return true;
			}
		}

		return false;
	}

	void Transform(T* p)
	{
		points.remove(p);

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