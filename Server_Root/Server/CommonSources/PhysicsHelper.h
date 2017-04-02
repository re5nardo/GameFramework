#pragma once

#include "MathematicalData.h"

using namespace MathematicalData;

namespace PhysicsHelper
{
	class Collision
	{
	private:
		static bool TriangleIsCCW(Vector3 a, Vector3 b, Vector3 c);

	public:
		static bool FindLineAndLineIntersection(Line2D line1, Line2D line2, Vector2& intersectionPos);
		static bool FindLineAndPolygonIntersection(Line2D line, Polygon polygon, Vector2& intersectionPos);
		static bool PointInConvexPolygon(Vector3 p, Polygon polygon);
	};
}