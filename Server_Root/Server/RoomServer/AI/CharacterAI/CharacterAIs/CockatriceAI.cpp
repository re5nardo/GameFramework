#include "stdafx.h"
#include "CockatriceAI.h"
#include "../../../Entity/Entities/Character/Character.h"
#include "../../../Game/BaeGameRoom.h"
#include "../../../MasterData/MasterDataManager.h"
#include "../../../MasterData/Character.h"
#include "../../../GameEvent/GameEvents/EntityCreate.h"
#include "../../../GameEvent/GameEvents/Rotation.h"
#include "../../../Factory.h"
#include "../../../State/StateIDs.h"
#include "../../../Util.h"

CockatriceAI::CockatriceAI(BaeGameRoom* pGameRoom, int nMasterDataID, long long lStartTime) : ICharacterAI(pGameRoom, nMasterDataID, lStartTime)
{
}

CockatriceAI::~CockatriceAI()
{
}

void CockatriceAI::UpdateBody(long long lUpdateTime)
{
	float spawntime = 1;
	float cooltime = 5;

	if (m_fPreviousTime < spawntime && spawntime <= m_fCurrentTime)
	{
		MasterData::Character* pMasterCharacter = NULL;
		MasterDataManager::Instance()->GetData<MasterData::Character>(m_nMasterDataID, pMasterCharacter);

		CharacterStatus status(pMasterCharacter->m_nHP, pMasterCharacter->m_nHP, pMasterCharacter->m_nMP, pMasterCharacter->m_nMP, pMasterCharacter->m_fMaximumSpeed, 0, pMasterCharacter->m_fMPChargeRate, 0);

		m_pGameRoom->CreateCharacter(m_nMasterDataID, NULL, &m_pCharacter, Character::Role::Disturber, status);
		m_pCharacter->SetPosition(m_vec3Position);
		m_pCharacter->SetRotation(m_vec3Rotation);
		m_pCharacter->SetMoveSpeed(0);

		GameEvent::EntityCreate* pEntityCreate = new GameEvent::EntityCreate();
		pEntityCreate->m_fEventTime = lUpdateTime / 1000.0f;
		pEntityCreate->m_nEntityID = m_pCharacter->GetID();
		pEntityCreate->m_nMasterDataID = m_nMasterDataID;
		pEntityCreate->m_EntityType = FBS::Data::EntityType_Character;
		pEntityCreate->m_vec3Position = m_pCharacter->GetPosition();
		pEntityCreate->m_vec3Rotation = m_pCharacter->GetRotation();

		m_pGameRoom->AddGameEvent(pEntityCreate);

		IState* pState = Factory::Instance()->CreateState(m_pGameRoom, m_pCharacter, StateID::ChallengerDisturbing, lUpdateTime);
		pState->Initialize();
		m_pCharacter->AddState(pState, lUpdateTime);
		pState->Update(lUpdateTime);
	}

	if (m_pCharacter != NULL)
	{
		list<pair<int, btVector3>> listItem;
		int nTypes = CollisionObject::Type::CollisionObjectType_Character;
		int nSparkID = 10;

		if (!m_pCharacter->GetBehavior(nSparkID)->IsActivated() && m_pGameRoom->CehckExistInRange(m_pCharacter->GetID(), 30, nTypes, &listItem) && current <= 0 )
		{
			IEntity* pTarget = m_pGameRoom->GetEntityByCollisionObjectID(listItem.front().first);

			float y = Util::GetAngle_Y(pTarget->GetPosition() - m_pCharacter->GetPosition());

			btVector3 curRot = m_pCharacter->GetRotation();
			m_pCharacter->SetRotation(btVector3(curRot.x(), y, curRot.z()));

			GameEvent::Rotation* pRotation = new GameEvent::Rotation();
			pRotation->m_fEventTime = lUpdateTime / 1000.0f;
			pRotation->m_nEntityID = m_pCharacter->GetID();
			pRotation->m_fStartTime = lUpdateTime / 1000.0f;
			pRotation->m_fEndTime = lUpdateTime / 1000.0f;
			pRotation->m_vec3StartRotation = m_pCharacter->GetRotation();
			pRotation->m_vec3EndRotation = m_pCharacter->GetRotation();

			m_pGameRoom->AddGameEvent(pRotation);

			m_pCharacter->GetBehavior(nSparkID)->Start(lUpdateTime, pTarget);
			m_pCharacter->GetBehavior(nSparkID)->Update(lUpdateTime);

			current = cooltime;
		}
		else
		{
			current -= m_fDeltaTime;
		}
	}
}

void CockatriceAI::SetData(btVector3& vec3StartPosition, btVector3& vec3StartRotation)
{
	m_vec3Position = vec3StartPosition;
	m_vec3Rotation = vec3StartRotation;
}
