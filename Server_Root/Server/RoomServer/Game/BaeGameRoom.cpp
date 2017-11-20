#include "stdafx.h"
#include "BaeGameRoom.h"
#include "../../CommonSources/Network/Network.h"
#include "../../CommonSources/Message/IMessage.h"
#include "../../RoomServer/RoomMessageHeader.h"
//#include "RoomMessageHeader.h"
#include <process.h>
#include "../Entity/Entities/Character/Character.h"
#include "../Entity/Entities/Projectile/Projectile.h"
#include "../Entity/Entities/Item/Item.h"
#include "../AI/CharacterAI/CharacterAIs/Flower1AI.h"
#include "../AI/CharacterAI/CharacterAIs/DummyCharacter1AI.h"
#include "../AI/CharacterAI/CharacterAIs/DummyCharacter2AI.h"
#include "../Messages/ToClient/WorldSnapShotToC.h"
#include "../Messages/ToClient/WorldInfoToC.h"
#include "../Behavior/BehaviorIDs.h"
#include "../../CommonSources/tinyxml2.h"
#include "../../CommonSources/QuadTree.h"
#include "../Util.h"
#include "btBulletCollisionCommon.h"
#include "../Factory.h"
#include <cstdlib>
#include "../Entity/IEntity.h"
#include "../GameEvent/GameEvents/EntityCreate.h"
#include "../GameEvent/GameEvents/EntityDestroy.h"
#include "../../FBSFiles/FBSData_generated.h"
#include "../Behavior/Behaviors/Dash.h"
#include "../GameEvent/GameEvents/Position.h"
#include "../State/States/DashState.h"

BaeGameRoom::BaeGameRoom(int nMatchID, vector<string> vecMatchedPlayerKey)	//	Receive Game Info from Lobby
{
	m_nMatchID = nMatchID;
	m_vecMatchedPlayerKey = vecMatchedPlayerKey;

	for (int i = 0; i < vecMatchedPlayerKey.size(); ++i)
	{
		m_mapPlayerIndexPreparationState[i] = 0.0f;
	}

	m_bPlaying = false;

	LoadMap(2/*temp..*/);
}

BaeGameRoom::~BaeGameRoom()
{
}


void BaeGameRoom::Loop()
{
	//	must create game worker thread cuz lock delay..
	//(네트워크 쓰레드로 게임 로직 돌리면 게임 룸 락 때문에 네트워크 쓰레드가 대기하게 되고 네트워크 요청을 원활하게 수행할 수 없다.)

	//	Tick Process
	while (m_bPlaying)
	{
		UpdateTime();

		ProcessInput();

		//	Update world
		Update();
		LateUpdate();

		SendWorldInfo();

		long long lTickProcessTime = GetElapsedTime() - m_lLastUpdateTime;

		//	Wait tick interval
		if (TIME_STEP > lTickProcessTime)
			Sleep(TIME_STEP - lTickProcessTime);

		m_nTick++;
	}
}

void BaeGameRoom::UpdateTime()
{
	if (m_nTick == 0)
	{
		m_StartTime = system_clock::now();
		m_lDeltaTime = 0;
		m_lLastUpdateTime = 0;
		m_lLastItemSpawnTime = 0;
	}
	else
	{
		long long lElapsedTime = GetElapsedTime();

		m_lDeltaTime = lElapsedTime - m_lLastUpdateTime;
		m_lLastUpdateTime = lElapsedTime;
	}
}

