#pragma once

#include <map>
#include <string>

class Network;
class IMessage;
class JoinLobbyToS;

using namespace std;

class Lobby
{
public:
	Lobby(const unsigned short nPort);
	virtual ~Lobby();

private:
	Network*							m_pNetwork;
	map<string, unsigned int>			m_mapPlayer;

private:
	void OnAccept(unsigned int socket);
	void OnRecvMessage(unsigned int socket, IMessage* pMsg);

private:
	void SendToAllUsers(IMessage* pMsg);

	//	Protocol Handler
	void OnJoinLobbyToS(JoinLobbyToS* pMsg, unsigned int socket);

private:
	static void OnAccept(void* pLobby, unsigned int socket);
	static void OnRecvMessage(void* pLobby, unsigned int socket, IMessage* pMsg);
};

