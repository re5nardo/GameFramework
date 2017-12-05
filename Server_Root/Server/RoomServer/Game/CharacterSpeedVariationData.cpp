#include "stdafx.h"
#include "CharacterSpeedVariationData.h"
#include "../MasterData/MasterDataManager.h"
#include "../Util.h"

CharacterSpeedVariationData::CharacterSpeedVariationData()
{
}

CharacterSpeedVariationData::CharacterSpeedVariationData(int nCharacterMasterDataID)
{
	MasterData::CharacterSpeedVariation* pMasterData = NULL;
	MasterDataManager::Instance()->GetData<MasterData::CharacterSpeedVariation>(nCharacterMasterDataID, pMasterData);

	m_MasterData = *pMasterData;
}

CharacterSpeedVariationData::~CharacterSpeedVariationData()
{
}

float CharacterSpeedVariationData::GetLastTouchDurationEndTime()
{
	return m_fLastTouchTime + m_MasterData.m_fTouchDuration;
}

float CharacterSpeedVariationData::GetLastTouchNormalizedPoint(float fTime)
{
	if (fTime < m_fLastTouchTime)
	{
		return 0;
	}
	else if (fTime > GetLastTouchDurationEndTime())
	{
		return 1;
	}
	else
	{
		return (fTime - m_fLastTouchTime) / m_MasterData.m_fTouchDuration;
	}
}

float CharacterSpeedVariationData::GetTouchAccelerationValue()
{
	return m_MasterData.m_fTouchAcceleration;
}

float CharacterSpeedVariationData::GetTouchDuration()
{
	return m_MasterData.m_fTouchDuration;
}

float CharacterSpeedVariationData::GetLastTouchAccelerationEndTime()
{
	return m_fLastTouchTime + m_MasterData.m_fTouchDuration * m_MasterData.m_fTouchAccelerationEndPoint;
}

float CharacterSpeedVariationData::GetLastTouchDecelerationStartTime()
{
	return m_fLastTouchTime + m_MasterData.m_fTouchDuration * m_MasterData.m_fTouchDecelerationStartPoint;
}

//if (fTime <= pCharacterSpeedVariationData->GetLastTouchAccelerationEndTime())
//{
//	pCharacter->SetMoveSpeed(Util::Lerp(pCharacterSpeedVariationData->m_fStartSpeed, pCharacterSpeedVariationData->m_fTargetSpeed, (fTime - pCharacterSpeedVariationData->m_fLastTouchTime) / pCharacterSpeedVariationData->GetTouchDuration() / 0.3f));	//    터치 초기에만 속도 exp하게 올라가도록 수정 (linear해도 괜찮을 듯)
//}
//else if ((fTime - pCharacterSpeedVariationData->m_fLastTouchTime) / pCharacterSpeedVariationData->GetTouchDuration() > 0.6f)
//{
//	pCharacter->SetMoveSpeed(Util::Lerp(pCharacterSpeedVariationData->m_fTargetSpeed, 0, ((fTime - pCharacterSpeedVariationData->m_fLastTouchTime) / pCharacterSpeedVariationData->GetTouchDuration() - 0.6f) / 0.4f));    //    linear -> exp하게 수정?
//}


float CharacterSpeedVariationData::CalculateSpeed(float fTime)
{
	float point = GetLastTouchNormalizedPoint(fTime);

	if (point <= m_MasterData.m_fTouchAccelerationEndPoint)
	{
		return Util::Lerp(m_fStartSpeed, m_fTargetSpeed, point / m_MasterData.m_fTouchAccelerationEndPoint);
	}
	else if (point >= m_MasterData.m_fTouchDecelerationStartPoint)
	{
		return Util::Lerp(m_fTargetSpeed, 0, (point - m_MasterData.m_fTouchDecelerationStartPoint) / (1 - m_MasterData.m_fTouchDecelerationStartPoint));
	}
	else
	{
		return m_fTargetSpeed;
	}
}