void BaeGameRoom::ProcessInput()
{
	m_LockPlayerInput.lock();

	vector<pair<int, pair<long long, IMessage*>>> vecPlayerInput;
	for (map<int, list<pair<long long, IMessage*>>>::iterator it = m_mapPlayerInput.begin(); it != m_mapPlayerInput.end(); ++it)
	{
		for (list<pair<long long, IMessage*>>::iterator iter = it->second.begin(); iter != it->second.end(); ++iter)
		{
			vecPlayerInput.push_back(make_pair(it->first, *iter));
		}
	}
	m_mapPlayerInput.clear();

	auto cmp = [](pair<int, pair<long long, IMessage*>> a, pair<int, pair<long long, IMessage*>> b)
	{
		return a.second.first < b.second.first;
	};
	sort(vecPlayerInput.begin(), vecPlayerInput.end(), cmp);

	for (vector<pair<int, pair<long long, IMessage*>>>::iterator it = vecPlayerInput.begin(); it != vecPlayerInput.end(); ++it)
	{
		int nPlayerIndex = it->first;
		long long lTime = it->second.first;
		IMessage* pPlayerInputMsg = it->second.second;
		Character* pCharacter = (Character*)m_mapEntity[m_mapPlayerEntity[nPlayerIndex]];

		if (pPlayerInputMsg->GetID() == GameEventRunToR_ID)
		{
			if (!pCharacter->IsAlive())
				continue;

			GameEventRunToR* pRunToR = (GameEventRunToR*)pPlayerInputMsg;

			if (pRunToR->m_vec3Dest != pCharacter->GetPosition())
			{
				if (pCharacter->GetState(StateID::DashState) != NULL)
				{
					pCharacter->GetBehavior(BehaviorID::DASH)->Start(lTime, &pRunToR->m_vec3Dest);
				}
				else
				{
					pCharacter->GetBehavior(BehaviorID::MOVE)->Start(lTime, &pRunToR->m_vec3Dest);
				}
			}
		}
		else if (pPlayerInputMsg->GetID() == GameEventDashToR_ID)
		{
			if (!pCharacter->IsAlive())
				continue;

			GameEventDashToR* pMsg = (GameEventDashToR*)pPlayerInputMsg;

			IState* pState = pCharacter->GetState(StateID::DashState);
			if (pState != NULL)
			{
				((DashState*)pState)->Prolong(lTime);
			}
			else
			{
				IState* pState = Factory::Instance()->CreateState(this, pCharacter, StateID::DashState, lTime);
				pState->Initialize();
				pCharacter->AddState(pState, lTime);
				pState->Update(lTime);
			}
		}
		else if (pPlayerInputMsg->GetID() == GameInputSkillToR_ID)
		{
			if (!pCharacter->IsAlive())
				continue;

			GameInputSkillToR* pMsg = (GameInputSkillToR*)pPlayerInputMsg;

			list<ISkill*> listSkill = pCharacter->GetAllSkills();
			for (list<ISkill*>::iterator it = listSkill.begin(); it != listSkill.end(); ++it)
			{
				if (pMsg->m_nSkillID == (*it)->GetMasterDataID())
					(*it)->ProcessInput(lTime, this, pMsg);
			}
		}
	}

	for (vector<pair<int, pair<long long, IMessage*>>>::iterator it = vecPlayerInput.begin(); it != vecPlayerInput.end(); ++it)
	{
		delete it->second.second;
	}
	vecPlayerInput.clear();

	m_LockPlayerInput.unlock();
}

void BaeGameRoom::Update()
{
	//	temp.. start position
	if (m_nTick == 0)
	{
		for (map<int, int>::iterator it = m_mapPlayerEntity.begin(); it != m_mapPlayerEntity.end(); ++it)
		{
			IEntity* pEntity = m_mapEntity[it->second];

			srand(0);
			btVector3 vec3Start(-18 + (rand() % 36), 0, -240 + (rand() % 10));

			pEntity->SetPosition(vec3Start);

			AddPositionGameEvent(m_lLastUpdateTime / 1000.0f, pEntity->GetID(), m_lLastUpdateTime / 1000.0f, m_lLastUpdateTime / 1000.0f, vec3Start, vec3Start);
		}
	}

	//	Item
	if (m_lLastUpdateTime - m_lLastItemSpawnTime >= ITEM_SPAWN_INTERVAL)
	{
		int nEntityID = 0;
		int nItemMasterDataID = rand() % 3;
		Item* pItem = NULL;
		CreateItem(nItemMasterDataID, &nEntityID, &pItem, m_lLastUpdateTime);

		btVector3 vec3Pos = GetEntity(0)->GetPosition();
		btVector3 vec3Rand = Util::GetRandomePosition(rand(), 10);

		pItem->SetPosition(btVector3(vec3Pos.x() + vec3Rand.x(), 0, vec3Pos.z() + vec3Rand.z()));

		GameEvent::EntityCreate* pEntityCreate = new GameEvent::EntityCreate();
		pEntityCreate->m_fEventTime = m_lLastUpdateTime / 1000.0f;
		pEntityCreate->m_nEntityID = nEntityID;
		pEntityCreate->m_nMasterDataID = nItemMasterDataID;
		pEntityCreate->m_EntityType = FBS::Data::EntityType_Item;
		pEntityCreate->m_vec3Position = pItem->GetPosition();

		AddGameEvent(pEntityCreate);

		m_lLastItemSpawnTime = m_lLastUpdateTime;
	}

	//	AI
	for (list<ICharacterAI*>::iterator it = m_listDisturber.begin(); it != m_listDisturber.end(); ++it)
	{
		(*it)->Update(m_lLastUpdateTime);
	}

	//	Skills
	TrimEntity();
	for (map<int, IEntity*>::iterator it = m_mapEntity.begin(); it != m_mapEntity.end(); ++it)
	{
		if (it->second->GetEntityType() == FBS::Data::EntityType::EntityType_Character)
		{
			((Character*)it->second)->UpdateSkills(m_lLastUpdateTime);
		}
	}

	//	States
	TrimEntity();
	for (map<int, IEntity*>::iterator it = m_mapEntity.begin(); it != m_mapEntity.end(); ++it)
	{
		it->second->UpdateStates(m_lLastUpdateTime);
	}

	//	Behaviors
	TrimEntity();
	for (map<int, IEntity*>::iterator it = m_mapEntity.begin(); it != m_mapEntity.end(); ++it)
	{
		it->second->UpdateBehaviors(m_lLastUpdateTime);
	}
}

