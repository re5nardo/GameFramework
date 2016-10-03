#pragma once

#include <map>
#include <string>
#include "RoomMessageConvertor.h"
#include "../CommonSources/Singleton.h"

class CreateRoomToR;
class GameEventMoveToR;
class BaeGameRoom;
class EnterRoomToR;

using namespace std;

class Room : public Singleton<Room>
{
public:
	Room(const unsigned short nPort);
	virtual ~Room();

private:
	map<string, unsigned int>			m_mapPlayerKeySocket;
	map<unsigned int, string>			m_mapSocketPlayerKey;
	RoomMessageConvertor				m_MessageConvertor;
	map<int, BaeGameRoom*>				m_mapMatchIDGameRoom;
	map<string, BaeGameRoom*>			m_mapPlayerKeyGameRoom;

private:
	void OnAccept(unsigned int socket);
	void OnRecvMessage(unsigned int socket, IMessage* pMsg);

private:
	//	Protocol Handler
	void OnCreateRoomToR(CreateRoomToR* pMsg, unsigned int socket);
	void OnEnterRoomToR(EnterRoomToR* pMsg, unsigned int socket);

private:
	static void OnAccept(void* pLobby, unsigned int socket);
	static void OnRecvMessage(void* pLobby, unsigned int socket, IMessage* pMsg);
};

