#include "stdafx.h"
#include "BaeGameRoom2.h"
#include "../../CommonSources/Network/Network.h"
#include "../../CommonSources/Message/IMessage.h"
#include "../../RoomServer/RoomMessageHeader.h"
//#include "RoomMessageHeader.h"
#include <process.h>
#include "../Entity/Entities/Character/Character.h"
#include "../Entity/Entities/Projectile/Projectile.h"
#include "../Entity/Entities/Item/Item.h"
#include "../AI/CharacterAI/CharacterAIs/BlobAI.h"
#include "../AI/CharacterAI/CharacterAIs/CockatriceAI.h"
#include "../AI/CharacterAI/CharacterAIs/GolemAI.h"
#include "../AI/CharacterAI/CharacterAIs/IceGolemAI.h"
#include "../AI/CharacterAI/CharacterAIs/IceQueenMaidAI.h"
#include "../AI/CharacterAI/CharacterAIs/MamboRabbitAI.h"
#include "../AI/CharacterAI/CharacterAIs/QueenMamboRabbitAI.h"
#include "../AI/CharacterAI/CharacterAIs/SkullArcherAI.h"
#include "../AI/CharacterAI/CharacterAIs/YetiAI.h"
#include "../Messages/ToClient/WorldSnapShotToC.h"
#include "../Messages/ToClient/WorldInfoToC.h"
#include "../Messages/ToClient/TickInfoToC.h"
#include "../Messages/ToClient/GameEndToC.h"
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
#include "../GameEvent/GameEvents/Position.h"
#include "../GameEvent/GameEvents/Rotation.h"
#include "../GameEvent/GameEvents/CharacterStatusChange.h"
#include "../MasterData/MasterDataManager.h"
#include "../MasterData/Character.h"
#include "../PlayerInput/IPlayerInput.h"
#include "../PlayerInput/PlayerInputs/Rotation.h"

BaeGameRoom2::BaeGameRoom2(int nMatchID, vector<string> vecMatchedPlayerKey)	//	Receive Game Info from Lobby
{
	m_nMatchID = nMatchID;
	m_vecMatchedPlayerKey = vecMatchedPlayerKey;

	for (int i = 0; i < vecMatchedPlayerKey.size(); ++i)
	{
		m_mapPlayerIndexPreparationState[i] = 0.0f;
	}

	m_bPlaying = false;

	PrepareGame();
}

BaeGameRoom2::~BaeGameRoom2()
{
}


void BaeGameRoom2::Loop()
{
	//	must create game worker thread cuz lock delay..
	//(네트워크 쓰레드로 게임 로직 돌리면 게임 룸 락 때문에 네트워크 쓰레드가 대기하게 되고 네트워크 요청을 원활하게 수행할 수 없다.)

	//	Tick Process
	while (m_bPlaying)
	{
		m_lLastUpdateTime = GetElapsedTime();

		SendTickInfo();

		if (m_nTick == m_nEndTick)
		{
			EndGame();
		}
		else
		{
			m_nTick++;
		}

		long long lTickProcessTime = GetElapsedTime() - m_lLastUpdateTime;

		//	Wait tick interval
		if (TIME_STEP > lTickProcessTime)
			Sleep(TIME_STEP - lTickProcessTime);
	}
}

void BaeGameRoom2::SendTickInfo()
{
	TickInfoToC* pTickInfoToC = new TickInfoToC();

	pTickInfoToC->m_nTick = m_nTick;

	for (list<IPlayerInput*>::iterator it = m_listPlayerInput.begin(); it != m_listPlayerInput.end(); ++it)
	{
		pTickInfoToC->m_listPlayerInput.push_back((*it)->Clone());
	}

	SendToAllUsers(pTickInfoToC);

	for (list<IPlayerInput*>::iterator it = m_listPlayerInput.begin(); it != m_listPlayerInput.end(); ++it)
	{
		delete *it;
	}
	m_listPlayerInput.clear();
}

void BaeGameRoom2::PrepareGame()
{
	srand(time(NULL));

	//	Set PlayerInfo
	for (int i = 0; i < m_vecMatchedPlayerKey.size(); ++i)
	{
		int nPlayerIndex = GetPlayerIndexByPlayerKey(m_vecMatchedPlayerKey[i]);
		int dummyCharacterMasterDataID = rand() % 2 == 0 ? 0 : rand() % 2 == 0 ? 1 : 2;		//	Temp..rand
		int nEntityID = 0;

		MasterData::Character* pMasterCharacter = NULL;
		MasterDataManager::Instance()->GetData<MasterData::Character>(dummyCharacterMasterDataID, pMasterCharacter);

		FBS::PlayerInfo playerInfo(nPlayerIndex, dummyCharacterMasterDataID, nEntityID);
		m_vecPlayerInfo.push_back(playerInfo);
	}

	m_nEndTick = TIME_LIMIT * 1000 / TIME_STEP - 1;
}

void BaeGameRoom2::StartGame()
{
	m_bPlaying = true;

	GameStartToC* gameStartToC = new GameStartToC();
	gameStartToC->m_fTickInterval = TIME_STEP / 1000.0f;
	gameStartToC->m_nRandomSeed = 0;	//	Temp
	gameStartToC->m_nTimeLimit = TIME_LIMIT;

	SendToAllUsers(gameStartToC);

	_beginthreadex(NULL, 0, LoopThreadStart, this, 0, NULL);
}

void BaeGameRoom2::EndGame()
{
	//	send end data to players


	//Reset();


	_endthreadex(0);
}

