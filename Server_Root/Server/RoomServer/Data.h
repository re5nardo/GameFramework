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

struct CharacterStat
{
public:
	CharacterStat() {};
	CharacterStat(int nHP, int nMP, float fMPChargeRate, float fRunSpeed, float fDashSpeed, float fStrength)
	{
		m_nHP = nHP;
		m_nMP = nMP;
		m_fMPChargeRate = fMPChargeRate;
		m_fRunSpeed = fRunSpeed;
		m_fDashSpeed = fDashSpeed;
		m_fStrength = fStrength;
	};

public:
	int m_nHP;
	int m_nMP;
	float m_fMPChargeRate;
	float m_fRunSpeed;
	float m_fDashSpeed;
	float m_fStrength;
};