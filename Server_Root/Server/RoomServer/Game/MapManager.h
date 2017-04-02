#pragma once

#include "../../CommonSources/MathematicalData.h"
#include "../../CommonSources/QuadTree.h"
#include "TerrainObjectInsertChecker.h"
#include "../Data.h"

using namespace std;
using namespace MathematicalData;

class MapManager
{
public:
	MapManager();
	virtual ~MapManager();

public:
	MapData m_MapData;
	QuadTree<MathematicalData::Polygon, TerrainObjectInsertChecker>* m_pTerrainObject = NULL;

public:
	void LoadMap(int nID);
	void Clear();
	bool IsPositionValidToMove(Vector3 vec3Pos);
	bool CanMoveStraightly(Vector3 start, Vector3 dest);
};