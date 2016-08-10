#include "stdafx.h"
#include "Lobby.h"
#include "Network.h"
#include "IMessage.h"
#include "TestMessage.h"
#include "GameStartToC.h"
#include "GameEvent_Move_ToC.h"
#include "GameEvent_Move_ToS.h"
#include "JoinLobbyToS.h"
#include "JoinLobbyToC.h"
#include "NetworkDefines.h"
#include <time.h>
//#include <math.h>

Lobby::Lobby(const unsigned short nPort)
{
	m_pNetwork = new Network(this, nPort);
	m_pNetwork->SetAcceptCallback(OnAccept);
	m_pNetwork->SetRecvMessageCallback(OnRecvMessage);

	m_pNetwork->Start();
}

Lobby::~Lobby()
{
	m_pNetwork->Stop();
	delete m_pNetwork;
}

void Lobby::OnAccept(unsigned int socket)
{
	//	glicko - 2
	
}

void Lobby::OnRecvMessage(unsigned int socket, IMessage* pMsg)
{
	//	temp
	//printf("%s", pMsg->Serialize());

	if (pMsg->GetID() == Messages::Join_Lobby_ToS)
	{
		OnJoinLobbyToS((JoinLobbyToS*)pMsg, socket);
	}
	else if (pMsg->GetID() == Messages::Ready_For_Start_ToS)
	{
		GameStartToC* pTest = new GameStartToC();
		pTest->m_lGameElapsedTime = 0;

		//time_t curTime;
		//time(&curTime);

		m_pNetwork->Send(socket, pTest);
	}
	else if (pMsg->GetID() == Messages::Game_Event_Move_ToS)
	{
		GameEvent_Move_ToC* pMsgToC = new GameEvent_Move_ToC();
		pMsgToC->m_nPlayerIndex = 0;
		pMsgToC->m_nElapsedTime = 0;
		pMsgToC->m_vec3Dest = ((GameEvent_Move_ToS*)pMsg)->m_vec3Dest;

		m_pNetwork->Send(socket, pMsgToC);
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