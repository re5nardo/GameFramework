#pragma once
#include <WinSock2.h>
#include "NetworkDefines.h"
#include "../Message/IMessageConvertor.h"
#include "../../CommonSources/Singleton.h"

using namespace std;

class IMessage;
class NetworkCore;

class Network : public Singleton<Network>
{
public:
	Network();
	virtual ~Network();

private:
	NetworkCore*		m_pNetworkCore;
	void*				m_pListener;
	void				(*m_RecvMessageCallback)(void* pListener, SOCKET socket, IMessage* pMsg);
	IMessageConvertor*	m_pMessageConvertor;

public:
	void		Initialize(void* pListener, const USHORT nPort, IMessageConvertor* pMessageConvertor);
	int			Start();
	void		Stop();
	void		SetAcceptCallback(void(*handler)(void* pListener, SOCKET socket));
	void		SetRecvMessageCallback(void(*handler)(void* pListener, SOCKET socket, IMessage* pMsg));
	void		Send(SOCKET socket, IMessage* pMsg, bool bDelete = true, bool bDisposable = false);
	int			Send(const char* pIP, const USHORT nPort, IMessage* pMsg, bool bDelete = true, bool bDisposable = false);

private:
	void		OnRecvMessage(unsigned int socket, LPPER_IO_DATA data);

private:
	static void OnRecvMessage(void* pNetwork, unsigned int socket, LPPER_IO_DATA data);
};