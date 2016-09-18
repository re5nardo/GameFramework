#include "stdafx.h"
#include "Room.h"
#include "../CommonSources/Network/Network.h"
#include "../CommonSources/Message/IMessage.h"
#include "RoomMessageHeader.h"
#include <time.h>
//#include <math.h>

Room::Room(const unsigned short nPort)
{
	m_pNetwork = new Network(this, nPort, &m_MessageConvertor);
	m_pNetwork->SetAcceptCallback(OnAccept);
	m_pNetwork->SetRecvMessageCallback(OnRecvMessage);

	m_pNetwork->Start();
}

Room::~Room()
{
	m_pNetwork->Stop();
	delete m_pNetwork;
}

void Room::SendToAllUsers(IMessage* pMsg)
{
	for (map<string, unsigned int>::iterator it = m_mapPlayer.begin(); it != m_mapPlayer.end(); it++)
	{
		m_pNetwork->Send(it->second, pMsg, false);
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

	delete pMsg;
}


//	Protocol Handlers
void Room::OnCreateRoomToR(CreateRoomToR* pMsg, unsigned int socket)
{
	CreateRoomToL* res = new CreateRoomToL();
	res->m_nResult = 0;
	
	for (int i = 0; i < pMsg->m_vecPlayers.size(); ++i)
	{
		res->m_vecPlayers.push_back(pMsg->m_vecPlayers[i]);
	}

	m_pNetwork->Send(socket, res, true, true);
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