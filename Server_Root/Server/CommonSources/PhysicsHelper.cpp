#include "stdafx.h"
#include "PhysicsHelper.h"

namespace MathematicalData
{
	Vector3 operator*(float fValue, Vector3 vec3Value)
	{
		Vector3 output;

		output.x = vec3Value.x * fValue;
		output.y = vec3Value.y * fValue;
		output.z = vec3Value.z * fValue;

		return output;
	}
}

namespace PhysicsHelper
{
	bool Collision::FindLineAndLineIntersection(Line2D line1, Line2D line2, Vector2& intersectionPos)
	{
		float denom = ((line1.end.x - line1.start.x) * (line2.end.y - line2.start.y)) - ((line1.end.y - line1.start.y) * (line2.end.x - line2.start.x));

		//  AB & CD are parallel 
		if (abs(denom - 0.0f) <= std::numeric_limits<float>::epsilon())
			return false;

		float numer = ((line1.start.y - line2.start.y) * (line2.end.x - line2.start.x)) - ((line1.start.x - line2.start.x) * (line2.end.y - line2.start.y));

		float r = numer / denom;

		float numer2 = ((line1.start.y - line2.start.y) * (line1.end.x - line1.start.x)) - ((line1.start.x - line2.start.x) * (line1.end.y - line1.start.y));

		float s = numer2 / denom;

		if ((r < 0 || r > 1) || (s < 0 || s > 1))
			return false;

		// Find intersection point
		intersectionPos.x = line1.start.x + (r * (line1.end.x - line1.start.x));
		intersectionPos.y = line1.start.y + (r * (line1.end.y - line1.start.y));

		return true;
	}

	bool Collision::FindLineAndPolygonIntersection(Line2D line, MathematicalData::Polygon polygon, Vector2& intersectionPos)
	{
		if (polygon.m_vecVertex.size() < 3)
		{
			return false;
		}

		vector<Line2D> vecLine;

		for (int i = 1; i < polygon.m_vecVertex.size(); ++i)
		{
			Vector2 start(polygon.m_vecVertex[i - 1].x, polygon.m_vecVertex[i - 1].z);
			Vector2 end(polygon.m_vecVertex[i].x, polygon.m_vecVertex[i].z);

			vecLine.push_back(Line2D(start, end));
		}

		//  first and last vertex
		{
			Vector2 start(polygon.m_vecVertex[polygon.m_vecVertex.size() - 1].x, polygon.m_vecVertex[polygon.m_vecVertex.size() - 1].z);
			Vector2 end(polygon.m_vecVertex[0].x, polygon.m_vecVertex[0].z);

			vecLine.push_back(Line2D(start, end));
		}

		for (vector<Line2D>::iterator it = vecLine.begin(); it != vecLine.end(); ++it)
		{
			if (FindLineAndLineIntersection(line, *it, intersectionPos))
			{
				return true;
			}
		}

		return false;
	}

	// Test if point p lies inside ccw-specified convex n-gon given by vertices v[]
	bool Collision::PointInConvexPolygon(Vector3 p, MathematicalData::Polygon polygon)
	{
		// Do binary search over polygon vertices to find the fan triangle
		// (v[0], v[low], v[high]) the point p lies within the near sides of
		int low = 0, high = polygon.m_vecVertex.size();
		do {
			int mid = (low + high) / 2;
			if (TriangleIsCCW(polygon.m_vecVertex[0], polygon.m_vecVertex[mid], p))
				low = mid;
			else
				high = mid;
		} while (low + 1 < high);

		// If point outside last (or first) edge, then it is not inside the n-gon
		if (low == 0 || high == polygon.m_vecVertex.size()) return false;

		// p is inside the polygon if it is left of
		// the directed edge from v[low] to v[high]
		return TriangleIsCCW(polygon.m_vecVertex[low], polygon.m_vecVertex[high], p);
	}

	bool Collision::TriangleIsCCW(Vector3 a, Vector3 b, Vector3 c)
	{
		float sum = 0.0f;

		sum += (b.x - a.x) * (b.z + a.z);
		sum += (c.x - b.x) * (c.z + b.z);
		sum += (a.x - c.x) * (a.z + c.z);

		//  negative : ccw, positive : cw
		return sum < 0;
	}
}