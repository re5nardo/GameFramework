#pragma once
#include <WinSock2.h>
#include <string>
#include "Defines.h"

using namespace std;

class IMessage;
class Accepter;

class Network
{
public:
	Network(const USHORT nPort);
	virtual ~Network();

private:
	Accepter*		m_pAccepter;
	HANDLE			m_hComport;
	bool			m_bRunning;
	USHORT			m_nPort;
	void*			m_pListener;
	void			(*m_RecvMessageCallback)(void* pListener, SOCKET socket, IMessage* pMsg);

public:
	int			Start();
	void		Stop();
	void		SetRecvMessageCallback(void* pListener, void(*handler)(void* pListener, SOCKET socket, IMessage* pMsg));
	void		Send(SOCKET socket, IMessage* pMsg);

private:
	IMessage*	GetIMessage(USHORT nMessageID, string strJson);

private:
	void		AccepterThread();
	void		WorkerThread();

private:
	static UINT WINAPI AccepterThreadStart(void* param);
	static UINT WINAPI WorkerThreadStart(void* param);
};