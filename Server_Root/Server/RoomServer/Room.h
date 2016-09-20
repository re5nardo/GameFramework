#pragma once

#include <map>
#include <string>
#include "RoomMessageConvertor.h"
#include "RoomMessageHeader.h"
#include "../CommonSources/Singleton.h"

class CreateRoomToR;
class GameEventMoveToS;
class BaeGameRoom;

using namespace std;

class Room : public Singleton<Room>
{
public:
	Room(const unsigned short nPort);
	virtual ~Room();

private:
	map<string, unsigned int>			m_mapPlayerSocket;
	map<unsigned int, string>			m_mapSocketPlayer;
	RoomMessageConvertor				m_MessageConvertor;
	map<int, BaeGameRoom*>				m_mapMatchIDGameRoom;
	map<string, BaeGameRoom*>			m_mapPlayerGameRoom;

private:
	void OnAccept(unsigned int socket);
	void OnRecvMessage(unsigned int socket, IMessage* pMsg);

private:
	void SendToAllUsers(IMessage* pMsg);

	//	Protocol Handler
	void OnCreateRoomToR(CreateRoomToR* pMsg, unsigned int socket);
	void OnGameEventMoveToS(GameEventMoveToS* pMsg, unsigned int socket);
	void OnEnterRoomToS(EnterRoomToS* pMsg, unsigned int socket);

private:
	static void OnAccept(void* pLobby, unsigned int socket);
	static void OnRecvMessage(void* pLobby, unsigned int socket, IMessage* pMsg);
};

