#include "stdafx.h"
#include "BaeGameRoom.h"
#include "../CommonSources/Network/Network.h"
#include "../CommonSources/Message/IMessage.h"
#include "RoomMessageHeader.h"
#include <time.h>


BaeGameRoom::BaeGameRoom(int nMatchID, vector<string> vecMatchedPlayerKey)
{
	m_nMatchID = nMatchID;

	for (int i = 0; i < vecMatchedPlayerKey.size(); ++i)
	{
		m_mapPlayerIndexPlayerKey[i] = vecMatchedPlayerKey[i];

		m_mapPlayerIndexPreparationState[i] = 0.0f;
	}
}

BaeGameRoom::~BaeGameRoom()
{

}

void BaeGameRoom::SendToAllUsers(IMessage* pMsg, string strExclusionKey)
{
	for (map<string, unsigned int>::iterator it = m_mapPlayerKeySocket.begin(); it != m_mapPlayerKeySocket.end(); ++it)
	{
		if (it->first != strExclusionKey)
			Network::Instance()->Send(m_mapPlayerKeySocket[it->first], pMsg);
	}
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
}

void BaeGameRoom::OnGameEventMoveToR(GameEventMoveToR* pMsg, unsigned int socket)
{
	GameEventMoveToC* res = new GameEventMoveToC();
	res->m_nPlayerIndex = pMsg->m_nPlayerIndex;
	res->m_nElapsedTime = pMsg->m_nElapsedTime;
	res->m_vec3Dest = pMsg->m_vec3Dest;

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
		m_mapPlayerKeyPlayerIndex[pMsg->m_strPlayerKey] = nPlayerIndex;
		m_mapPlayerIndexPlayerKey[nPlayerIndex] = pMsg->m_strPlayerKey;

		enterRoomToC->m_nResult = 0;
		enterRoomToC->m_nPlayerIndex = nPlayerIndex;
		for (map<int, string>::iterator it = m_mapPlayerIndexPlayerKey.begin(); it != m_mapPlayerIndexPlayerKey.end(); ++it)
		{
			enterRoomToC->m_mapPlayers[it->first] = it->second;
		}

		Network::Instance()->Send(socket, enterRoomToC);
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
		GameStartToC* gameStartToC = new GameStartToC();

		SendToAllUsers(gameStartToC);
	}
}

int BaeGameRoom::GetPlayerIndexByPlayerKey(string strPlayerKey)
{
	return m_mapPlayerKeyPlayerIndex[strPlayerKey];
}

int BaeGameRoom::GetPlayerIndexBySocket(unsigned int socket)
{
	return m_mapPlayerKeyPlayerIndex[m_mapSocketPlayerKey[socket]];
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