void BaeGameRoom::LateUpdate()
{
	TrimEntity();

	for (map<int, IEntity*>::iterator it = m_mapEntity.begin(); it != m_mapEntity.end(); ++it)
	{
		it->second->LateUpdate(m_lLastUpdateTime);
	}

	TrimEntity();
}

void BaeGameRoom::SendWorldSnapShot()
{
	WorldSnapShotToC* pWorldSnapShotToC = new WorldSnapShotToC();

	pWorldSnapShotToC->m_nTick = m_nTick;
	pWorldSnapShotToC->m_fTime = m_lLastUpdateTime / 1000.0f;

	for (map<int, IEntity*>::iterator it = m_mapEntity.begin(); it != m_mapEntity.end(); ++it)
	{
		pWorldSnapShotToC->m_mapEntity[it->first] = it->second;
	}

	SendToAllUsers(pWorldSnapShotToC);
}

void BaeGameRoom::SendWorldInfo()
{
	WorldInfoToC* pWorldInfoToC = new WorldInfoToC();

	pWorldInfoToC->m_nTick = m_nTick;
	pWorldInfoToC->m_fStartTime = (m_lLastUpdateTime - m_lDeltaTime) / 1000.0f;
	pWorldInfoToC->m_fEndTime = m_lLastUpdateTime / 1000.0f;

	for (list<IGameEvent*>::iterator it = m_listGameEvent.begin(); it != m_listGameEvent.end(); ++it)
	{
		pWorldInfoToC->m_listGameEvent.push_back(*it);
	}

	SendToAllUsers(pWorldInfoToC);

	for (list<IGameEvent*>::iterator it = m_listGameEvent.begin(); it != m_listGameEvent.end(); ++it)
	{
		delete *it;
	}
	m_listGameEvent.clear();
}

void BaeGameRoom::StartGame()
{
	m_bPlaying = true;
	m_nTick = 0;

	SetObstacles(2, 0/*temp..*/);

	GameStartToC* gameStartToC = new GameStartToC();
	SendToAllUsers(gameStartToC);

	_beginthreadex(NULL, 0, LoopThreadStart, this, 0, NULL);
}

void BaeGameRoom::EndGame()
{
	Reset();
}

void BaeGameRoom::Reset()
{
	m_nEntitySequence = 0;
	m_nTick = 0;
	m_lDeltaTime = 0;
	m_lLastUpdateTime = 0;
	m_lLastItemSpawnTime = 0;

	m_bPlaying = false;

	for (map<int, list<pair<long long, IMessage*>>>::iterator it = m_mapPlayerInput.begin(); it != m_mapPlayerInput.end(); ++it)
	{
		for (list<pair<long long, IMessage*>>::iterator iter = it->second.begin(); iter != it->second.end(); ++iter)
		{
			delete iter->second;
		}
	}
	m_mapPlayerInput.clear();

	for (map<int, IEntity*>::iterator it = m_mapEntity.begin(); it != m_mapEntity.end(); ++it)
	{
		delete it->second;
	}
	m_mapEntity.clear();

	for (list<ICharacterAI*>::iterator it = m_listDisturber.begin(); it != m_listDisturber.end(); ++it)
	{
		delete *it;
	}
	m_listDisturber.clear();

	m_CollisionManager.Reset();
	m_mapEntityCollision.clear();
	m_mapCollisionEntity.clear();
	m_mapPlayerEntity.clear();
	m_mapEntityPlayer.clear();
}

void BaeGameRoom::LoadMap(int nID)
{
	stringstream mapFileName;
	mapFileName << "map_" << to_string(nID) << ".xml";

	tinyxml2::XMLDocument doc;
	doc.LoadFile(mapFileName.str().c_str());

	tinyxml2::XMLElement* pMap = doc.FirstChildElement("Map");

	float fHalfWidth = atof(pMap->FirstChildElement("Width")->GetText());
	float fHalfHeight = atof(pMap->FirstChildElement("Height")->GetText());

	m_CollisionManager.Init(btVector3(fHalfWidth, 0, fHalfHeight));

	tinyxml2::XMLElement* pTerrainObjectsElement = pMap->FirstChildElement("TerrainObjects");
	for (tinyxml2::XMLElement* pTerrainObjectElement = pTerrainObjectsElement->FirstChildElement("TerrainObject"); pTerrainObjectElement; pTerrainObjectElement = pTerrainObjectElement->NextSiblingElement("TerrainObject"))
	{
		vector<string> vecText;
		Util::Parse(pTerrainObjectElement->GetText(), '/', &vecText);

		if (vecText[0] == "Box2d")
		{
			btVector3 vec3Position = Util::StringToVector3(vecText[1]);
			btVector3 vec3Rotation = Util::StringToVector3(vecText[2]);
			btVector3 vec3HalfExtents = Util::StringToVector3(vecText[3]);

			m_CollisionManager.AddBox2dShapeTerrainObject(vec3Position, vec3Rotation, vec3HalfExtents);
		}
		else if (vecText[0] == "Sphere2d")
		{
			btVector3 vec3Position = Util::StringToVector3(vecText[1]);
			float fRadius = atof(vecText[2].c_str());

			m_CollisionManager.AddSphere2dShapeTerrainObject(vec3Position, fRadius);
		}
		else if (vecText[0] == "ConvexPolygon2d")
		{
			btVector3 vec3Position = Util::StringToVector3(vecText[1]);
			list<btVector3> listPoint;
			for (int i = 2; i < vecText.size(); ++i)
			{
				listPoint.push_back(Util::StringToVector3(vecText[i]));
			}

			m_CollisionManager.AddConvexPolygon2dShapeTerrainObject(vec3Position, listPoint);
		}
	}
}

