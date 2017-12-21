#pragma once

#include <map>
#include <string>
#include <queue>
#include "LobbyMessageConvertor.h"

class IMessage;
class JoinLobbyToL;
class SelectNormalGameToL;
class CreateRoomToL;

using namespace std;

class Lobby
{
public:
	Lobby(const unsigned short nPort);
	virtual ~Lobby();

private:
	map<string, unsigned int>			m_mapPlayerKeySocket;
	map<unsigned int, string>			m_mapSocketPlayerKey;
	LobbyMessageConvertor				m_MessageConvertor;

private:
	queue<string>	m_queueMatchingPool;

private:
	string GetRoomServerIP();		//	temp..
	int GetRoomServerPort();		//	temp..
	int GetMatchingPlayerCount();	//	temp..

private:
	void OnAccept(unsigned int socket);
	void OnRecvMessage(unsigned int socket, IMessage* pMsg);

private:
	void SendToAllUsers(IMessage* pMsg);

	//	Protocol Handler
	void OnJoinLobbyToL(JoinLobbyToL* pMsg, unsigned int socket);
	void OnSelectNormalGameToL(SelectNormalGameToL* pMsg, unsigned int socket);
	void OnCreateRoomToL(CreateRoomToL* pMsg, unsigned int socket);

private:
	static void OnAccept(void* pLobby, unsigned int socket);
	static void OnRecvMessage(void* pLobby, unsigned int socket, IMessage* pMsg);
};

