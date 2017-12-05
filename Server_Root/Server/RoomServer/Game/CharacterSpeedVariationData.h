#pragma once

#include "../MasterData/CharacterSpeedVariation.h"

class CharacterSpeedVariationData
{
public:
	CharacterSpeedVariationData();
	CharacterSpeedVariationData(int nCharacterMasterDataID);
	virtual ~CharacterSpeedVariationData();

private:
	MasterData::CharacterSpeedVariation m_MasterData;

public:
	float m_fLastTouchTime = -1;
	float m_fStartSpeed;
	float m_fTargetSpeed;

public:
	float GetTouchAccelerationValue();
	float GetTouchDuration();
	float GetLastTouchAccelerationEndTime();
	float GetLastTouchDecelerationStartTime();
	float GetLastTouchDurationEndTime();
	float GetLastTouchNormalizedPoint(float fTime);
	float CalculateSpeed(float fTime);
};