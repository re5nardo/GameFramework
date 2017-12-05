#pragma once

#include "IMasterData.h"

namespace MasterData
{
	class CharacterSpeedVariation : public IMasterData
	{
	public:
		CharacterSpeedVariation();
		virtual ~CharacterSpeedVariation();

	public:
		int m_nTargetCharacterID;	//	Target Character MasterDataID 
		float m_fTouchAcceleration;
		float m_fTouchAccelerationEndPoint;
		float m_fTouchDecelerationStartPoint;
		float m_fTouchDuration;

	public:
		void SetData(Sheet* pSheet, int nRow) override;
	};
}