void BaeGameRoom2::Reset()
{
	m_nTick = 0;
	m_lDeltaTime = 0;
	m_lLastUpdateTime = 0;
	m_vecPlayerInfo.clear();

	for (list<IPlayerInput*>::iterator it = m_listPlayerInput.begin(); it != m_listPlayerInput.end(); ++it)
	{
		delete *it;
	}
	m_listPlayerInput.clear();

	m_bPlaying = false;
}

void BaeGameRoom2::SendToAllUsers(IMessage* pMsg, string strExclusionKey, bool bDelete)
{
	for (map<string, unsigned int>::iterator it = m_mapPlayerKeySocket.begin(); it != m_mapPlayerKeySocket.end(); ++it)
	{
		if (it->first != strExclusionKey)
			Network::Instance()->Send(m_mapPlayerKeySocket[it->first], pMsg, false);
	}

	if (bDelete)
		delete pMsg;
}

void BaeGameRoom2::OnRecvMessage(unsigned int socket, IMessage* pMsg)
{
	if (pMsg->GetID() == EnterRoomToR::MESSAGE_ID)
	{
		OnEnterRoomToR((EnterRoomToR*)pMsg, socket);
	}
	else if (pMsg->GetID() == PreparationStateToR::MESSAGE_ID)
	{
		OnPreparationStateToR((PreparationStateToR*)pMsg, socket);
	}
	else if (pMsg->GetID() == PlayerInputToR::MESSAGE_ID)
	{
		OnPlayerInputToR((PlayerInputToR*)pMsg, socket);
	}
	else if (pMsg->GetID() == GameResultToR::MESSAGE_ID)
	{
		OnGameResultToR((GameResultToR*)pMsg, socket);
	}
}

void BaeGameRoom2::OnEnterRoomToR(EnterRoomToR* pMsg, unsigned int socket)
{
	EnterRoomToC* enterRoomToC = new EnterRoomToC();
	
	if (IsValidPlayer(pMsg->m_strPlayerKey))
	{
		int nPlayerIndex = GetPlayerIndexByPlayerKey(pMsg->m_strPlayerKey);

		m_mapPlayerKeySocket[pMsg->m_strPlayerKey] = socket;
		m_mapSocketPlayerKey[socket] = pMsg->m_strPlayerKey;
		
		enterRoomToC->m_nResult = 0;
		enterRoomToC->m_nUserPlayerIndex = nPlayerIndex;
		enterRoomToC->m_vecPlayerInfo = m_vecPlayerInfo;

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

void BaeGameRoom2::OnPreparationStateToR(PreparationStateToR* pMsg, unsigned int socket)
{
	int nPlayerIndex = GetPlayerIndexBySocket(socket);

	PreparationStateToC* preparationStateToC = new PreparationStateToC();

	preparationStateToC->m_nPlayerIndex = nPlayerIndex;
	preparationStateToC->m_fState = pMsg->m_fState;

	SendToAllUsers(preparationStateToC);

	m_mapPlayerIndexPreparationState[nPlayerIndex] = pMsg->m_fState;

	if (IsAllPlayersReady())
	{
		Reset();
		StartGame();
	}
}

void BaeGameRoom2::OnPlayerInputToR(PlayerInputToR* pMsg, unsigned int socket)
{
	m_LockPlayerInput.lock();

	if (m_listPlayerInput.size() == 0)
	{
		m_listPlayerInput.push_back(pMsg->m_PlayerInput->Clone());
	}

	m_LockPlayerInput.unlock();
}

void BaeGameRoom2::OnGameResultToR(GameResultToR* pMsg, unsigned int socket)
{
	GameEndToC* gameEndToC = new GameEndToC();
	gameEndToC->m_vecPlayerRankInfo = pMsg->m_vecPlayerRankInfo;

	SendToAllUsers(gameEndToC);
}

int BaeGameRoom2::GetPlayerIndexByPlayerKey(string strPlayerKey)
{
	for (int i = 0; i < m_vecMatchedPlayerKey.size(); ++i)
	{
		if (m_vecMatchedPlayerKey[i] == strPlayerKey)
			return i;
	}

	return -1;
}

int BaeGameRoom2::GetPlayerIndexBySocket(unsigned int socket)
{
	return GetPlayerIndexByPlayerKey(m_mapSocketPlayerKey[socket]);
}

bool BaeGameRoom2::IsValidPlayer(string strPlayerKey)
{
	//if (pMsg->m_nMatchID != m_nMatchID || m_vecMatchedPlayers.~_Container_base12(pMsg->m_strPlayerKey))

	//	Temp
	return true;
}

bool BaeGameRoom2::IsAllPlayersReady()
{
	for (map<int, float>::iterator it = m_mapPlayerIndexPreparationState.begin(); it != m_mapPlayerIndexPreparationState.end(); ++it)
	{
		if (it->second < 1.0f)
			return false;
	}

	return true;
}

long long BaeGameRoom2::GetElapsedTime()
{
	milliseconds elapsedTime = duration_cast<milliseconds>(system_clock::now() - m_StartTime);

	return elapsedTime.count();
}

long long BaeGameRoom2::GetLastUpdateTime()
{
	return m_lLastUpdateTime;
}

unsigned int __stdcall BaeGameRoom2::LoopThreadStart(void* param)
{
	BaeGameRoom2* pRoom = (BaeGameRoom2*)param;

	pRoom->Loop();

	return 0;
}