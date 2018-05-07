#include "stdafx.h"
#include "Character.h"
#include "../Util.h"

namespace MasterData
{
	Character::Character()
	{
	}

	Character::~Character()
	{
	}

	void Character::SetData(vector<string> data)
	{
		m_nID = atoi(data[0].c_str());
		m_strName = data[1];
		m_strClassName = data[2];
		Util::Parse(data[3], ',', &m_vecSkillID);
		Util::Parse(data[4], ',', &m_vecBehaviorID);
		m_nHP = atoi(data[5].c_str());
		m_nMP = atoi(data[6].c_str());
		m_fMaximumSpeed = atof(data[7].c_str());
		m_fMPChargeRate = atof(data[8].c_str());
		m_nJumpCount = atoi(data[9].c_str());
		m_fJumpRegenerationTime = atof(data[10].c_str());
		m_fSize = atof(data[11].c_str());
		m_fHeight = atof(data[13].c_str());
		m_fDefault_Y = atof(data[14].c_str());
		m_nDefaultBehaviorID = atoi(data[15].c_str());
	}
}