#include "stdafx.h"
#include "MapManager.h"
#include "../../CommonSources/tinyxml2.h"
#include "../../CommonSources/PhysicsHelper.h"
#include <sstream>

MapManager::MapManager()
{
}

MapManager::~MapManager()
{
	if (m_pTerrainObject != NULL)
		delete m_pTerrainObject;

	for (vector<MathematicalData::Polygon*>::iterator it = m_MapData.vecTerrainObject.begin(); it != m_MapData.vecTerrainObject.end(); ++it)
	{
		delete *it;
	}
	m_MapData.vecTerrainObject.clear();
}

void MapManager::Clear()
{
	if (m_pTerrainObject != NULL)
	{
		delete m_pTerrainObject;
		m_pTerrainObject = NULL;
	}

	for (vector<MathematicalData::Polygon*>::iterator it = m_MapData.vecTerrainObject.begin(); it != m_MapData.vecTerrainObject.end(); ++it)
	{
		delete *it;
	}
	m_MapData.vecTerrainObject.clear();
}

void MapManager::LoadMap(int nID)
{
	stringstream mapFileName;
	mapFileName << "map_" << to_string(nID) << ".xml";

	tinyxml2::XMLDocument doc;
	doc.LoadFile(mapFileName.str().c_str());

	tinyxml2::XMLElement* pMap = doc.FirstChildElement("Map");

	m_MapData.nID = atoi(pMap->FirstChildElement("ID")->GetText());
	m_MapData.fWidth = atof(pMap->FirstChildElement("Width")->GetText());
	m_MapData.fHeight = atof(pMap->FirstChildElement("Height")->GetText());

	for (vector<MathematicalData::Polygon*>::iterator it = m_MapData.vecTerrainObject.begin(); it != m_MapData.vecTerrainObject.end(); ++it)
	{
		delete *it;
	}
	m_MapData.vecTerrainObject.clear();

	tinyxml2::XMLElement* pTerrainObjectsElement = pMap->FirstChildElement("Obstacles");
	for (tinyxml2::XMLElement* pTerrainObjectElement = pTerrainObjectsElement->FirstChildElement("Obstacle"); pTerrainObjectElement; pTerrainObjectElement = pTerrainObjectElement->NextSiblingElement("Obstacle"))
	{
		MathematicalData::Polygon* pTerrainObject = new MathematicalData::Polygon();
		for (tinyxml2::XMLElement* pVertexElement = pTerrainObjectElement->FirstChildElement("Vertex"); pVertexElement; pVertexElement = pVertexElement->NextSiblingElement("Vertex"))
		{
			stringstream ss;
			ss.str(pVertexElement->GetText());
			string item;
			char delim = ',';
			int nCnt = 0;
			float arrValue[3];
			while (getline(ss, item, delim))
			{
				arrValue[nCnt++] = atof(item.c_str());
			}

			pTerrainObject->m_vecVertex.push_back(Vector3(arrValue[0], arrValue[1], arrValue[2]));
		}

		m_MapData.vecTerrainObject.push_back(pTerrainObject);
	}

	m_pTerrainObject = new QuadTree<MathematicalData::Polygon, TerrainObjectInsertChecker>(AABB(XY(0.0f, 0.0f), m_MapData.fWidth > m_MapData.fHeight ? m_MapData.fWidth : m_MapData.fHeight));
	for (vector<MathematicalData::Polygon*>::iterator it = m_MapData.vecTerrainObject.begin(); it != m_MapData.vecTerrainObject.end(); ++it)
	{
		m_pTerrainObject->Insert(**it);
	}
}

bool MapManager::CanMoveStraightly(Vector3 start, Vector3 dest)
{
	if (!IsPositionValidToMove(dest))
	{
		return false;
	}

	Line2D line(Vector2(start.x, start.z), Vector2(dest.x, dest.z));
	Vector2 vec2Intersection;

	for (vector<MathematicalData::Polygon*>::iterator it = m_MapData.vecTerrainObject.begin(); it != m_MapData.vecTerrainObject.end(); ++it)
	{
		if (PhysicsHelper::Collision::FindLineAndPolygonIntersection(line, **it, vec2Intersection))
		{
			return false;
		}
	}

	return true;
}


bool MapManager::IsPositionValidToMove(Vector3 vec3Position)
{
	if (!(vec3Position.x < m_MapData.fWidth * 0.5f && vec3Position.x > m_MapData.fWidth * -0.5f && vec3Position.z < m_MapData.fHeight * 0.5f && vec3Position.z > m_MapData.fHeight * -0.5f))
	{
		return false;
	}

	for (vector<MathematicalData::Polygon*>::iterator it = m_MapData.vecTerrainObject.begin(); it != m_MapData.vecTerrainObject.end(); ++it)
	{
		if (PhysicsHelper::Collision::PointInConvexPolygon(vec3Position, **it))
		{
			return false;
		}
	}

	return true;
}