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

	//if (pMsg->GetID() == Messages::JoinLobbyToS_ID)
	//{
	//	OnJoinLobbyToS((JoinLobbyToS*)pMsg, socket);
	//}
	//else if (pMsg->GetID() == Messages::ReadyForStartToS_ID)
	//{
	//	GameStartToC* pTest = new GameStartToC();
	//	pTest->m_lGameElapsedTime = 0;

	//	//time_t curTime;
	//	//time(&curTime);

	//	m_pNetwork->Send(socket, pTest);
	//}
	//else if (pMsg->GetID() == Messages::GameEventMoveToS_ID)
	//{
	//	GameEventMoveToC* pMsgToC = new GameEventMoveToC();
	//	pMsgToC->m_nPlayerIndex = 0;
	//	pMsgToC->m_nElapsedTime = 0;
	//	pMsgToC->m_vec3Dest = ((GameEventMoveToS*)pMsg)->m_vec3Dest;

	//	m_pNetwork->Send(socket, pMsgToC);
	//}
	//else if (pMsg->GetID() == Messages::SelectNormalGameToS_ID)
	//{
	//	OnSelectNormalGameToS((SelectNormalGameToS*)pMsg, socket);
	//}
}


//	Protocol Handlers



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