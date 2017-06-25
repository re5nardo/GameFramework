#include "stdafx.h"
#include "AddState.h"
#include "../../Entity/IEntity.h"
#include "../BehaviorIDs.h"
#include "../../MasterData/MasterDataManager.h"
#include "../../MasterData/Behavior.h"
#include "../../Util.h"
#include "../../Factory.h"

const string AddState::NAME = "AddState";

AddState::AddState(IEntity* pEntity, int nMasterDataID) : IBehavior(pEntity, nMasterDataID)
{
}

AddState::~AddState()
{
}

void AddState::Start(__int64 lStartTime, ...)
{
	__super::Start(lStartTime);

	if (m_pEntity->GetBehavior(BehaviorID::IDLE)->IsActivated())
		m_pEntity->GetBehavior(BehaviorID::IDLE)->Stop();
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

void AddState::Update(__int64 lUpdateTime)
{
	if (!m_bActivated || (m_lLastUpdateTime == lUpdateTime))
		return;

	float fLast = 0, fCur = 0;
	if (m_lStartTime != lUpdateTime)
	{
		fLast = (m_lLastUpdateTime - m_lStartTime) / 1000.0f;
		fCur = (lUpdateTime - m_lStartTime) / 1000.0f;
	}

	if ((fCur == 0 && m_fTime == 0) || (fLast < m_fTime && m_fTime <= fCur))
	{
		IState* pState = Factory::Instance()->CreateState(m_pEntity, m_nStateID, lUpdateTime);
		pState->Initialize();
		m_pEntity->AddState(pState);
		pState->Update(lUpdateTime);
	}

	if (fCur >= m_fLength)
	{
		m_bActivated = false;
	}

	m_lLastUpdateTime = lUpdateTime;
}