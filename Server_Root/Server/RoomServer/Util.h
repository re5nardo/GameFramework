#pragma once

#define _USE_MATH_DEFINES
#include <math.h>
#include <sstream>
#include <vector>
#include "btBulletCollisionCommon.h"
#include "../CommonSources/QuadTreePrerequisites.h"

using namespace std;

class Util
{
public:
	static float Lerp(float a, float b, float t)
	{
		if (t > 1) t = 1;

		return a + (b - a) * t;
	}

	static btVector3 Lerp(btVector3& a, btVector3& b, float t)
	{
		if (t > 1) t = 1;

		btVector3 output;

		output.setX(Lerp(a.x(), b.x(), t));
		output.setY(Lerp(a.y(), b.y(), t));
		output.setZ(Lerp(a.z(), b.z(), t));

		return output;
	}

	//	http://quat.zachbennett.com/
	static btVector3 QuaternionToDegrees(const btQuaternion& q)
	{
		btVector3 d;

		float qw = q.w();
		float qx = q.x();
		float qy = q.y();
		float qz = q.z();

		float qw2 = qw * qw;
		float qx2 = qx * qx;
		float qy2 = qy * qy;
		float qz2 = qz * qz;

		float test = qx * qy + qz * qw;
		if (test > 0.499)
		{
			d.setY(360 / M_PI * atan2(qx, qw));
			d.setZ(90);
			d.setX(0);
			return d;
		}
		if (test < -0.499)
		{
			d.setY(-360 / M_PI * atan2(qx, qw));
			d.setZ(-90);
			d.setX(0);
			return d;
		}
		float h = atan2(2 * qy * qw - 2 * qx*qz, 1 - 2 * qy2 - 2 * qz2);
		float a = asin(2 * qx * qy + 2 * qz * qw);
		float b = atan2(2 * qx * qw - 2 * qy * qz, 1 - 2 * qx2 - 2 * qz2);

		d.setY(round(h * 180 / M_PI));
		d.setZ(round(a * 180 / M_PI));
		d.setX(round(b * 180 / M_PI));

		return d;
	}

	//	http://quat.zachbennett.com/
	static btQuaternion DegreesToQuaternion(btVector3& d)
	{
		float h = d.y() * M_PI / 360;
		float a = d.z() * M_PI / 360;
		float b = d.x() * M_PI / 360;

		float c1 = cos(h);
		float c2 = cos(a);
		float c3 = cos(b);
		float s1 = sin(h);
		float s2 = sin(a);
		float s3 = sin(b);

		btQuaternion q;

		q.setW(round((c1*c2*c3 - s1*s2*s3) * 100000) / 100000);
		q.setX(round((s1*s2*c3 + c1*c2*s3) * 100000) / 100000);
		q.setY(round((s1*c2*c3 + c1*s2*s3) * 100000) / 100000);
		q.setZ(round((c1*s2*c3 - s1*c2*s3) * 100000) / 100000);

		return q;
	}

	static void StringSplit(string text, char delim, vector<string>* output)
	{
		stringstream ss;
		ss.str(text);
		string item;
		vector<string> vecText;
		while (getline(ss, item, delim))
		{
			output->push_back(item);
		}
	}

	static btVector3 StringToVector3(string text)
	{
		if (text.at(0) == '(' && text.at(text.length() - 1) == ')')
		{
			text = text.substr(1, text.length() - 2);
		}

		vector<string> output;
		StringSplit(text, ',', &output);

		return btVector3(atof(output[0].c_str()), atof(output[1].c_str()), atof(output[2].c_str()));
	}

	static bool IsIntersect(AABB aabb, btCollisionObject* collisionObject)
	{
		btTransform t;
		btVector3 min, max;
		collisionObject->getRootCollisionShape()->getAabb(t, min, max);
		btVector3 origin = t.getOrigin();

		if (aabb.center.x - aabb.halfDimension < origin.x() + min.x())
		{
			return false;
		}
		else if (aabb.center.x + aabb.halfDimension > origin.x() + max.x())
		{
			return false;
		}
		else if (aabb.center.y - aabb.halfDimension < origin.z() + min.z())
		{
			return false;
		}
		else if (aabb.center.y + aabb.halfDimension > origin.z() + max.z())
		{
			return false;
		}

		return true;
	}

	static float GetAngle_Y(btVector3& vec3Target)
	{
		btVector3 criteria(0, 0, 1);
		btScalar angle = 0;

		angle = criteria.angle(vec3Target) * 180 / M_PI;
		if (criteria.x() >= vec3Target.x())
		{
			angle = 360 - angle;
		}

		return angle;
	}
};