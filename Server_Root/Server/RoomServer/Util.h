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

	static void Parse(string text, char delim, vector<string>* output)
	{
		stringstream ss;
		ss.str(text);
		string item;
		output->clear();
		while (getline(ss, item, delim))
		{
			output->push_back(item);
		}
	}

	static void Parse(string text, char delim, vector<int>* output)
	{
		vector<string> temp;
		Parse(text, delim, &temp);

		output->clear();
		for (vector<string>::iterator it = temp.begin(); it != temp.end(); ++it)
		{
			output->push_back(atoi((*it).c_str()));
		}
	}

	static void Parse(string text, char delim, vector<double>* output)
	{
		vector<string> temp;
		Parse(text, delim, &temp);

		output->clear();
		for (vector<string>::iterator it = temp.begin(); it != temp.end(); ++it)
		{
			output->push_back(atof((*it).c_str()));
		}
	}

	static btVector3 StringToVector3(string text)
	{
		if (text.at(0) == '(' && text.at(text.length() - 1) == ')')
		{
			text = text.substr(1, text.length() - 2);
		}

		vector<string> output;
		Parse(text, ',', &output);

		return btVector3(atof(output[0].c_str()), atof(output[1].c_str()), atof(output[2].c_str()));
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

	static btVector3 GetAngledPosition(btVector3& vec3Origin, float fAngle, float fLength)
	{
		float fRadian = (90 - fAngle) * M_PI / 180.0f;

		float x = cos(fRadian) * fLength;
		float z = sin(fRadian) * fLength;

		return btVector3(vec3Origin.x() + x, 0, vec3Origin.z() + z);
	}

	static btScalar GetDistance(btVector3& vec3One, btVector3& vec3Two)
	{
		return vec3One.distance(vec3Two);
	}

	static btScalar GetDistance2(btVector3& vec3One, btVector3& vec3Two)
	{
		return vec3One.distance2(vec3Two);
	}

	static bool IsEqual(float a, float b)
	{
		float diff = a > b ? a - b : b - a;

		return diff <= 0.000001f;
	}

	static bool IsEqual(btVector3& a, btVector3& b)
	{
		return IsEqual(a.x(), b.x()) && IsEqual(a.y(), b.y()) && IsEqual(a.z(), b.z());
	}

	//	https://stackoverflow.com/questions/401847/circle-rectangle-collision-detection-intersection
	static bool CircleRectangleCollisionDectection(btVector3& vec3Center, float fRadius, AABB rect)
	{
		float circleDistance_x = abs(vec3Center.x() - rect.halfDimension_x);
		float circleDistance_z = abs(vec3Center.z() - rect.halfDimension_y);

		if (circleDistance_x > (rect.halfDimension_x + fRadius)) { return false; }
		if (circleDistance_z > (rect.halfDimension_y + fRadius)) { return false; }

		if (circleDistance_x <= (rect.halfDimension_x)) { return true; }
		if (circleDistance_z <= (rect.halfDimension_y)) { return true; }

		float cornerDistance_sq = powf(circleDistance_x - rect.halfDimension_x, 2) + powf(circleDistance_z - rect.halfDimension_y, 2);

		return (cornerDistance_sq <= powf(fRadius, 2));
	}
};