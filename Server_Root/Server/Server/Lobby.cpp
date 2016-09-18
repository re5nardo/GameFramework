#include "stdafx.h"
#include "Lobby.h"
#include "../CommonSources/Network/Network.h"
#include "../CommonSources/Message/IMessage.h"
#include "LobbyMessageHeader.h"
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
	for (map<string, unsigned int>::iterator it = m_mapPlayerSocket.begin(); it != m_mapPlayerSocket.end(); it++)
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

	if (pMsg->GetID() == JoinLobbyToS::MESSAGE_ID)
	{
		OnJoinLobbyToS((JoinLobbyToS*)pMsg, socket);
	}
	else if (pMsg->GetID() == SelectNormalGameToS::MESSAGE_ID)
	{
		OnSelectNormalGameToS((SelectNormalGameToS*)pMsg, socket);
	}
	else if (pMsg->GetID() == CreateRoomToL::MESSAGE_ID)
	{
		OnCreateRoomToL((CreateRoomToL*)pMsg, socket);
	}

	delete pMsg;
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

	m_mapPlayerSocket[pMsg->m_strPlayerKey] = socket;
	m_mapSocketPlayer[socket] = pMsg->m_strPlayerKey;

	JoinLobbyToC* pMsgToC = new JoinLobbyToC();
	pMsgToC->m_nResult = 0;

	m_pNetwork->Send(socket, pMsgToC);
}

void Lobby::OnSelectNormalGameToS(SelectNormalGameToS* pMsg, unsigned int socket)
{
	//	Match making.. & select room server..
	//	...

	CreateRoomToR* req = new CreateRoomToR();
	req->m_nMatchID = 119;
	req->m_vecPlayers.push_back(m_mapSocketPlayer[socket]);

	m_pNetwork->Send("175.197.228.153", 9111, req);
}

void Lobby::OnCreateRoomToL(CreateRoomToL* pMsg, unsigned int socket)
{
	SelectNormalGameToC* pMsgToC = new SelectNormalGameToC();
	pMsgToC->m_nResult = pMsg->m_nResult;

	for (int i = 0; i < pMsg->m_vecPlayers.size(); ++i)
	{
		m_pNetwork->Send(m_mapPlayerSocket[pMsg->m_vecPlayers[i]], pMsgToC);
	}
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