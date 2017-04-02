#pragma once

#include <vector>
#include "../CommonSources/MathematicalData.h"

using namespace std;

struct MapData
{
public:
	int nID = 0;
	float fWidth = 0.0f;
	float fHeight = 0.0f;
	vector<MathematicalData::Polygon*> vecTerrainObject;
};

struct Stat
{
public:
	Stat(){ fSpeed = 0.0f; fAgility = 0.0f; };
	Stat(float fSpeed, float fAgility){ this->fSpeed = fSpeed; this->fAgility = fAgility; };
public:
	float fSpeed = 0.0f;
	float fAgility = 0.0f;
};