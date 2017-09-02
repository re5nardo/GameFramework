#include "stdafx.h"
#include "BaeGameRoom.h"
#include "../../CommonSources/Network/Network.h"
#include "../../CommonSources/Message/IMessage.h"
#include "../../RoomServer/RoomMessageHeader.h"
//#include "RoomMessageHeader.h"
#include <process.h>
#include "../Entity/Entities/Character/Character.h"
#include "../AI/CharacterAI/CharacterAIs/Flower1AI.h"
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
#include "../Entity/Entities/Projectile/Projectile.h"
#include "../GameEvent/GameEvents/EntityDestroy.h"
#include "../../FBSFiles/FBSData_generated.h"

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
	for (map<int, pair<long long, IMessage*>>::iterator it = m_mapPlayerInput.begin(); it != m_mapPlayerInput.end(); ++it)
	{
		vecPlayerInput.push_back(make_pair(it->first, make_pair(it->second.first, it->second.second)));
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

		if (pPlayerInputMsg->GetID() == GameEventMoveToR_ID)
		{
			GameEventMoveToR* pMoveToR = (GameEventMoveToR*)pPlayerInputMsg;

			int nEntityID = m_mapPlayerEntity[nPlayerIndex];

			if (pMoveToR->m_vec3Dest != m_mapEntity[nEntityID]->GetPosition())
			{
				m_mapEntity[nEntityID]->GetBehavior(BehaviorID::MOVE)->Start(lTime, &pMoveToR->m_vec3Dest);
			}
		}
		else if (pPlayerInputMsg->GetID() == GameInputSkillToR_ID)
		{
			GameInputSkillToR* pMsg = (GameInputSkillToR*)pPlayerInputMsg;

			int nEntityID = m_mapPlayerEntity[nPlayerIndex];

			list<ISkill*> listSkill = ((Character*)m_mapEntity[nEntityID])->GetAllSkills();
			for (list<ISkill*>::iterator it = listSkill.begin(); it != listSkill.end(); ++it)
			{
				if (pMsg->m_nSkillID == (*it)->GetMasterDataID())
					(*it)->ProcessInput(lTime, this, pMsg);
			}
		}

		delete pPlayerInputMsg;
	}
	vecPlayerInput.clear();

	m_LockPlayerInput.unlock();
}

