#include "stdafx.h"
#include "BaeGameRoom.h"
#include "../../CommonSources/Network/Network.h"
#include "../../CommonSources/Message/IMessage.h"
#include "../../RoomServer/RoomMessageHeader.h"
//#include "RoomMessageHeader.h"
#include <process.h>
#include "../Entity/Entities/Character/ICharacter.h"
#include "../Messages/WorldSnapShotToC.h"
#include "../Behavior/BehaviorIDs.h"
#include "../Entity/Entities/Character/Characters/MisterBae.h"
#include "../../CommonSources/tinyxml2.h"
#include "../../CommonSources/QuadTree.h"

BaeGameRoom::BaeGameRoom(int nMatchID, vector<string> vecMatchedPlayerKey)
{
	m_nMatchID = nMatchID;
	m_vecMatchedPlayerKey = vecMatchedPlayerKey;

	for (int i = 0; i < vecMatchedPlayerKey.size(); ++i)
	{
		m_mapPlayerIndexPreparationState[i] = 0.0f;
	}

	m_bPlaying = false;
}

BaeGameRoom::~BaeGameRoom()
{
}


void BaeGameRoom::Loop()
{
	//	must create game worker thread cuz lock delay..

	//	Tick Process
	while (m_bPlaying)
	{
		UpdateTime();

		ProcessInput();

		//	Update world
		Update();
		LateUpdate();

		SendWorldSnapShot();

		__int64 lTickProcessTime = GetElapsedTime() - m_lLastUpdateTime;

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
		__int64 lElapsedTime = GetElapsedTime();

		m_lDeltaTime = lElapsedTime - m_lLastUpdateTime;
		m_lLastUpdateTime = lElapsedTime;
	}
}

void BaeGameRoom::ProcessInput()
{
	m_LockPlayerInput.lock();

	for (map<int, IMessage*>::iterator it = m_mapPlayerInput.begin(); it != m_mapPlayerInput.end(); ++it)
	{
		int nPlayerIndex = it->first;
		IMessage* pPlayerInputMsg = it->second;

		if (pPlayerInputMsg == NULL)
			continue;

		if (pPlayerInputMsg->GetID() == GameEventMoveToR_ID)
		{
			GameEventMoveToR* pMoveToR = (GameEventMoveToR*)pPlayerInputMsg;

			m_mapCharacter[nPlayerIndex]->GetBehavior(Move_ID)->Start(m_lLastUpdateTime, pMoveToR->m_vec3Dest, &m_MapManager);
		}

		delete pPlayerInputMsg;
		it->second = NULL;
	}

	m_LockPlayerInput.unlock();
}

void BaeGameRoom::Update()
{
	for (map<int, ICharacter*>::iterator it = m_mapCharacter.begin(); it != m_mapCharacter.end(); ++it)
	{
		ICharacter* pCharacter = it->second;

		list<IBehavior*> listBehavior = pCharacter->GetActivatedBehaviors();
		for (list<IBehavior*>::iterator iteratorBehavior = listBehavior.begin(); iteratorBehavior != listBehavior.end(); ++iteratorBehavior)
		{
			IBehavior* pBehavior = *iteratorBehavior;
			pBehavior->Update(m_lLastUpdateTime);
		}
	}
}

void BaeGameRoom::LateUpdate()
{
	for (map<int, ICharacter*>::iterator it = m_mapCharacter.begin(); it != m_mapCharacter.end(); ++it)
	{
		ICharacter* pCharacter = it->second;

		if (!pCharacter->IsBehavioring())
		{
			pCharacter->GetBehavior(Idle_ID)->Start(m_lLastUpdateTime);
		}
	}
}

void BaeGameRoom::SendWorldSnapShot()
{
	WorldSnapShotToC* pWorldSnapShotToC = new WorldSnapShotToC();

	pWorldSnapShotToC->m_nTick = m_nTick;
	pWorldSnapShotToC->m_fTime = m_lLastUpdateTime / 1000.0f;

	for (map<int, ICharacter*>::iterator it = m_mapCharacter.begin(); it != m_mapCharacter.end(); ++it)
	{
		pWorldSnapShotToC->m_mapEntity[it->first] = it->second;
	}

	SendToAllUsers(pWorldSnapShotToC);
}

