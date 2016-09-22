#include "stdafx.h"
#include "Room.h"
#include "../CommonSources/Network/Network.h"
#include "../CommonSources/Message/IMessage.h"
#include "RoomMessageHeader.h"
#include "Game\BaeGameRoom.h"
#include <time.h>
//#include <math.h>

Room::Room(const unsigned short nPort)
{
	Network::Instance()->Initialize(this, nPort, &m_MessageConvertor);
	Network::Instance()->SetAcceptCallback(OnAccept);
	Network::Instance()->SetRecvMessageCallback(OnRecvMessage);

	Network::Instance()->Start();
}

Room::~Room()
{
	Network::Instance()->Stop();
	Network::Instance()->Destroy();
}

void Room::SendToAllUsers(IMessage* pMsg)
{
	for (map<string, unsigned int>::iterator it = m_mapPlayerSocket.begin(); it != m_mapPlayerSocket.end(); it++)
	{
		Network::Instance()->Send(it->second, pMsg, false);
	}

	delete pMsg;
}



void Room::OnAccept(unsigned int socket)
{
	//	glicko - 2

}

void Room::OnRecvMessage(unsigned int socket, IMessage* pMsg)
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

	delete pMsg;
}


//	Protocol Handlers
void Room::OnCreateRoomToR(CreateRoomToR* pMsg, unsigned int socket)
{
	BaeGameRoom* gameRoom = new BaeGameRoom(pMsg->m_nMatchID, pMsg->m_vecPlayers);
	
	m_mapMatchIDGameRoom[pMsg->m_nMatchID] = gameRoom;

	for (int i = 0; i < pMsg->m_vecPlayers.size(); ++i)
	{
		m_mapPlayerGameRoom[pMsg->m_vecPlayers[i]] = gameRoom;
	}

	CreateRoomToL* res = new CreateRoomToL();
	res->m_nResult = 0;
	
	for (int i = 0; i < pMsg->m_vecPlayers.size(); ++i)
	{
		res->m_vecPlayers.push_back(pMsg->m_vecPlayers[i]);
	}

	Network::Instance()->Send(socket, res, true, true);
}

void Room::OnGameEventMoveToR(GameEventMoveToR* pMsg, unsigned int socket)
{
	m_mapPlayerGameRoom[m_mapSocketPlayer[socket]]->OnRecvMessage(socket, pMsg);
	

	/*GameEventMoveToC* res = new GameEventMoveToC();
	res->m_nPlayerIndex = pMsg->m_nPlayerIndex;
	res->m_nElapsedTime = pMsg->m_nElapsedTime;
	res->m_vec3Dest = pMsg->m_vec3Dest;

	SendToAllUsers(res);*/
}

void Room::OnEnterRoomToR(EnterRoomToR* pMsg, unsigned int socket)
{
	//	now.. always accept..
	/*
	if (!IsAuth(pMsg->m_nAuthKey))
	{
	delete pMsg;
	return;
	}
	*/

	m_mapPlayerSocket[pMsg->m_strPlayerKey] = socket;
	m_mapSocketPlayer[socket] = pMsg->m_strPlayerKey;



	m_mapPlayerGameRoom[pMsg->m_strPlayerKey]->OnRecvMessage(socket, pMsg);
}


//	Static Functions for Callback
void Room::OnAccept(void* pRoom, unsigned int socket)
{
	if (pRoom == NULL)
	{
		return;
	}

	((Room*)pRoom)->OnAccept(socket);
}

void Room::OnRecvMessage(void* pRoom, unsigned int socket, IMessage* pMsg)
{
	if (pRoom == NULL || pMsg == NULL)
	{
		return;
	}

	((Room*)pRoom)->OnRecvMessage(socket, pMsg);
}