//temp..
void BaeGameRoom::SetObstacles(int nMapID, int nRandomSeed)
{
	srand(nRandomSeed);

	float z = -240;
	while (true)
	{
		z += (5 + (rand() % 30));

		if (z > 250)
		{
			break;
		}

		btVector3 vec3Start(-15 + (rand() % 30), 0, z);
		btVector3 vec3Dest(-15 + (rand() % 30), 0, z);

		while (vec3Start == vec3Dest)
		{
			vec3Start = btVector3(-15 + (rand() % 30), 0, z);
			vec3Dest = btVector3(-15 + (rand() % 30), 0, z);
		}

		DummyCharacter1AI* dummy1 = new DummyCharacter1AI(this, 2, 0);
		dummy1->SetData(vec3Start, btVector3(0, 0, 0), vec3Dest);
		m_listDisturber.push_back(dummy1);
	}

	//	flower1
	z = -240;
	while (true)
	{
		z += (5 + (rand() % 30));

		if (z > 250)
		{
			break;
		}

		btVector3 vec3Start(-15 + (rand() % 30), 0, z);

		Flower1AI* flower1AI = new Flower1AI(this, 1, 0);
		flower1AI->SetData(vec3Start, btVector3(0, 0, 0));
		m_listDisturber.push_back(flower1AI);
	}
}

//	Target	(Other부터 처리하도록 순서 수정할까..? (이동하려는 Entity가 공격권을 가지도록 -> 먼저 공격처리가 이뤄지도록)
void BaeGameRoom::EntityMove(int nEntityID, IBehavior* pBehavior, btVector3& vec3To, int nTypes, long long lStartTime, long long lEndTime)
{
	IEntity* pTarget = GetEntity(nEntityID);
	btVector3 vec3Start = pTarget->GetPosition();

	list<CollisionObject*> listCollisionObject;
	if (GetCollisionObjectsInRange(nEntityID, vec3To, nTypes, &listCollisionObject))
	{
		btVector3 vec3Hit;
		for (list<CollisionObject*>::iterator it = listCollisionObject.begin(); it != listCollisionObject.end(); ++it)
		{
			CollisionObject* pOtherCollisionObject = *it;

			if (ContinuousCollisionDectection(nEntityID, pOtherCollisionObject, vec3To, vec3Hit))
			{
				if (pOtherCollisionObject->GetCollisionObjectType() == CollisionObject::Type::CollisionObjectType_Terrain)
				{
					if (pTarget->IsTerrainPassable())
					{
						continue;
					}

					pBehavior->Stop(lEndTime);
				}
				else if (pOtherCollisionObject->GetCollisionObjectType() == CollisionObject::Type::CollisionObjectType_Character)
				{
					Character* pOtherCharacter = (Character*)pOtherCollisionObject->GetbtCollisionObject()->getUserPointer();

					pTarget->OnCollision(pOtherCharacter, lEndTime);
					pOtherCharacter->OnCollision(pTarget, lEndTime);
				}
				else if (pOtherCollisionObject->GetCollisionObjectType() == CollisionObject::Type::CollisionObjectType_Projectile)
				{
					Projectile* pOtherProjectile = (Projectile*)pOtherCollisionObject->GetbtCollisionObject()->getUserPointer();

					pTarget->OnCollision(pOtherProjectile, lEndTime);
					pOtherProjectile->OnCollision(pTarget, lEndTime);
				}
				
				if (!pBehavior->IsActivated())
				{
					pTarget->SetPosition(vec3Hit);

					AddPositionGameEvent(lStartTime / 1000.0f, pTarget->GetID(), lStartTime / 1000.0f, lEndTime / 1000.0f, vec3Start, vec3Hit);

					return;
				}
			}
		}
	}
	
	pTarget->SetPosition(vec3To);

	AddPositionGameEvent(lStartTime / 1000.0f, pTarget->GetID(), lStartTime / 1000.0f, lEndTime / 1000.0f, vec3Start, vec3To);
}

