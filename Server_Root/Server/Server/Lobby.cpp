#include "stdafx.h"
#include "Lobby.h"
#include "../CommonSources/Network/Network.h"
#include "../CommonSources/Message/IMessage.h"
#include "LobbyMessageHeader.h"
#include "LobbyMessageDefines.h"
#include <time.h>
//#include <math.h>

Lobby::Lobby(const unsigned short nPort)
{
	m_pNetwork = new Network(this, nPort, &m_MessageConvertor);
	m_pNetwork->SetAcceptCallback(OnAccept);
	m_pNetwork->SetRecvMessageCallback(OnRecvMessage);

	m_pNetwork->Start();
}

Lobby::~Lobby()
{
	m_pNetwork->Stop();
	delete m_pNetwork;
}

void Lobby::SendToAllUsers(IMessage* pMsg)
{
	for (map<string, unsigned int>::iterator it = m_mapPlayer.begin(); it != m_mapPlayer.end(); it++)
	{
		m_pNetwork->Send(it->second, pMsg, false);
	}
	
	delete pMsg;
}



void Lobby::OnAccept(unsigned int socket)
{
	//	glicko - 2
	
}

void Lobby::OnRecvMessage(unsigned int socket, IMessage* pMsg)
{
	//	temp
	//printf("%s", pMsg->Serialize());

	if (pMsg->GetID() == Messages::JoinLobbyToS_ID)
	{
		OnJoinLobbyToS((JoinLobbyToS*)pMsg, socket);
	}
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
	else if (pMsg->GetID() == Messages::SelectNormalGameToS_ID)
	{
		OnSelectNormalGameToS((SelectNormalGameToS*)pMsg, socket);
	}
}


//	Protocol Handlers
void Lobby::OnJoinLobbyToS(JoinLobbyToS* pMsg, unsigned int socket)
{
	//	now.. always accept..
	/*
	if (!IsAuth(pMsg->m_nAuthKey))
	{
		delete pMsg;
		return;
	}
	*/

	m_mapPlayer[pMsg->m_strPlayerKey] = socket;

	JoinLobbyToC* pMsgToC = new JoinLobbyToC();
	pMsgToC->m_nResult = 0;

	m_pNetwork->Send(socket, pMsgToC);
}

void Lobby::OnSelectNormalGameToS(SelectNormalGameToS* pMsg, unsigned int socket)
{
	SelectNormalGameToC* pMsgToC = new SelectNormalGameToC();
	pMsgToC->m_nResult = 0;

	m_pNetwork->Send(socket, pMsgToC);
}


//	Static Functions for Callback
void Lobby::OnAccept(void* pLobby, unsigned int socket)
{
	if (pLobby == NULL)
	{
		return;
	}

	((Lobby*)pLobby)->OnAccept(socket);
}

void Lobby::OnRecvMessage(void* pLobby, unsigned int socket, IMessage* pMsg)
{
	if (pLobby == NULL || pMsg == NULL)
	{
		return;
	}

	((Lobby*)pLobby)->OnRecvMessage(socket, pMsg);
}