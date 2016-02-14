#pragma once

class Network;
class IMessage;

class Lobby
{
public:
	Lobby(const unsigned short nPort);
	virtual ~Lobby();

private:
	Network*	m_pNetwork;

private:
	void OnRecvMessage(unsigned int socket, IMessage* pMsg);

private:
	static void OnRecvMessage(void* pLobby, unsigned int socket, IMessage* pMsg);
};