void BaeGameRoom::EntityAttack(int nAttackingEntityID, int nAttackedEntityID, int nDamage, long long lTime)
{
	Character* pAttackedCharacter = (Character*)m_mapEntity[nAttackedEntityID];

	pAttackedCharacter->OnAttacked(nAttackingEntityID, nDamage, lTime);
}

void BaeGameRoom::CharacterDieEnd(int nCharacterID, long long lTime)
{
	Character* pCharacter = (Character*)m_mapEntity[nCharacterID];

	pCharacter->OnRespawn(lTime);
}

bool BaeGameRoom::ContinuousCollisionDectection(int nTargetID, CollisionObject* pOther, btVector3& vec3To, btVector3& vec3Hit)
{
	return m_CollisionManager.ContinuousCollisionDectection(GetCollisionObjectIDByEntityID(nTargetID), pOther->GetID(), vec3To, vec3Hit);
}

bool BaeGameRoom::DiscreteCollisionDectection(int nTargetID, int nOtherID, btVector3& vec3Hit)
{
	return m_CollisionManager.DiscreteCollisionDectection(GetCollisionObjectIDByEntityID(nTargetID), GetCollisionObjectIDByEntityID(nOtherID), vec3Hit);
}

bool BaeGameRoom::GetCollisionObjectsInRange(int nTargetID, btVector3& vec3To, int nTypes, list<CollisionObject*>* pObjects)
{
	return m_CollisionManager.GetCollisionObjectsInRange(GetCollisionObjectIDByEntityID(nTargetID), vec3To, nTypes, pObjects);
}

bool BaeGameRoom::CheckDiscreteCollisionDectection(int nEntityID, int nTypes, list<pair<int, btVector3>>* listHit)
{
	return m_CollisionManager.DiscreteCollisionDectection(GetCollisionObjectIDByEntityID(nEntityID), nTypes, listHit);
}

bool BaeGameRoom::CheckContinuousCollisionDectectionFirst(int nEntityID, btVector3& vec3Dest, int nTypes, pair<int, btVector3>* hit)
{
	return m_CollisionManager.ContinuousCollisionDectectionFirst(GetCollisionObjectIDByEntityID(nEntityID), vec3Dest, nTypes, hit);
}

bool BaeGameRoom::CheckContinuousCollisionDectection(int nEntityID, btVector3& vec3Dest, int nTypes, list<pair<int, btVector3>>* listHit)
{
	return m_CollisionManager.ContinuousCollisionDectection(GetCollisionObjectIDByEntityID(nEntityID), vec3Dest, nTypes, listHit);
}

bool BaeGameRoom::CehckExistInRange(btVector3& vec3Center, float fRadius, int nTypes, list<pair<int, btVector3>>* listItem)
{
	return m_CollisionManager.CehckExistInRange(vec3Center, fRadius, nTypes, listItem);
}

bool BaeGameRoom::CehckExistInRange(int nEntityID, float fRadius, int nTypes, list<pair<int, btVector3>>* listItem)
{
	return m_CollisionManager.CehckExistInRange(GetCollisionObjectIDByEntityID(nEntityID), fRadius, nTypes, listItem);
}

bool BaeGameRoom::IsChallenger(int nEntityID)
{
	return m_mapEntityPlayer.count(nEntityID) > 0;
}

bool BaeGameRoom::IsDisturber(int nEntityID)
{
	return false;
}

void BaeGameRoom::SetCollisionObjectPosition(int nCollisionObjectID, btVector3& vec3Position)
{
	m_CollisionManager.SetPosition(nCollisionObjectID, vec3Position);
}

void BaeGameRoom::SetCollisionObjectRotation(int nCollisionObjectID, btVector3& vec3Rotation)
{
	m_CollisionManager.SetRotation(nCollisionObjectID, vec3Rotation);
}

void BaeGameRoom::AddGameEvent(IGameEvent* pGameEvent)
{
	m_listGameEvent.push_back(pGameEvent);
}

void BaeGameRoom::AddPositionGameEvent(float fEventTime, int nEntityID, float fStartTime, float fEndTime, btVector3& vec3StartPosition, btVector3& vec3EndPosition)
{
	GameEvent::Position* pPosition = new GameEvent::Position();
	pPosition->m_fEventTime = fEventTime;
	pPosition->m_nEntityID = nEntityID;
	pPosition->m_fStartTime = fStartTime;
	pPosition->m_fEndTime = fEndTime;
	pPosition->m_vec3StartPosition = vec3StartPosition;
	pPosition->m_vec3EndPosition = vec3EndPosition;

	AddGameEvent(pPosition);
}

