#include "stdafx.h"
#include "Lobby.h"
#include "Network.h"
#include "IMessage.h"
#include "TestMessage.h"
#include "NetworkDefines.h"


Lobby::Lobby(const unsigned short nPort)
{
	m_pNetwork = new Network(nPort);
	m_pNetwork->SetRecvMessageCallback(OnRecvMessage);

	m_pNetwork->Start();
}


Lobby::~Lobby()
{
	m_pNetwork->Stop();
	delete m_pNetwork;
}

void Lobby::OnRecvMessage(unsigned int socket, IMessage* pMsg)
{
	if (pMsg == NULL)
	{
		return;
	}

	if (pMsg->GetID() == Messages::TEST_MESSAGE_ID)
	{
		//	temp
		TestMessage* pTestMsg = (TestMessage*)pMsg;
		printf("%s", pTestMsg->Serialize().c_str());
	}

	delete pMsg;
}