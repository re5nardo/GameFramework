#pragma once
#include <WinSock2.h>
#include "NetworkDefines.h"

using namespace std;

class Accepter;

class NetworkCore
{
public:
	NetworkCore(void* pListener, const USHORT nPort);
	virtual ~NetworkCore();

private:
	Accepter*		m_pAccepter;
	HANDLE			m_hComport;
	bool			m_bRunning;
	USHORT			m_nPort;
	void*			m_pListener;
	void			(*m_AcceptCallback)(void* pListener, SOCKET socket);
	void			(*m_RecvMessageCallback)(void* pListener, SOCKET socket, LPPER_IO_DATA data);

public:
	int			Start();
	void		Stop();
	void		SetAcceptCallback(void(*handler)(void* pListener, SOCKET socket));
	void		SetRecvMessageCallback(void(*handler)(void* pListener, SOCKET socket, LPPER_IO_DATA data));
	void		Send(SOCKET socket, char* pChar, int nLength, bool bDisposable = false);
	HANDLE		GetCompletionPortHandle();

private:
	void		AccepterThread();
	void		WorkerThread();

private:
	static UINT WINAPI AccepterThreadStart(void* param);
	static UINT WINAPI WorkerThreadStart(void* param);
};