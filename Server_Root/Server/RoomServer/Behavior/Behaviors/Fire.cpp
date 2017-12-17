#include "stdafx.h"
#include "Fire.h"
#include "../../Entity/Entities/Projectile/Projectile.h"
#include "../BehaviorIDs.h"
#include "../../MasterData/MasterDataManager.h"
#include "../../MasterData/Behavior.h"
#include "../../Factory.h"
#include "../../Game/BaeGameRoom.h"
#include "../../GameEvent/GameEvents/EntityCreate.h"
#include "../../Util.h"
#include "../../State/StateIDs.h"

const string Fire::NAME = "Fire";

Fire::Fire(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID) : IBehavior(pGameRoom, pEntity, nMasterDataID)
{
}

Fire::~Fire()
{
}

void Fire::Start(long long lStartTime, ...)
{
	__super::Start(lStartTime);

	va_list ap;
	va_start(ap, lStartTime);
	m_pTarget = va_arg(ap, IEntity*);
	va_end(ap);
}

void Fire::Initialize()
{
	MasterData::Behavior* pMasterBehavior = NULL;
	MasterDataManager::Instance()->GetData<MasterData::Behavior>(m_nMasterDataID, pMasterBehavior);

	m_fLength = pMasterBehavior->m_fLength;
	m_strStringParams = pMasterBehavior->m_strStringParams;
	m_vecAction = pMasterBehavior->m_vecAction;
}

void Fire::UpdateBody(long long lUpdateTime)
{
	for (vector<MasterData::Behavior::Action>::iterator it = m_vecAction.begin(); it != m_vecAction.end(); ++it)
	{
		if ((m_fCurrentTime == 0 && it->m_fTime == 0) || (m_fPreviousTime < it->m_fTime && it->m_fTime <= m_fCurrentTime))
		{
			if (it->m_strID == "Project")
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
					Projectile* pProjectile = NULL;
					m_pGameRoom->CreateProjectile(nProjectileMasterDataID, &nEntityID, &pProjectile, m_pEntity->GetID());
					pProjectile->SetPosition(m_pEntity->GetPosition());

					GameEvent::EntityCreate* pEntityCreate = new GameEvent::EntityCreate();
					pEntityCreate->m_fEventTime = lUpdateTime / 1000.0f;
					pEntityCreate->m_nEntityID = nEntityID;
					pEntityCreate->m_nMasterDataID = nProjectileMasterDataID;
					pEntityCreate->m_EntityType = FBS::Data::EntityType_Projectile;
					pEntityCreate->m_vec3Position = pProjectile->GetPosition();
					pEntityCreate->m_vec3Rotation = pProjectile->GetRotation();

					m_pGameRoom->AddGameEvent(pEntityCreate);

					if (m_pEntity->GetEntityType() == FBS::Data::EntityType::EntityType_Character)
					{
						Character* pCharacter = (Character*)m_pEntity;
						if (pCharacter->GetRole() == Character::Role::Disturber)
						{
							IState* pState = Factory::Instance()->CreateState(m_pGameRoom, pProjectile, StateID::ChallengerDisturbing, lUpdateTime);
							pState->Initialize();
							pProjectile->AddState(pState, lUpdateTime);
							pState->Update(lUpdateTime);
						}
					}

					float fTargetAngle = 0;
					if (m_pTarget == NULL)
					{
						fTargetAngle = m_pEntity->GetRotation().y();
					}
					else
					{
						fTargetAngle = Util::GetAngle_Y(m_pTarget->GetPosition() - m_pEntity->GetPosition());
					}

					btVector3 vec3Dest = Util::GetAngledPosition(m_pEntity->GetPosition(), fTargetAngle + *itDirection, 20/*temp..*/);
					pProjectile->GetBehavior(BehaviorID::MOVE)->Start(lUpdateTime, &vec3Dest);
					pProjectile->GetBehavior(BehaviorID::MOVE)->Update(lUpdateTime);
				}
			}
			else if (it->m_strID == "Fire")
			{
				btVector3 vec3Offset(atof(it->m_vecParams[0].c_str()), atof(it->m_vecParams[1].c_str()), atof(it->m_vecParams[2].c_str()));
				btVector3 vec3Range(atof(it->m_vecParams[3].c_str()), atof(it->m_vecParams[4].c_str()), atof(it->m_vecParams[5].c_str()));
				int nTypes = m_pEntity->GetAttackTargetTypes();
				list<CollisionObject*> objects;
				btVector3 vec3Position = Util::GetAngledPosition(m_pEntity->GetPosition() + vec3Offset, m_pEntity->GetRotation().y(), vec3Range.z() * 0.5f);

				if (m_pGameRoom->GetCollisionObjectsInRange(vec3Position, m_pEntity->GetRotation(), vec3Range, nTypes, &objects))
				{
					for (list<CollisionObject*>::iterator iterCollisionObject = objects.begin(); iterCollisionObject != objects.end(); ++iterCollisionObject)
					{
						CollisionObject* pCollisionObject = *iterCollisionObject;
						int nTargetEntityID = m_pGameRoom->GetEntityIDByCollisionObjectID(pCollisionObject->GetID());

						if (nTargetEntityID == m_pEntity->GetID())
							continue;

						m_pGameRoom->EntityAttack(m_pEntity->GetID(), nTargetEntityID, 1, lUpdateTime);
					}
				}
			}
		}
	}

	if (m_fLength != -1 && m_fCurrentTime >= m_fLength)
	{
		Stop(lUpdateTime - (m_fCurrentTime - m_fLength) * 1000);
	}
}