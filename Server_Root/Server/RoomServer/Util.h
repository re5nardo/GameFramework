#pragma once

#include "../CommonSources/MathematicalData.h"

using namespace std;

class Util
{
public:
	static float Lerp(float a, float b, float t)
	{
		return a + (b - a) * t;
	}

	static Vector3 Lerp(Vector3 a, Vector3 b, float t)
	{
		Vector3 output;

		output.x = Lerp(a.x, b.x, t);
		output.y = Lerp(a.y, b.y, t);
		output.z = Lerp(a.z, b.z, t);

		return output;
	}
};