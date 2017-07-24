#include "stdafx.h"
#include "AddState.h"
#include "../../Entity/IEntity.h"
#include "../BehaviorIDs.h"
#include "../../MasterData/MasterDataManager.h"
#include "../../MasterData/Behavior.h"
#include "../../Util.h"
#include "../../Factory.h"
#include "../../GameEvent/GameEvents/BehaviorEnd.h"
#include "../../Game/BaeGameRoom.h"

const string AddState::NAME = "AddState";

AddState::AddState(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID) : IBehavior(pGameRoom, pEntity, nMasterDataID)
{
}

AddState::~AddState()
{
}

void AddState::Start(__int64 lStartTime, ...)
{
	__super::Start(lStartTime);

	if (m_pEntity->GetBehavior(BehaviorID::IDLE)->IsActivated())
		m_pEntity->GetBehavior(BehaviorID::IDLE)->Stop(lStartTime);
}

void AddState::Initialize()
{
	MasterData::Behavior* pMasterBehavior = NULL;
	MasterDataManager::Instance()->GetData<MasterData::Behavior>(m_nMasterDataID, pMasterBehavior);

	m_fLength = pMasterBehavior->m_fLength;
	vector<string> vecText;
	Util::Parse(pMasterBehavior->m_strStringParams, ':', &vecText);
	m_nStateID = atoi(vecText[0].c_str());
	m_fTime = atof(vecText[1].c_str());
}

void AddState::UpdateBody(__int64 lUpdateTime)
{
	if ((m_fCurrentTime == 0 && m_fTime == 0) || (m_fPreviousTime < m_fTime && m_fTime <= m_fCurrentTime))
	{
		IState* pState = Factory::Instance()->CreateState(m_pGameRoom, m_pEntity, m_nStateID, lUpdateTime);
		pState->Initialize();
		m_pEntity->AddState(pState);
		pState->Update(lUpdateTime);
	}

	if (m_fCurrentTime >= m_fLength)
	{
		Stop(lUpdateTime - (m_fCurrentTime - m_fLength) * 1000);
	}
}