void BaeGameRoom::Update()
{
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
		IEntity* pEntity = it->second;

		if (!pEntity->IsBehavioring() && pEntity->GetBehavior(BehaviorID::IDLE) != NULL)
		{
			pEntity->GetBehavior(BehaviorID::IDLE)->Start(m_lLastUpdateTime);
		}
	}
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

	m_bPlaying = false;

	for (map<int, pair<long long, IMessage*>>::iterator it = m_mapPlayerInput.begin(); it != m_mapPlayerInput.end(); ++it)
	{
		delete it->second.second;
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

void BaeGameRoom::SetObstacles(int nMapID, int nRandomSeed)
{
	srand(nRandomSeed);

	//temp..
	int nDisturberCount = rand() % 10;
	nDisturberCount = 2;
	for (int i = 0; i < 1; ++i)
	{
		Flower1AI* ai = new Flower1AI(this, 1, 0);
		ai->SetData(btVector3(5, 0, 5), btVector3(0, 0, 0));
		m_listDisturber.push_back(ai);

		Flower1AI* ai2 = new Flower1AI(this, 1, 0);
		ai2->SetData(btVector3(-5, 0, 5), btVector3(0, 0, 0));
		m_listDisturber.push_back(ai2);

		Flower1AI* ai3 = new Flower1AI(this, 1, 0);
		ai3->SetData(btVector3(5, 0, 10), btVector3(0, 0, 0));
		m_listDisturber.push_back(ai3);

		Flower1AI* ai4 = new Flower1AI(this, 1, 0);
		ai4->SetData(btVector3(-5, 0, 10), btVector3(0, 0, 0));
		m_listDisturber.push_back(ai4);

		Flower1AI* ai5 = new Flower1AI(this, 1, 0);
		ai5->SetData(btVector3(5, 0, 15), btVector3(0, 0, 0));
		m_listDisturber.push_back(ai5);

		Flower1AI* ai6 = new Flower1AI(this, 1, 0);
		ai6->SetData(btVector3(-5, 0, 15), btVector3(0, 0, 0));
		m_listDisturber.push_back(ai6);
	}
}

bool BaeGameRoom::CheckDiscreteCollisionDectection(int nEntityID, int nTypes, list<pair<int, btVector3>>* listHit)
{
	int nCollisionObjectID = m_mapEntityCollision[nEntityID];

	return m_CollisionManager.DiscreteCollisionDectection(nCollisionObjectID, nTypes, listHit);
}

bool BaeGameRoom::CheckContinuousCollisionDectectionFirst(int nEntityID, btVector3& vec3Dest, int nTypes, pair<int, btVector3>* hit)
{
	int nCollisionObjectID = m_mapEntityCollision[nEntityID];

	return m_CollisionManager.ContinuousCollisionDectectionFirst(nCollisionObjectID, vec3Dest, nTypes, hit);
}

bool BaeGameRoom::CheckContinuousCollisionDectection(int nEntityID, btVector3& vec3Dest, int nTypes, list<pair<int, btVector3>>* listHit)
{
	int nCollisionObjectID = m_mapEntityCollision[nEntityID];

	return m_CollisionManager.ContinuousCollisionDectection(nCollisionObjectID, vec3Dest, nTypes, listHit);
}

bool BaeGameRoom::IsChallenger(int nEntityID)
{
	return m_mapEntityPlayer.count(nEntityID) > 0;
}

bool BaeGameRoom::IsDisturber(int nEntityID)
{
	return false;
}

void BaeGameRoom::SetCollisionObjectPosition(int nEntityID, btVector3& vec3Position)
{
	int nCollisionObjectID = m_mapEntityCollision[nEntityID];

	m_CollisionManager.SetPosition(nCollisionObjectID, vec3Position);
}

void BaeGameRoom::SetCollisionObjectRotation(int nEntityID, btVector3& vec3Rotation)
{
	int nCollisionObjectID = m_mapEntityCollision[nEntityID];

	m_CollisionManager.SetRotation(nCollisionObjectID, vec3Rotation);
}

void BaeGameRoom::AddGameEvent(IGameEvent* pGameEvent)
{
	m_listGameEvent.push_back(pGameEvent);
}

bool BaeGameRoom::CreateEntity(FBS::Data::EntityType type, int nMasterDataID, int* pEntityID, IEntity** pEntity)
{
	m_LockEntitySequence.lock();
	int nEntityID = m_nEntitySequence++;
	m_LockEntitySequence.unlock();

	IEntity* entity = NULL;
	if (type == FBS::Data::EntityType::EntityType_Character)
	{
		entity = Factory::Instance()->CreateCharacter(this, nEntityID, nMasterDataID);
	}
	else if (type == FBS::Data::EntityType::EntityType_Projectile)
	{
		entity = Factory::Instance()->CreateProjectile(this, nEntityID, nMasterDataID);
	}
	entity->Initialize();
	m_mapEntity[nEntityID] = entity;

	//	collision
	int nCollisionObjectID = 0;
	if (type == FBS::Data::EntityType::EntityType_Character)
	{
		nCollisionObjectID = m_CollisionManager.AddCharacter(entity->GetPosition(), 0.5f);
	}
	else if (type == FBS::Data::EntityType::EntityType_Projectile)
	{
		nCollisionObjectID = m_CollisionManager.AddProjectile(entity->GetPosition(), 0.5f);
	}
	m_mapEntityCollision[nEntityID] = nCollisionObjectID;

	if (pEntityID != NULL)
		*pEntityID = nEntityID;

	if (pEntity != NULL)
		*pEntity = entity;

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

		int nCollisionObjectID = m_mapEntityCollision[nEntityID];
		m_CollisionManager.RemoveCollisionObject(nCollisionObjectID);
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
	else if (pMsg->GetID() == GameEventMoveToR::MESSAGE_ID)
	{
		OnGameEventMoveToR((GameEventMoveToR*)pMsg, socket);
	}
	else if (pMsg->GetID() == GameEventStopToR::MESSAGE_ID)
	{
		OnGameEventStopToR((GameEventStopToR*)pMsg, socket);
	}
	else if (pMsg->GetID() == GameInputSkillToR::MESSAGE_ID)
	{
		OnGameInputSkillToR((GameInputSkillToR*)pMsg, socket);
	}
}

void BaeGameRoom::OnGameEventMoveToR(GameEventMoveToR* pMsg, unsigned int socket)
{
	m_LockPlayerInput.lock();

	m_mapPlayerInput[pMsg->m_nPlayerIndex] = make_pair(GetElapsedTime(), pMsg->Clone());

	m_LockPlayerInput.unlock();
}

void BaeGameRoom::OnGameEventStopToR(GameEventStopToR* pMsg, unsigned int socket)
{
	m_LockPlayerInput.lock();

	if (m_mapPlayerInput.count(pMsg->m_nPlayerIndex) > 0 && m_mapPlayerInput[pMsg->m_nPlayerIndex].second != NULL)
	{
		delete m_mapPlayerInput[pMsg->m_nPlayerIndex].second;
	}

	m_mapPlayerInput[pMsg->m_nPlayerIndex] = make_pair(GetElapsedTime(), pMsg->Clone());

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
		CreateEntity(FBS::Data::EntityType::EntityType_Character, 0, &nEntityID, NULL);
		m_mapPlayerEntity[nPlayerIndex] = nEntityID;
		m_mapEntityPlayer[nEntityID] = nPlayerIndex;

		enterRoomToC->m_nResult = 0;
		enterRoomToC->m_nPlayerIndex = nPlayerIndex;
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

	if (m_mapPlayerInput.count(pMsg->m_nPlayerIndex) > 0 && m_mapPlayerInput[pMsg->m_nPlayerIndex].second != NULL)
	{
		delete m_mapPlayerInput[pMsg->m_nPlayerIndex].second;
	}

	m_mapPlayerInput[pMsg->m_nPlayerIndex] = make_pair(GetElapsedTime(), pMsg->Clone());

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

unsigned int __stdcall BaeGameRoom::LoopThreadStart(void* param)
{
	BaeGameRoom* pRoom = (BaeGameRoom*)param;

	pRoom->Loop();

	return 0;
}