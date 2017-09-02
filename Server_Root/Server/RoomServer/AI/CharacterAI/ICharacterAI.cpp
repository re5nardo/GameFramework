#include "stdafx.h"
#include "ICharacterAI.h"

ICharacterAI::ICharacterAI(BaeGameRoom* pGameRoom, int nMasterDataID, long long lStartTime)
{
	m_pGameRoom = pGameRoom;
	m_nMasterDataID = nMasterDataID;
	m_lStartTime = lStartTime;
	m_lLastUpdateTime = -1;
}

ICharacterAI::~ICharacterAI()
{
}

int ICharacterAI::GetMasterDataID()
{
	return m_nMasterDataID;
}

Character* ICharacterAI::GetCharacter()
{
	return m_pCharacter;
}

void ICharacterAI::Update(long long lUpdateTime)
{
	if (m_lLastUpdateTime == lUpdateTime)
		return;

	if (m_lLastUpdateTime == -1)
	{
		m_lLastUpdateTime = m_lStartTime;
	}

	m_fPreviousTime = (m_lLastUpdateTime - m_lStartTime) / 1000.0f;
	m_fCurrentTime = (lUpdateTime - m_lStartTime) / 1000.0f;
	m_fDeltaTime = m_fCurrentTime - m_fPreviousTime;

	UpdateBody(lUpdateTime);

	m_lLastUpdateTime = lUpdateTime;
}