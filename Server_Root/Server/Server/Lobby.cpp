#include "stdafx.h"
#include "Lobby.h"
#include "Network.h"
#include "IMessage.h"
#include "TestMessage.h"
#include "NetworkDefines.h"


Lobby::Lobby(const unsigned short nPort)
{
	m_pNetwork = new Network(nPort);
	m_pNetwork->SetRecvMessageCallback(this, OnRecvMessage);

	m_pNetwork->Start();
}


Lobby::~Lobby()
{
	m_pNetwork->Stop();
	delete m_pNetwork;
}

void Lobby::OnRecvMessage(unsigned int socket, IMessage* pMsg)
{
	if (pMsg->GetID() == Messages::TEST_MESSAGE_ID)
	{
		//	temp
		TestMessage* pTestMsg = (TestMessage*)pMsg;
		printf("%s", pTestMsg->Serialize().c_str());

		m_pNetwork->Send(socket, pMsg);
	}
}

void Lobby::OnRecvMessage(void* pLobby, unsigned int socket, IMessage* pMsg)
{
	if (pLobby == NULL || pMsg == NULL)
	{
		return;
	}

	((Lobby*)pLobby)->OnRecvMessage(socket, pMsg);
}