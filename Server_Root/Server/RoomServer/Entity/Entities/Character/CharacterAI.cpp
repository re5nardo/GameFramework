#include "stdafx.h"
#include "CharacterAI.h"
#include "../../../MasterData/MasterDataManager.h"
#include "../../../Factory.h"

CharacterAI::CharacterAI(BaeGameRoom* pGameRoom, int nID, int nMasterDataID)
{
	m_pGameRoom = pGameRoom;
	m_nID = nID;
	m_nMasterDataID = nMasterDataID;
}

CharacterAI::~CharacterAI()
{
}

void CharacterAI::Initialize()
{
	
}

void CharacterAI::Update(long long lUpdateTime)
{
	
}

Character* CharacterAI::GetCharacter()
{
	return m_pCharacter;
}

