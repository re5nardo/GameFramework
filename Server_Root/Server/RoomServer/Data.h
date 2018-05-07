#pragma once

#include <vector>
#include "btBulletCollisionCommon.h"
#include "../FBSFiles/FBSData_generated.h"

using namespace std;

struct MapData
{
public:
	int nID = 0;
	float fWidth = 0.0f;
	float fHeight = 0.0f;
	vector<btCollisionShape> vecTerrainObjectShape;
};

struct CharacterStatus
{
public:
	CharacterStatus() {};
	CharacterStatus(int nMaximumHP, int nHP, int nMaximumMP, int nMP, float fMaximumSpeed, float fSpeed, float fMPChargeRate, int nMaximumJumpCount, int nJumpCount, float fJumpRegenerationTime, float fMovePoint)
	{
		m_nMaximumHP = nMaximumHP;
		m_nHP = nHP;
		m_nMaximumMP = nMaximumMP;
		m_nMP = nMP;
		m_fMaximumSpeed = fMaximumSpeed;
		m_fSpeed = fSpeed;
		m_fMPChargeRate = fMPChargeRate;
		m_nMaximumJumpCount = nMaximumJumpCount;
		m_nJumpCount = nJumpCount;
		m_fJumpRegenerationTime = fJumpRegenerationTime;
		m_fMovePoint = fMovePoint;
	};

public:
	int m_nMaximumHP;
	int m_nHP;
	int m_nMaximumMP;
	int m_nMP;
	float m_fMaximumSpeed;
	float m_fSpeed;
	float m_fMPChargeRate;
	int m_nMaximumJumpCount;
	int m_nJumpCount;
	float m_fJumpRegenerationTime;
	float m_fMovePoint;
};