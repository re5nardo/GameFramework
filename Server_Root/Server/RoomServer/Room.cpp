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
	else if (pMsg->GetID() == EnterRoomToR::MESSAGE_ID)
	{
		OnEnterRoomToR((EnterRoomToR*)pMsg, socket);
	}
	else
	{
		m_mapPlayerKeyGameRoom[m_mapSocketPlayerKey[socket]]->OnRecvMessage(socket, pMsg);
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
		m_mapPlayerKeyGameRoom[pMsg->m_vecPlayers[i]] = gameRoom;
	}

	CreateRoomToL* res = new CreateRoomToL();
	res->m_nResult = 0;
	
	for (int i = 0; i < pMsg->m_vecPlayers.size(); ++i)
	{
		res->m_vecPlayers.push_back(pMsg->m_vecPlayers[i]);
	}

	Network::Instance()->Send(socket, res, true, true);
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

	m_mapPlayerKeySocket[pMsg->m_strPlayerKey] = socket;
	m_mapSocketPlayerKey[socket] = pMsg->m_strPlayerKey;



	m_mapPlayerKeyGameRoom[pMsg->m_strPlayerKey]->OnRecvMessage(socket, pMsg);
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