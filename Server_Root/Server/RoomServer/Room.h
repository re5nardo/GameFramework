#pragma once

#include <map>
#include <string>
#include "RoomMessageConvertor.h"
#include "RoomMessageHeader.h"

class Network;
class CreateRoomToR;

using namespace std;

class Room
{
public:
	Room(const unsigned short nPort);
	virtual ~Room();

private:
	Network*							m_pNetwork;
	map<string, unsigned int>			m_mapPlayer;
	RoomMessageConvertor				m_MessageConvertor;

private:
	void OnAccept(unsigned int socket);
	void OnRecvMessage(unsigned int socket, IMessage* pMsg);

private:
	void SendToAllUsers(IMessage* pMsg);

	//	Protocol Handler
	void OnCreateRoomToR(CreateRoomToR* pMsg, unsigned int socket);

private:
	static void OnAccept(void* pLobby, unsigned int socket);
	static void OnRecvMessage(void* pLobby, unsigned int socket, IMessage* pMsg);
};

