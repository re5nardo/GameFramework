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
	CharacterStatus(FBS::Data::CharacterStatus status)
	{
		m_nMaximumHP = status.MaximumHP();
		m_nHP = status.HP();
		m_nMaximumMP = status.MaximumMP();
		m_nMP = status.MP();
		m_fMaximumSpeed = status.MaximumSpeed();
		m_fSpeed = status.Speed();
		m_fMPChargeRate = status.MPChargeRate();
		m_fMovePoint = status.MovePoint();
	};
	CharacterStatus(int nMaximumHP, int nHP, int nMaximumMP, int nMP, float fMaximumSpeed, float fSpeed, float fMPChargeRate, float fMovePoint)
	{
		m_nMaximumHP = nMaximumHP;
		m_nHP = nHP;
		m_nMaximumMP = nMaximumMP;
		m_nMP = nMP;
		m_fMaximumSpeed = fMaximumSpeed;
		m_fSpeed = fSpeed;
		m_fMPChargeRate = fMPChargeRate;
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
	float m_fMovePoint;
};