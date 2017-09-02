#include "stdafx.h"
#include "General.h"
#include "../../Entity/IEntity.h"
#include "../BehaviorIDs.h"
#include "../../MasterData/MasterDataManager.h"
#include "../../MasterData/Behavior.h"
#include "../../Factory.h"
#include "../../Game/BaeGameRoom.h"
#include "../../GameEvent/GameEvents/EntityCreate.h"
#include "../../GameEvent/GameEvents/EntityDestroy.h"
#include "../../Util.h"

const string General::NAME = "General";

General::General(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID) : IBehavior(pGameRoom, pEntity, nMasterDataID)
{
}

General::~General()
{
}

void General::Start(long long lStartTime, ...)
{
	__super::Start(lStartTime);

	if (m_bStopIdle && m_pEntity->GetBehavior(BehaviorID::IDLE)->IsActivated())
		m_pEntity->GetBehavior(BehaviorID::IDLE)->Stop(lStartTime);
}

void General::Initialize()
{
	MasterData::Behavior* pMasterBehavior = NULL;
	MasterDataManager::Instance()->GetData<MasterData::Behavior>(m_nMasterDataID, pMasterBehavior);

	m_fLength = pMasterBehavior->m_fLength;
	m_strStringParams = pMasterBehavior->m_strStringParams;
	m_vecAction = pMasterBehavior->m_vecAction;
	m_bStopIdle = pMasterBehavior->m_strName != "Idle";
}

void General::UpdateBody(long long lUpdateTime)
{
	for (vector<MasterData::Behavior::Action>::iterator it = m_vecAction.begin(); it != m_vecAction.end(); ++it)
	{
		if ((m_fCurrentTime == 0 && it->m_fTime == 0) || (m_fPreviousTime < it->m_fTime && it->m_fTime <= m_fCurrentTime))
		{
			if (it->m_strID == "Fire")
			{
				vector<float> vecDirection;
				vector<string> vecString;
				Util::Parse(m_strStringParams, ',', &vecString);
				for (vector<string>::iterator it = vecString.begin(); it != vecString.end(); ++it)
				{
					vecDirection.push_back(atof((*it).c_str()));
				}

				for (vector<float>::iterator itDirection = vecDirection.begin(); itDirection != vecDirection.end(); ++itDirection)
				{
					int nEntityID = 0;
					int nProjectileMasterDataID = atoi(it->m_vecParams[0].c_str());
					IEntity* pProjectile = NULL;
					m_pGameRoom->CreateEntity(FBS::Data::EntityType_Projectile, nProjectileMasterDataID, &nEntityID, &pProjectile);
					pProjectile->SetPosition(m_pEntity->GetPosition());

					GameEvent::EntityCreate* pEntityCreate = new GameEvent::EntityCreate();
					pEntityCreate->m_fEventTime = lUpdateTime / 1000.0f;
					pEntityCreate->m_nEntityID = nEntityID;
					pEntityCreate->m_nMasterDataID = nProjectileMasterDataID;
					pEntityCreate->m_EntityType = FBS::Data::EntityType_Projectile;
					pEntityCreate->m_vec3Position = pProjectile->GetPosition();

					m_pGameRoom->AddGameEvent(pEntityCreate);

					btVector3 vec3Dest = Util::GetAngledPosition(m_pEntity->GetPosition(), *itDirection, 10/*temp..*/);
					pProjectile->GetBehavior(BehaviorID::MOVE)->Start(lUpdateTime, &vec3Dest);
					pProjectile->GetBehavior(BehaviorID::MOVE)->Update(lUpdateTime);
				}
			}
			else if (it->m_strID == "AddState")
			{
				int nStateID = atoi(it->m_vecParams[0].c_str());
				
				IState* pState = Factory::Instance()->CreateState(m_pGameRoom, m_pEntity, nStateID, lUpdateTime);
				pState->Initialize();
				m_pEntity->AddState(pState);
				pState->Update(lUpdateTime);
			}
			else if (it->m_strID == "Destroy")
			{
				GameEvent::EntityDestroy* pEntityDestroy = new GameEvent::EntityDestroy();
				pEntityDestroy->m_nEntityID = m_pEntity->GetID();

				m_pGameRoom->AddGameEvent(pEntityDestroy);

				return;
			}
		}
	}

	if (m_fLength != -1 && m_fCurrentTime >= m_fLength)
	{
		Stop(lUpdateTime - (m_fCurrentTime - m_fLength) * 1000);
	}
}