#pragma once

#include "../../CommonSources/QuadTreePrerequisites.h"
#include "../../CommonSources/MathematicalData.h"

using namespace std;

class TerrainObjectInsertChecker
{
public:
	TerrainObjectInsertChecker();
	virtual ~TerrainObjectInsertChecker();

public:
	bool IsValidate(AABB boundary, MathematicalData::Polygon terrainObject);
	bool IsMine(AABB boundary, MathematicalData::Polygon terrainObject);
};