#pragma once

#include <vector>
#include <math.h>

using namespace std;

namespace MathematicalData
{
	struct Vector3
	{
	public:
		Vector3() {};
		Vector3(float x, float y, float z) { this->x = x; this->y = y; this->z = z; };

	public:
		float x = 0.0f;
		float y = 0.0f;
		float z = 0.0f;

	public:
		Vector3 operator-(Vector3 other)
		{
			Vector3 output;

			output.x = this->x - other.x;
			output.y = this->y - other.y;
			output.z = this->z - other.z;

			return output;
		}

		Vector3 operator+(Vector3 other)
		{
			Vector3 output;

			output.x = this->x + other.x;
			output.y = this->y + other.y;
			output.z = this->z + other.z;

			return output;
		}

		friend Vector3 operator*(float fValue, Vector3 vec3Value);

		float GetMagnitude()
		{
			return sqrtf(x * x + y * y + z * z);
		}

		Vector3 GetNormalized()
		{
			float v = GetMagnitude();

			Vector3 output;

			output.x = x / v;
			output.y = y / v;
			output.z = z / v;

			return output;
		}

	public:
		static float Dot(Vector3 a, Vector3 b)
		{
			return a.x * b.x + a.y * b.y + a.z * b.z;
		}

		static Vector3 Cross(Vector3 a, Vector3 b)
		{
			Vector3 output;

			output.x = a.y * b.z - a.z * b.y;
			output.y = a.z * b.x - a.x * b.z;
			output.z = a.x * b.y - a.y * b.x;

			return output;
		}
	};

	struct Vector2
	{
	public:
		Vector2() {};
		Vector2(float x, float y) { this->x = x; this->y = y; };

	public:
		float x = 0.0f;
		float y = 0.0f;
	};

	struct Polygon
	{
	public:
		vector<Vector3> m_vecVertex;
	};

	struct Line2D
	{
	public:
		Line2D(Vector2 start, Vector2 end) { this->start = start; this->end = end; }

	public:
		Vector2 start;
		Vector2 end;
	};

	struct Sphere
	{
	public:
		Sphere(Vector3 center, float radius) { this->center = center; this->radius = radius; }

	public:
		Vector3 center;
		float radius = 0.0f;
	};
}