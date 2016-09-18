#pragma once

#include <map>
#include <string>
#include "LobbyMessageConvertor.h"

class Network;
class IMessage;
class JoinLobbyToS;
class SelectNormalGameToS;
class CreateRoomToL;

using namespace std;

class Lobby
{
public:
	Lobby(const unsigned short nPort);
	virtual ~Lobby();

private:
	Network*							m_pNetwork;
	map<string, unsigned int>			m_mapPlayerSocket;
	map<unsigned int, string>			m_mapSocketPlayer;
	LobbyMessageConvertor				m_MessageConvertor;

private:
	void OnAccept(unsigned int socket);
	void OnRecvMessage(unsigned int socket, IMessage* pMsg);

private:
	void SendToAllUsers(IMessage* pMsg);

	//	Protocol Handler
	void OnJoinLobbyToS(JoinLobbyToS* pMsg, unsigned int socket);
	void OnSelectNormalGameToS(SelectNormalGameToS* pMsg, unsigned int socket);
	void OnCreateRoomToL(CreateRoomToL* pMsg, unsigned int socket);

private:
	static void OnAccept(void* pLobby, unsigned int socket);
	static void OnRecvMessage(void* pLobby, unsigned int socket, IMessage* pMsg);
};