bool BaeGameRoom::CreateCharacter(int nMasterDataID, int* pEntityID, Character** pCharacter, Character::Role role)
{
	m_LockEntitySequence.lock();
	int nEntityID = m_nEntitySequence++;
	m_LockEntitySequence.unlock();

	Character* pNewCharacter = Factory::Instance()->CreateCharacter(this, nEntityID, nMasterDataID, role);
	pNewCharacter->Initialize();
	m_mapEntity[nEntityID] = pNewCharacter;

	//	collision
	int nCollisionObjectID = m_CollisionManager.AddCharacter(pNewCharacter->GetPosition(), pNewCharacter->GetSize(), pNewCharacter->GetHeight(), pNewCharacter);
	m_mapEntityCollision[nEntityID] = nCollisionObjectID;
	m_mapCollisionEntity[nCollisionObjectID] = nEntityID;

	if (pEntityID != NULL)
		*pEntityID = nEntityID;

	if (pCharacter != NULL)
		*pCharacter = pNewCharacter;

	return true;
}

bool BaeGameRoom::CreateProjectile(int nMasterDataID, int* pEntityID, Projectile** pProjectile, int nCreatorID)
{
	m_LockEntitySequence.lock();
	int nEntityID = m_nEntitySequence++;
	m_LockEntitySequence.unlock();

	Projectile* pNewProjectile = Factory::Instance()->CreateProjectile(this, nCreatorID, nEntityID, nMasterDataID);
	pNewProjectile->Initialize();
	m_mapEntity[nEntityID] = pNewProjectile;

	//	collision
	int nCollisionObjectID = m_CollisionManager.AddProjectile(pNewProjectile->GetPosition(), pNewProjectile->GetSize(), pNewProjectile->GetHeight(), pNewProjectile);
	m_mapEntityCollision[nEntityID] = nCollisionObjectID;
	m_mapCollisionEntity[nCollisionObjectID] = nEntityID;

	if (pEntityID != NULL)
		*pEntityID = nEntityID;

	if (pProjectile != NULL)
		*pProjectile = pNewProjectile;

	return true;
}

bool BaeGameRoom::CreateItem(int nMasterDataID, int* pEntityID, Item** pItem, long long lTime)
{
	m_LockEntitySequence.lock();
	int nEntityID = m_nEntitySequence++;
	m_LockEntitySequence.unlock();

	Item* pNewItem = Factory::Instance()->CreateItem(this, lTime, nEntityID, nMasterDataID);
	pNewItem->Initialize();
	m_mapEntity[nEntityID] = pNewItem;

	//	collision
	int nCollisionObjectID = m_CollisionManager.AddItem(pNewItem->GetPosition(), pNewItem->GetSize(), pNewItem->GetHeight());
	m_mapEntityCollision[nEntityID] = nCollisionObjectID;
	m_mapCollisionEntity[nCollisionObjectID] = nEntityID;

	if (pEntityID != NULL)
		*pEntityID = nEntityID;

	if (pItem != NULL)
		*pItem = pNewItem;

	return true;
}

void BaeGameRoom::DestroyEntity(int nEntityID)
{
	GameEvent::EntityDestroy* pEntityDestroy = new GameEvent::EntityDestroy();
	pEntityDestroy->m_fEventTime = m_lLastUpdateTime / 1000.0f;
	pEntityDestroy->m_nEntityID = nEntityID;

	AddGameEvent(pEntityDestroy);

	m_mapEntity[nEntityID]->m_bDestroyReserved = true;
	
	m_listDestroyReserved.push_back(nEntityID);
}

void BaeGameRoom::TrimEntity()
{
	for (list<int>::iterator it = m_listDestroyReserved.begin(); it != m_listDestroyReserved.end(); ++it)
	{
		int nEntityID = *it;

		delete m_mapEntity[nEntityID];
		m_mapEntity.erase(nEntityID);

		int nCollisionObjectID = GetCollisionObjectIDByEntityID(nEntityID);
		m_CollisionManager.RemoveCollisionObject(nCollisionObjectID);

		m_mapEntityCollision.erase(nEntityID);
		m_mapCollisionEntity.erase(nCollisionObjectID);
	}
	m_listDestroyReserved.clear();
}

void BaeGameRoom::SendToAllUsers(IMessage* pMsg, string strExclusionKey, bool bDelete)
{
	for (map<string, unsigned int>::iterator it = m_mapPlayerKeySocket.begin(); it != m_mapPlayerKeySocket.end(); ++it)
	{
		if (it->first != strExclusionKey)
			Network::Instance()->Send(m_mapPlayerKeySocket[it->first], pMsg, false);
	}

	if (bDelete)
		delete pMsg;
}