void BaeGameRoom::StartGame()
{
	m_bPlaying = true;
	m_nTick = 0;

	m_MapManager.LoadMap(1);

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
	m_nTick = 0;
	m_lDeltaTime = 0;
	m_lLastUpdateTime = 0;

	m_bPlaying = false;

	for (map<int, IMessage*>::iterator it = m_mapPlayerInput.begin(); it != m_mapPlayerInput.end(); ++it)
	{
		delete it->second;
	}
	m_mapPlayerInput.clear();

	for (map<int, ICharacter*>::iterator it = m_mapCharacter.begin(); it != m_mapCharacter.end(); ++it)
	{
		delete it->second;
	}
	m_mapCharacter.clear();
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
	else if (pMsg->GetID() == GameEventIdleToR::MESSAGE_ID)
	{
		OnGameEventIdleToR((GameEventIdleToR*)pMsg, socket);
	}
	else if (pMsg->GetID() == GameEventStopToR::MESSAGE_ID)
	{
		OnGameEventStopToR((GameEventStopToR*)pMsg, socket);
	}
	else if (pMsg->GetID() == GameEventTeleportToR::MESSAGE_ID)
	{
		GameEventTeleportToR* tt = (GameEventTeleportToR*)pMsg;

		__int64 lEventTime = GetElapsedTime();

		GameEventTeleportToC* res = new GameEventTeleportToC();
		res->m_nPlayerIndex = tt->m_nPlayerIndex;
		res->m_lEventTime = lEventTime;
		res->m_vec3Start = tt->m_vec3Start;
		res->m_vec3Dest = tt->m_vec3Dest;
		res->m_nState = tt->m_nState;

		//m_vecGameEventRecord.push_back(make_pair(lEventTime, res->Clone()));

		SendToAllUsers(res);
	}
}

void BaeGameRoom::OnGameEventMoveToR(GameEventMoveToR* pMsg, unsigned int socket)
{
	/*__int64 lEventTime = GetElapsedTime();

	GameEventMoveToC* res = new GameEventMoveToC();
	res->m_nPlayerIndex = pMsg->m_nPlayerIndex;
	res->m_lEventTime = lEventTime;
	res->m_vec3Dest = pMsg->m_vec3Dest;

	m_vecGameEventRecord.push_back(make_pair(lEventTime, res->Clone()));

	SendToAllUsers(res);*/

	/*MoveInput* moveInput = new MoveInput();
	moveInput->playerIndex = pMsg->m_nPlayerIndex;
	moveInput->pos.x = pMsg->m_vec3Dest.x;
	moveInput->pos.y = pMsg->m_vec3Dest.y;
	moveInput->pos.z = pMsg->m_vec3Dest.z;*/

	m_LockPlayerInput.lock();


	m_mapPlayerInput[pMsg->m_nPlayerIndex] = pMsg->Clone();

	//m_mapPlayerInput[pMsg->m_nPlayerIndex] = pMsg->Clone();

	m_LockPlayerInput.unlock();
}

void BaeGameRoom::OnGameEventIdleToR(GameEventIdleToR* pMsg, unsigned int socket)
{
	__int64 lEventTime = GetElapsedTime();

	GameEventIdleToC* res = new GameEventIdleToC();
	res->m_nPlayerIndex = pMsg->m_nPlayerIndex;
	res->m_lEventTime = lEventTime;
	res->m_vec3Pos = pMsg->m_vec3Pos;

	//m_vecGameEventRecord.push_back(make_pair(lEventTime, res->Clone()));

	SendToAllUsers(res);
}

void BaeGameRoom::OnGameEventStopToR(GameEventStopToR* pMsg, unsigned int socket)
{
	__int64 lEventTime = GetElapsedTime();

	GameEventStopToC* res = new GameEventStopToC();
	res->m_nPlayerIndex = pMsg->m_nPlayerIndex;
	res->m_lEventTime = lEventTime;
	res->m_vec3Pos = pMsg->m_vec3Pos;

	//m_vecGameEventRecord.push_back(make_pair(lEventTime, res->Clone()));

	SendToAllUsers(res);
}

void BaeGameRoom::OnEnterRoomToR(EnterRoomToR* pMsg, unsigned int socket)
{
	EnterRoomToC* enterRoomToC = new EnterRoomToC();
	
	if (IsValidPlayer(pMsg->m_strPlayerKey))
	{
		int nPlayerIndex = GetPlayerIndexByPlayerKey(pMsg->m_strPlayerKey);

		m_mapPlayerKeySocket[pMsg->m_strPlayerKey] = socket;
		m_mapSocketPlayerKey[socket] = pMsg->m_strPlayerKey;

		m_mapCharacter[nPlayerIndex] = new MisterBae();
		m_mapCharacter[nPlayerIndex]->Initialize();
		m_mapCharacter[nPlayerIndex]->SetStat(Stat(3.0f, 1.0f));
		m_mapCharacter[nPlayerIndex]->SetPosition(Vector3());
		m_mapCharacter[nPlayerIndex]->SetRotation(Vector3());

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

__int64 BaeGameRoom::GetElapsedTime()
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