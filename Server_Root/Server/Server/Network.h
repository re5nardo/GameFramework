#pragma once
#include <WinSock2.h>
#include "NetworkDefines.h"
#include "IMessageConvertor.h"

using namespace std;

class IMessage;
class NetworkCore;

class Network
{
public:
	Network(void* pListener, const USHORT nPort, IMessageConvertor* pMessageConvertor);
	virtual ~Network();

private:
	NetworkCore*		m_pNetworkCore;
	void*				m_pListener;
	void				(*m_RecvMessageCallback)(void* pListener, SOCKET socket, IMessage* pMsg);
	IMessageConvertor*	m_pMessageConvertor;

public:
	int			Start();
	void		Stop();
	void		SetAcceptCallback(void(*handler)(void* pListener, SOCKET socket));
	void		SetRecvMessageCallback(void(*handler)(void* pListener, SOCKET socket, IMessage* pMsg));
	void		Send(SOCKET socket, IMessage* pMsg, bool bDelete = true);

private:
	void		OnRecvMessage(unsigned int socket, LPPER_IO_DATA data);

private:
	static void OnRecvMessage(void* pNetwork, unsigned int socket, LPPER_IO_DATA data);
};