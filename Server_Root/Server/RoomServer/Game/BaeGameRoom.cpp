#include "stdafx.h"
#include "BaeGameRoom.h"
#include "../CommonSources/Network/Network.h"
#include "../CommonSources/Message/IMessage.h"
#include "RoomMessageHeader.h"


BaeGameRoom::BaeGameRoom(int nMatchID, vector<string> vecMatchedPlayerKey)
{
	m_nMatchID = nMatchID;
	m_vecMatchedPlayerKey = vecMatchedPlayerKey;

	for (int i = 0; i < vecMatchedPlayerKey.size(); ++i)
	{
		m_mapPlayerIndexPreparationState[i] = 0.0f;
	}
}

BaeGameRoom::~BaeGameRoom()
{
	for (vector<pair<__int64, IMessage*>>::iterator it = m_vecGameEventRecord.begin(); it != m_vecGameEventRecord.end(); ++it)
	{
		delete it->second;
	}
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
}

void BaeGameRoom::OnGameEventMoveToR(GameEventMoveToR* pMsg, unsigned int socket)
{
	__int64 lEventTime = GetElapsedTime();

	GameEventMoveToC* res = new GameEventMoveToC();
	res->m_nPlayerIndex = pMsg->m_nPlayerIndex;
	res->m_lEventTime = lEventTime;
	res->m_vec3Dest = pMsg->m_vec3Dest;

	m_vecGameEventRecord.push_back(make_pair(lEventTime, res->Clone()));

	SendToAllUsers(res);
}

void BaeGameRoom::OnGameEventIdleToR(GameEventIdleToR* pMsg, unsigned int socket)
{
	__int64 lEventTime = GetElapsedTime();

	GameEventIdleToC* res = new GameEventIdleToC();
	res->m_nPlayerIndex = pMsg->m_nPlayerIndex;
	res->m_lEventTime = lEventTime;
	res->m_vec3Pos = pMsg->m_vec3Pos;

	m_vecGameEventRecord.push_back(make_pair(lEventTime, res->Clone()));

	SendToAllUsers(res);
}

void BaeGameRoom::OnGameEventStopToR(GameEventStopToR* pMsg, unsigned int socket)
{
	__int64 lEventTime = GetElapsedTime();

	GameEventStopToC* res = new GameEventStopToC();
	res->m_nPlayerIndex = pMsg->m_nPlayerIndex;
	res->m_lEventTime = lEventTime;
	res->m_vec3Pos = pMsg->m_vec3Pos;

	m_vecGameEventRecord.push_back(make_pair(lEventTime, res->Clone()));

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
		m_StartTime = system_clock::now();

		GameStartToC* gameStartToC = new GameStartToC();

		SendToAllUsers(gameStartToC);
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