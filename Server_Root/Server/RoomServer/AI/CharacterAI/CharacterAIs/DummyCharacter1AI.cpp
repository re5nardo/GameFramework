#include "stdafx.h"
#include "DummyCharacter1AI.h"
#include "../../../GameEvent/GameEvents/EntityCreate.h"
#include "../../../Entity/Entities/Character/Character.h"
#include "../../../Game/BaeGameRoom.h"
#include "../../../Behavior/BehaviorIDs.h"
#include "../../../Util.h"

DummyCharacter1AI::DummyCharacter1AI(BaeGameRoom* pGameRoom, int nMasterDataID, long long lStartTime) : ICharacterAI(pGameRoom, nMasterDataID, lStartTime)
{
}

DummyCharacter1AI::~DummyCharacter1AI()
{
}

void DummyCharacter1AI::UpdateBody(long long lUpdateTime)
{
	float spawntime = 5;

	if (m_fPreviousTime < spawntime && spawntime <= m_fCurrentTime)
	{
		m_pGameRoom->CreateCharacter(m_nMasterDataID, NULL, &m_pCharacter, Character::Role::Disturber);
		m_pCharacter->SetPosition(m_vec3StartPosition);
		m_pCharacter->SetRotation(m_vec3StartRotation);

		GameEvent::EntityCreate* pEntityCreate = new GameEvent::EntityCreate();
		pEntityCreate->m_fEventTime = lUpdateTime / 1000.0f;
		pEntityCreate->m_nEntityID = m_pCharacter->GetID();
		pEntityCreate->m_nMasterDataID = m_nMasterDataID;
		pEntityCreate->m_EntityType = FBS::Data::EntityType_Character;
		pEntityCreate->m_vec3Position = m_pCharacter->GetPosition();

		m_pGameRoom->AddGameEvent(pEntityCreate);
	}
	
	if (m_pCharacter != NULL)
	{
		list<pair<int, btVector3>> listItem;
		int nTypes = CollisionObject::Type::CollisionObjectType_Character;

		if (!m_pCharacter->GetBehavior(BehaviorID::MOVE)->IsActivated() && m_pGameRoom->CehckExistInRange(m_pCharacter->GetID(), 10, nTypes, &listItem))
		{
			if (m_pCharacter->GetPosition() == m_vec3DestPosition)
			{
				m_pCharacter->GetBehavior(BehaviorID::MOVE)->Start(lUpdateTime, &m_vec3StartPosition);
			}
			else
			{
				m_pCharacter->GetBehavior(BehaviorID::MOVE)->Start(lUpdateTime, &m_vec3DestPosition);
			}
		}
	}
}

void DummyCharacter1AI::SetData(btVector3& vec3StartPosition, btVector3& vec3StartRotation, btVector3& vec3DestPosition)
{
	m_vec3StartPosition = vec3StartPosition;
	m_vec3StartRotation = vec3StartRotation;
	m_vec3DestPosition = vec3DestPosition;
}
