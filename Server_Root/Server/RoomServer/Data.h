#pragma once

#include <vector>
#include "btBulletCollisionCommon.h"

using namespace std;

struct MapData
{
public:
	int nID = 0;
	float fWidth = 0.0f;
	float fHeight = 0.0f;
	vector<btCollisionShape> vecTerrainObjectShape;
};

struct Stat
{
public:
	Stat() {};
	Stat(float fMoveSpeed, int nHP, int nMP)
	{
		m_fMoveSpeed = fMoveSpeed;
		m_nHP = nHP;
		m_nMP = nMP;
	};

public:
	float m_fMoveSpeed = 0.0f;
	int m_nHP = 0;					//	Health Point
	int m_nMP = 0;					//	Mana Point
};