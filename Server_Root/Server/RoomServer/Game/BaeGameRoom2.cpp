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
		long long lTickProcessTime = GetElapsedTime() - m_lLastUpdateTime;

		//	Wait tick interval
		if (TIME_STEP > lTickProcessTime)
			Sleep(TIME_STEP - lTickProcessTime);

		ProcessInput();

		SendTickInfo();

		m_lLastUpdateTime = GetElapsedTime();

		m_nTick++;
	}
}

void BaeGameRoom2::ProcessInput()
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
		delete it->second.second;
	}
	vecPlayerInput.clear();

	m_LockPlayerInput.unlock();
}

void BaeGameRoom2::SendWorldInfo()
{
	
}

void BaeGameRoom2::SendTickInfo()
{
	TickInfoToC* pTickInfoToC = new TickInfoToC();

	pTickInfoToC->m_nTick = m_nTick + 1;

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
	//	Set PlayerInfo
	for (int i = 0; i < m_vecMatchedPlayerKey.size(); ++i)
	{
		int nPlayerIndex = GetPlayerIndexByPlayerKey(m_vecMatchedPlayerKey[i]);
		int dummyCharacterMasterDataID = 0;		//	Temp..0 is MisterBae
		int nEntityID = 0;

		MasterData::Character* pMasterCharacter = NULL;
		MasterDataManager::Instance()->GetData<MasterData::Character>(dummyCharacterMasterDataID, pMasterCharacter);
		FBS::Data::CharacterStatus status(pMasterCharacter->m_nHP, pMasterCharacter->m_nHP, pMasterCharacter->m_nMP, pMasterCharacter->m_nMP, pMasterCharacter->m_fMaximumSpeed, 0, pMasterCharacter->m_fMPChargeRate, 0);

		FBS::PlayerInfo playerInfo(nPlayerIndex, dummyCharacterMasterDataID, nEntityID, status);
		m_vecPlayerInfo.push_back(playerInfo);
	}
}

void BaeGameRoom2::StartGame()
{
	m_bPlaying = true;

	GameStartToC* gameStartToC = new GameStartToC();
	gameStartToC->m_fTickInterval = TIME_STEP / 1000.0f;
	gameStartToC->m_nRandomSeed = 0;	//	Temp

	SendToAllUsers(gameStartToC);

	_beginthreadex(NULL, 0, LoopThreadStart, this, 0, NULL);
}

void BaeGameRoom2::EndGame()
{
	Reset();
}

void BaeGameRoom2::Reset()
{
	m_nTick = 0;
	m_lDeltaTime = 0;
	m_lLastUpdateTime = 0;
	m_vecPlayerInfo.clear();

	m_bPlaying = false;

	for (map<int, list<pair<long long, IMessage*>>>::iterator it = m_mapPlayerInput.begin(); it != m_mapPlayerInput.end(); ++it)
	{
		for (list<pair<long long, IMessage*>>::iterator iter = it->second.begin(); iter != it->second.end(); ++iter)
		{
			delete iter->second;
		}
	}
	m_mapPlayerInput.clear();
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
	if (pMsg->GetID() == GameInputSkillToR::MESSAGE_ID)
	{
		OnGameInputSkillToR((GameInputSkillToR*)pMsg, socket);
	}
	else if (pMsg->GetID() == GameInputMoveToR::MESSAGE_ID)
	{
		OnGameInputMoveToR((GameInputMoveToR*)pMsg, socket);
	}
	else if (pMsg->GetID() == GameInputRotationToR::MESSAGE_ID)
	{
		OnGameInputRotationToR((GameInputRotationToR*)pMsg, socket);
	}
	else if (pMsg->GetID() == PlayerInputToR::MESSAGE_ID)
	{
		OnPlayerInputToR((PlayerInputToR*)pMsg, socket);
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

void BaeGameRoom2::OnGameInputSkillToR(GameInputSkillToR* pMsg, unsigned int socket)
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

void BaeGameRoom2::OnGameInputMoveToR(GameInputMoveToR* pMsg, unsigned int socket)
{
	m_LockPlayerInput.lock();

	if (m_mapPlayerInput.count(pMsg->m_nPlayerIndex) > 0)
	{
		list<pair<long long, IMessage*>>* pInputs = &m_mapPlayerInput[pMsg->m_nPlayerIndex];

		pInputs->push_back(make_pair(GetElapsedTime(), pMsg->Clone()));
	}
	else
	{
		m_mapPlayerInput[pMsg->m_nPlayerIndex].push_back(make_pair(GetElapsedTime(), pMsg->Clone()));
	}

	m_LockPlayerInput.unlock();
}

void BaeGameRoom2::OnGameInputRotationToR(GameInputRotationToR* pMsg, unsigned int socket)
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

void BaeGameRoom2::OnPlayerInputToR(PlayerInputToR* pMsg, unsigned int socket)
{
	m_LockPlayerInput.lock();

	m_listPlayerInput.push_back(pMsg->m_PlayerInput->Clone());

	m_LockPlayerInput.unlock();
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