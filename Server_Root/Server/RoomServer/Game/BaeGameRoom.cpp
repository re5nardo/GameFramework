#include "stdafx.h"
#include "BaeGameRoom.h"
#include "../CommonSources/Network/Network.h"
#include "../CommonSources/Message/IMessage.h"
#include "RoomMessageHeader.h"
#include <time.h>


BaeGameRoom::BaeGameRoom()
{

}

BaeGameRoom::~BaeGameRoom()
{

}

void BaeGameRoom::SendToAllUsers(IMessage* pMsg)
{
	for (map<string, unsigned int>::iterator it = m_mapPlayerSocket.begin(); it != m_mapPlayerSocket.end(); it++)
	{
		
	}

	delete pMsg;
}


void BaeGameRoom::OnRecvMessage(unsigned int socket, IMessage* pMsg)
{
	//	temp
	//printf("%s", pMsg->Serialize());

	if (pMsg->GetID() == CreateRoomToR::MESSAGE_ID)
	{
		OnCreateRoomToR((CreateRoomToR*)pMsg, socket);
	}
	else if (pMsg->GetID() == GameEventMoveToS::MESSAGE_ID)
	{
		OnGameEventMoveToS((GameEventMoveToS*)pMsg, socket);
	}

	delete pMsg;
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

void BaeGameRoom::OnGameEventMoveToS(GameEventMoveToS* pMsg, unsigned int socket)
{
	GameEventMoveToC* res = new GameEventMoveToC();
	res->m_nPlayerIndex = pMsg->m_nPlayerIndex;
	res->m_nElapsedTime = pMsg->m_nElapsedTime;
	res->m_vec3Dest = pMsg->m_vec3Dest;

	SendToAllUsers(res);
}