void BaeGameRoom::OnRecvMessage(unsigned int socket, IMessage* pMsg)
{
	if (pMsg->GetID() == EnterRoomToR::MESSAGE_ID)
	{
		OnEnterRoomToR((EnterRoomToR*)pMsg, socket);
	}
	else if (pMsg->GetID() == PreparationStateToR::MESSAGE_ID)
	{
		OnPreparationStateToR((PreparationStateToR*)pMsg, socket);
	}
	else if (pMsg->GetID() == GameEventRunToR::MESSAGE_ID)
	{
		OnGameEventRunToR((GameEventRunToR*)pMsg, socket);
	}
	else if (pMsg->GetID() == GameEventStopToR::MESSAGE_ID)
	{
		OnGameEventStopToR((GameEventStopToR*)pMsg, socket);
	}
	else if (pMsg->GetID() == GameInputSkillToR::MESSAGE_ID)
	{
		OnGameInputSkillToR((GameInputSkillToR*)pMsg, socket);
	}
	else if (pMsg->GetID() == GameEventDashToR::MESSAGE_ID)
	{
		OnGameEventDashToR((GameEventDashToR*)pMsg, socket);
	}
}

void BaeGameRoom::OnGameEventRunToR(GameEventRunToR* pMsg, unsigned int socket)
{
	m_LockPlayerInput.lock();

	if (m_mapPlayerInput.count(pMsg->m_nPlayerIndex) > 0)
	{
		list<pair<long long, IMessage*>>* pInputs = &m_mapPlayerInput[pMsg->m_nPlayerIndex];

		list<pair<long long, IMessage*>>::iterator found = find_if(pInputs->begin(), pInputs->end(), [&](pair<long long, IMessage*>& item) { return item.second->GetID() == pMsg->GetID(); });

		if (found != pInputs->end())
		{
			delete found->second;

			found->first = GetElapsedTime();
			found->second = pMsg->Clone();
		}
		else
		{
			pInputs->push_back(make_pair(GetElapsedTime(), pMsg->Clone()));
		}
	}
	else
	{
		m_mapPlayerInput[pMsg->m_nPlayerIndex].push_back(make_pair(GetElapsedTime(), pMsg->Clone()));
	}

	m_LockPlayerInput.unlock();
}

void BaeGameRoom::OnGameEventStopToR(GameEventStopToR* pMsg, unsigned int socket)
{
	m_LockPlayerInput.lock();

	if (m_mapPlayerInput.count(pMsg->m_nPlayerIndex) > 0)
	{
		list<pair<long long, IMessage*>>* pInputs = &m_mapPlayerInput[pMsg->m_nPlayerIndex];

		list<pair<long long, IMessage*>>::iterator found = find_if(pInputs->begin(), pInputs->end(), [&](pair<long long, IMessage*>& item) { return item.second->GetID() == pMsg->GetID(); });

		if (found != pInputs->end())
		{
			delete found->second;

			found->first = GetElapsedTime();
			found->second = pMsg->Clone();
		}
		else
		{
			pInputs->push_back(make_pair(GetElapsedTime(), pMsg->Clone()));
		}
	}
	else
	{
		m_mapPlayerInput[pMsg->m_nPlayerIndex].push_back(make_pair(GetElapsedTime(), pMsg->Clone()));
	}

	m_LockPlayerInput.unlock();
}

void BaeGameRoom::OnEnterRoomToR(EnterRoomToR* pMsg, unsigned int socket)
{
	EnterRoomToC* enterRoomToC = new EnterRoomToC();
	
	if (IsValidPlayer(pMsg->m_strPlayerKey))
	{
		int nPlayerIndex = GetPlayerIndexByPlayerKey(pMsg->m_strPlayerKey);

		m_mapPlayerKeySocket[pMsg->m_strPlayerKey] = socket;
		m_mapSocketPlayerKey[socket] = pMsg->m_strPlayerKey;

		//	Temp..0 is MisterBae
		int nEntityID = 0;
		CreateCharacter(0, &nEntityID, NULL, Character::Role::Challenger);

		m_mapPlayerEntity[nPlayerIndex] = nEntityID;
		m_mapEntityPlayer[nEntityID] = nPlayerIndex;

		enterRoomToC->m_nResult = 0;
		enterRoomToC->m_nPlayerIndex = nPlayerIndex;
		enterRoomToC->m_nPlayerEntityID = nEntityID;
		for (int i = 0; i < m_vecMatchedPlayerKey.size(); ++i)
		{
			enterRoomToC->m_mapPlayers[i] = m_vecMatchedPlayerKey[i];
		}

		Network::Instance()->Send(socket, enterRoomToC);

		/*PlayerEnterRoomToC* playerEnterRoomToC = new PlayerEnterRoomToC();

		playerEnterRoomToC->m_nPlayerIndex = nPlayerIndex;
		playerEnterRoomToC->m_strCharacterID = "Test";

		SendToAllUsers(playerEnterRoomToC, pMsg->m_strPlayerKey);*/
	}
	else
	{
		enterRoomToC->m_nResult = -1;

		Network::Instance()->Send(socket, enterRoomToC);
	}
}

