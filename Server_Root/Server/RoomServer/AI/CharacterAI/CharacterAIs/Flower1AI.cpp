#include "stdafx.h"
#include "Flower1AI.h"
#include "../../../GameEvent/GameEvents/EntityCreate.h"
#include "../../../Entity/Entities/Character/Character.h"
#include "../../../Game/BaeGameRoom.h"

Flower1AI::Flower1AI(BaeGameRoom* pGameRoom, int nMasterDataID, long long lStartTime) : ICharacterAI(pGameRoom, nMasterDataID, lStartTime)
{
}

Flower1AI::~Flower1AI()
{
}

float spawntime = 5;
float interval = 2;
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
		m_pGameRoom->CreateEntity(FBS::Data::EntityType_Character, m_nMasterDataID, NULL, (IEntity**)&m_pCharacter);
		m_pCharacter->SetPosition(m_vec3Position);
		m_pCharacter->SetRotation(m_vec3Rotation);

		GameEvent::EntityCreate* pEntityCreate = new GameEvent::EntityCreate();
		pEntityCreate->m_fEventTime = lUpdateTime / 1000.0f;
		pEntityCreate->m_nEntityID = m_pCharacter->GetID();
		pEntityCreate->m_nMasterDataID = m_nMasterDataID;
		pEntityCreate->m_EntityType = FBS::Data::EntityType_Character;
		pEntityCreate->m_vec3Position = m_pCharacter->GetPosition();

		m_pGameRoom->AddGameEvent(pEntityCreate);
	}
	else if (m_fCurrentTime > spawntime && m_fPreviousTime < fFireTime && fFireTime <= m_fCurrentTime)
	{
		m_pCharacter->GetBehavior(6)->Start(lUpdateTime);
		m_pCharacter->GetBehavior(6)->Update(lUpdateTime);
	}
}

void Flower1AI::SetData(btVector3& vec3StartPosition, btVector3& vec3StartRotation)
{
	m_vec3Position = vec3StartPosition;
	m_vec3Rotation = vec3StartRotation;
}
