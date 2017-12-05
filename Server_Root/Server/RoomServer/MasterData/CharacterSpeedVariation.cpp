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

	void CharacterSpeedVariation::SetData(Sheet* pSheet, int nRow)
	{
		m_nID = pSheet->readNum(nRow, 0);
		m_nTargetCharacterID = pSheet->readNum(nRow, 1);
		m_fTouchAcceleration = pSheet->readNum(nRow, 2);
		m_fTouchAccelerationEndPoint = pSheet->readNum(nRow, 3);
		m_fTouchDecelerationStartPoint = pSheet->readNum(nRow, 4);
		m_fTouchDuration = pSheet->readNum(nRow, 5);
	}
}