void BaeGameRoom::OnPreparationStateToR(PreparationStateToR* pMsg, unsigned int socket)
{
	int nPlayerIndex = GetPlayerIndexBySocket(socket);

	PreparationStateToC* preparationStateToC = new PreparationStateToC();

	preparationStateToC->m_nPlayerIndex = nPlayerIndex;
	preparationStateToC->m_fState = pMsg->m_fState;

	SendToAllUsers(preparationStateToC);

	m_mapPlayerIndexPreparationState[nPlayerIndex] = pMsg->m_fState;

	if (IsAllPlayersReady())
	{
		StartGame();
	}
}

void BaeGameRoom::OnGameInputSkillToR(GameInputSkillToR* pMsg, unsigned int socket)
{
	m_LockPlayerInput.lock();

	if (m_mapPlayerInput.count(pMsg->m_nPlayerIndex) > 0)
	{
		list<pair<long long, IMessage*>>* pInputs = &m_mapPlayerInput[pMsg->m_nPlayerIndex];

		list<pair<long long, IMessage*>>::iterator found = find_if(pInputs->begin(), pInputs->end(), [&](pair<long long, IMessage*>& item) { return item.second->GetID() == pMsg->GetID(); });

		if (found != pInputs->end())
		{
			delete found->second;

			found->first = GetElapsedTime();
			found->second = pMsg->Clone();
		}
		else
		{
			pInputs->push_back(make_pair(GetElapsedTime(), pMsg->Clone()));
		}
	}
	else
	{
		m_mapPlayerInput[pMsg->m_nPlayerIndex].push_back(make_pair(GetElapsedTime(), pMsg->Clone()));
	}

	m_LockPlayerInput.unlock();
}

void BaeGameRoom::OnGameEventDashToR(GameEventDashToR* pMsg, unsigned int socket)
{
	m_LockPlayerInput.lock();

	if (m_mapPlayerInput.count(pMsg->m_nPlayerIndex) > 0)
	{
		list<pair<long long, IMessage*>>* pInputs = &m_mapPlayerInput[pMsg->m_nPlayerIndex];

		list<pair<long long, IMessage*>>::iterator found = find_if(pInputs->begin(), pInputs->end(), [&](pair<long long, IMessage*>& item) { return item.second->GetID() == pMsg->GetID(); });

		if (found != pInputs->end())
		{
			delete found->second;

			found->first = GetElapsedTime();
			found->second = pMsg->Clone();
		}
		else
		{
			pInputs->push_back(make_pair(GetElapsedTime(), pMsg->Clone()));
		}
	}
	else
	{
		m_mapPlayerInput[pMsg->m_nPlayerIndex].push_back(make_pair(GetElapsedTime(), pMsg->Clone()));
	}

	m_LockPlayerInput.unlock();
}

int BaeGameRoom::GetPlayerIndexByPlayerKey(string strPlayerKey)
{
	for (int i = 0; i < m_vecMatchedPlayerKey.size(); ++i)
	{
		if (m_vecMatchedPlayerKey[i] == strPlayerKey)
			return i;
	}

	return -1;
}

int BaeGameRoom::GetPlayerIndexBySocket(unsigned int socket)
{
	return GetPlayerIndexByPlayerKey(m_mapSocketPlayerKey[socket]);
}

int BaeGameRoom::GetEntityIDByCollisionObjectID(int nCollisionObjectID)
{
	return m_mapCollisionEntity[nCollisionObjectID];
}

int BaeGameRoom::GetCollisionObjectIDByEntityID(int nEntityID)
{
	return m_mapEntityCollision[nEntityID];
}

IEntity* BaeGameRoom::GetEntity(int nEntityID)
{
	return m_mapEntity[nEntityID];
}

bool BaeGameRoom::IsValidPlayer(string strPlayerKey)
{
	//if (pMsg->m_nMatchID != m_nMatchID || m_vecMatchedPlayers.~_Container_base12(pMsg->m_strPlayerKey))

	//	Temp
	return true;
}

bool BaeGameRoom::IsAllPlayersReady()
{
	for (map<int, float>::iterator it = m_mapPlayerIndexPreparationState.begin(); it != m_mapPlayerIndexPreparationState.end(); ++it)
	{
		if (it->second < 1.0f)
			return false;
	}

	return true;
}

long long BaeGameRoom::GetElapsedTime()
{
	milliseconds elapsedTime = duration_cast<milliseconds>(system_clock::now() - m_StartTime);

	return elapsedTime.count();
}

long long BaeGameRoom::GetLastUpdateTime()
{
	return m_lLastUpdateTime;
}

unsigned int __stdcall BaeGameRoom::LoopThreadStart(void* param)
{
	BaeGameRoom* pRoom = (BaeGameRoom*)param;

	pRoom->Loop();

	return 0;
}