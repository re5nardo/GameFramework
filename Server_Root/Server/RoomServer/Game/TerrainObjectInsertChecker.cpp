#include "stdafx.h"
#include "TerrainObjectInsertChecker.h"

TerrainObjectInsertChecker::TerrainObjectInsertChecker()
{
}

TerrainObjectInsertChecker::~TerrainObjectInsertChecker()
{
}

bool TerrainObjectInsertChecker::IsValidate(AABB boundary, MathematicalData::Polygon terrainObject)
{
	return true;
}

bool TerrainObjectInsertChecker::IsMine(AABB boundary, MathematicalData::Polygon terrainObject)
{
	return false;
}
