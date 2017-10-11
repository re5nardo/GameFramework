#include "stdafx.h"
#include "ChallengerDisturbing.h"
#include "../../Entity/IEntity.h"
#include "../../MasterData/MasterDataManager.h"
#include "../../MasterData/State.h"
#include "../../Game/BaeGameRoom.h"

const string ChallengerDisturbing::NAME = "ChallengerDisturbing";

ChallengerDisturbing::ChallengerDisturbing(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID, long long lStartTime) : IState(pGameRoom, pEntity, nMasterDataID, lStartTime)
{
}

ChallengerDisturbing::~ChallengerDisturbing()
{
}

int ChallengerDisturbing::GetID()
{
	return STATE_ID;
}

void ChallengerDisturbing::Initialize()
{
	MasterData::State* pMasterState = NULL;
	MasterDataManager::Instance()->GetData<MasterData::State>(m_nMasterDataID, pMasterState);

	m_fLength = pMasterState->m_fLength;

	for (vector<string>::iterator it = pMasterState->m_vecCoreState.begin(); it != pMasterState->m_vecCoreState.end(); ++it)
	{
		if ((*it) == "Invincible")
		{
			m_vecCoreState.push_back(CoreState::CoreState_Invincible);
		}
		else if ((*it) == "ChallengerDisturbing")
		{
			m_vecCoreState.push_back(CoreState::CoreState_ChallengerDisturbing);
		}
	}
}

void ChallengerDisturbing::UpdateBody(long long lUpdateTime)
{
	if (m_fLength != -1 && m_fCurrentTime >= m_fLength)
	{
		m_pEntity->RemoveState(m_nMasterDataID, lUpdateTime - (m_fCurrentTime - m_fLength) * 1000);
	}
}

void ChallengerDisturbing::OnCollision(IEntity* pOther, long long lTime)
{
	if (pOther->GetEntityType() == FBS::Data::EntityType::EntityType_Character)
	{
		Character* pOtherCharacter = (Character*)pOther;

		if (pOtherCharacter->GetRole() == Character::Role::Challenger)
		{
			m_pGameRoom->EntityAttack(m_pEntity->GetID(), pOther->GetID(), 1, lTime);
		}
	}
}