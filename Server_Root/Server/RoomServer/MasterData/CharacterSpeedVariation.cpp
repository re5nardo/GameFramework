#include "stdafx.h"
#include "CharacterSpeedVariation.h"

namespace MasterData
{
	CharacterSpeedVariation::CharacterSpeedVariation()
	{
	}

	CharacterSpeedVariation::~CharacterSpeedVariation()
	{
	}

	void CharacterSpeedVariation::SetData(vector<string> data)
	{
		m_nID = atoi(data[0].c_str());
		m_nTargetCharacterID = atoi(data[1].c_str());
		m_fTouchAcceleration = atof(data[2].c_str());
		m_fTouchAccelerationEndPoint = atof(data[3].c_str());
		m_fTouchDecelerationStartPoint = atof(data[4].c_str());
		m_fTouchDuration = atof(data[5].c_str());
	}
}