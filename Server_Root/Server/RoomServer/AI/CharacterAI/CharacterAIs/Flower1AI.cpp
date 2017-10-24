#include "stdafx.h"
#include "Flower1AI.h"
#include "../../../GameEvent/GameEvents/EntityCreate.h"
#include "../../../Entity/Entities/Character/Character.h"
#include "../../../Game/BaeGameRoom.h"
#include "../../../Factory.h"
#include "../../../State/StateIDs.h"

Flower1AI::Flower1AI(BaeGameRoom* pGameRoom, int nMasterDataID, long long lStartTime) : ICharacterAI(pGameRoom, nMasterDataID, lStartTime)
{
}

Flower1AI::~Flower1AI()
{
}

float spawntime = 5;
float interval = 3;
void Flower1AI::UpdateBody(long long lUpdateTime)
{
	float fFireTime = 0;
	int cur = (int)m_fCurrentTime;
	if (cur % (int)interval == 0)
	{
		fFireTime = cur;
	}
	else
	{
		float f = std::fmod(m_fCurrentTime, interval);
		fFireTime = m_fCurrentTime - f;
	}

	if (m_fPreviousTime < spawntime && spawntime <= m_fCurrentTime)
	{
		m_pGameRoom->CreateCharacter(m_nMasterDataID, NULL, &m_pCharacter, Character::Role::Disturber);
		m_pCharacter->SetPosition(m_vec3Position);
		m_pCharacter->SetRotation(m_vec3Rotation);

		GameEvent::EntityCreate* pEntityCreate = new GameEvent::EntityCreate();
		pEntityCreate->m_fEventTime = lUpdateTime / 1000.0f;
		pEntityCreate->m_nEntityID = m_pCharacter->GetID();
		pEntityCreate->m_nMasterDataID = m_nMasterDataID;
		pEntityCreate->m_EntityType = FBS::Data::EntityType_Character;
		pEntityCreate->m_vec3Position = m_pCharacter->GetPosition();

		m_pGameRoom->AddGameEvent(pEntityCreate);

		IState* pState = Factory::Instance()->CreateState(m_pGameRoom, m_pCharacter, StateID::ChallengerDisturbing, lUpdateTime);
		pState->Initialize();
		m_pCharacter->AddState(pState, lUpdateTime);
		pState->Update(lUpdateTime);
	}
	else if (m_pCharacter != NULL && m_fPreviousTime < fFireTime && fFireTime <= m_fCurrentTime)
	{
		list<pair<int, btVector3>> listItem;
		int nTypes = CollisionObject::Type::CollisionObjectType_Character_Challenger;

		if (!m_pCharacter->GetBehavior(6)->IsActivated() && m_pGameRoom->CehckExistInRange(m_pCharacter->GetID(), 30, nTypes, &listItem))
		{
			m_pCharacter->GetBehavior(6)->Start(lUpdateTime);
			m_pCharacter->GetBehavior(6)->Update(lUpdateTime);
		}
	}
}

void Flower1AI::SetData(btVector3& vec3StartPosition, btVector3& vec3StartRotation)
{
	m_vec3Position = vec3StartPosition;
	m_vec3Rotation = vec3StartRotation;
}
