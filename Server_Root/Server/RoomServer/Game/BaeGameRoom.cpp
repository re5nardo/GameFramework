#include "stdafx.h"
#include "BaeGameRoom.h"
#include "../CommonSources/Network/Network.h"
#include "../CommonSources/Message/IMessage.h"
#include "RoomMessageHeader.h"
#include <time.h>


BaeGameRoom::BaeGameRoom(int nMatchID, vector<string> vecMatchedPlayers)
{
	m_nMatchID = nMatchID;
	m_vecMatchedPlayers = vecMatchedPlayers;
}

BaeGameRoom::~BaeGameRoom()
{

}

void BaeGameRoom::SendToAllUsers(IMessage* pMsg)
{
	for (int i = 0; i < m_vecPlayers.size(); ++i)
	{
		Network::Instance()->Send(m_mapPlayerSocket[m_vecPlayers[i]], pMsg);
	}
}


void BaeGameRoom::OnRecvMessage(unsigned int socket, IMessage* pMsg)
{
	//	temp
	//printf("%s", pMsg->Serialize());

	if (pMsg->GetID() == CreateRoomToR::MESSAGE_ID)
	{
		OnCreateRoomToR((CreateRoomToR*)pMsg, socket);
	}
	else if (pMsg->GetID() == GameEventMoveToR::MESSAGE_ID)
	{
		OnGameEventMoveToR((GameEventMoveToR*)pMsg, socket);
	}
	else if (pMsg->GetID() == EnterRoomToR::MESSAGE_ID)
	{
		OnEnterRoomToR((EnterRoomToR*)pMsg, socket);
	}
}


//	Protocol Handlers
void BaeGameRoom::OnCreateRoomToR(CreateRoomToR* pMsg, unsigned int socket)
{
	CreateRoomToL* res = new CreateRoomToL();
	res->m_nResult = 0;

	for (int i = 0; i < pMsg->m_vecPlayers.size(); ++i)
	{
		res->m_vecPlayers.push_back(pMsg->m_vecPlayers[i]);
	}

	//m_pNetwork->Send(socket, res, true, true);
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
	EnterRoomToC* res = new EnterRoomToC();

	//if (pMsg->m_nMatchID != m_nMatchID || m_vecMatchedPlayers.~_Container_base12(pMsg->m_strPlayerKey))
	if (false)
	{
		res->m_nResult = -1;
	}
	else
	{
		res->m_nResult = 0;
	}

	m_vecPlayers.push_back(pMsg->m_strPlayerKey);
	m_mapPlayerSocket[pMsg->m_strPlayerKey] = socket;

	Network::Instance()->Send(socket, res);
}