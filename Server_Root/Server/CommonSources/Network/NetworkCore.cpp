#include "stdafx.h"
#include "NetworkCore.h"
#include "Accepter.h"
#include "NetworkDefines.h"
#include <process.h>

NetworkCore::NetworkCore(void* pListener, const USHORT nPort)
{
	m_pAccepter = NULL;
	m_hComport = NULL;
	m_bRunning = false;
	m_AcceptCallback = NULL;
	m_RecvMessageCallback = NULL;

	m_pListener = pListener;
	m_nPort = nPort;
}

NetworkCore::~NetworkCore()
{
	if (m_pAccepter != NULL)
	{
		delete m_pAccepter;
	}
}

int NetworkCore::Start()
{
	WSADATA wsaData;
	int nResult = WSAStartup(MAKEWORD(2, 2), &wsaData);
	if (nResult != 0)
	{
		perror("WSAStartup() error!");
		return nResult;
	}

	m_bRunning = true;
	m_hComport = CreateIoCompletionPort(INVALID_HANDLE_VALUE, NULL, 0, 0);

	SYSTEM_INFO sysInfo;
	GetSystemInfo(&sysInfo);
	for (int i = 0; i < sysInfo.dwNumberOfProcessors; ++i)
	{
		_beginthreadex(NULL, 0, WorkerThreadStart, this, 0, NULL);
	}

	m_pAccepter = new Accepter(m_nPort);
	m_pAccepter->Start();

	_beginthreadex(NULL, 0, AccepterThreadStart, this, 0, NULL);

	return 0;
}

void NetworkCore::Stop()
{
	m_bRunning = false;

	WSACleanup();
}

void NetworkCore::SetRecvMessageCallback(void(*handler)(void* pListener, SOCKET socket, LPPER_IO_DATA data))
{
	m_RecvMessageCallback = handler;
}

void NetworkCore::SetAcceptCallback(void(*handler)(void* pListener, SOCKET socket))
{
	m_AcceptCallback = handler;
}

void NetworkCore::Send(SOCKET socket, char* pChar, int nLength, bool bDisposable)
{
	LPPER_IO_DATA ioInfo = (LPPER_IO_DATA)malloc(sizeof(PER_IO_DATA));
	memset(&(ioInfo->Overlapped), 0, sizeof(OVERLAPPED));
	ioInfo->WsaBuf.len = nLength;
	ioInfo->WsaBuf.buf = pChar;
	ioInfo->Mode = IOMode::Write;
	ioInfo->Disposable = bDisposable;

	int nResult = WSASend(socket, &(ioInfo->WsaBuf), 1, NULL, 0, &(ioInfo->Overlapped), NULL);
	if (nResult == SOCKET_ERROR && (WSAGetLastError() != ERROR_IO_PENDING))
	{
		closesocket(socket);
		delete ioInfo->WsaBuf.buf;
		free(ioInfo);
	}
}

HANDLE NetworkCore::GetCompletionPortHandle()
{
	return m_hComport;
}

#pragma region
void NetworkCore::AccepterThread()
{
	while (m_bRunning)
	{
		LPPER_HANDLE_DATA handleInfo = m_pAccepter->Accept();

		CreateIoCompletionPort((HANDLE)handleInfo->Socket, m_hComport, (DWORD)handleInfo, 0);

		LPPER_IO_DATA ioInfo = (LPPER_IO_DATA)malloc(sizeof(PER_IO_DATA));
		memset(&(ioInfo->Overlapped), 0, sizeof(OVERLAPPED));
		ioInfo->WsaBuf.len = WSA_BUF_SIZE;
		ioInfo->WsaBuf.buf = new char[WSA_BUF_SIZE];
		ioInfo->Mode = IOMode::Read;
		ioInfo->Disposable = false;
		ioInfo->CurPos = 0;
		DWORD flags = 0;

		int nResult = WSARecv(handleInfo->Socket, &(ioInfo->WsaBuf), 1, NULL, &flags, &(ioInfo->Overlapped), NULL);
		if (nResult == SOCKET_ERROR && (WSAGetLastError() != ERROR_IO_PENDING))
		{
			closesocket(handleInfo->Socket);
			free(handleInfo);
			delete ioInfo->WsaBuf.buf;
			free(ioInfo);
		}

		m_AcceptCallback(m_pListener, handleInfo->Socket);
	}
}

void NetworkCore::WorkerThread()
{
	SOCKET socket;
	DWORD bytesTrans;
	LPPER_HANDLE_DATA handleInfo = NULL;
	LPPER_IO_DATA ioInfo = NULL;
	DWORD flags = 0;

	while (m_bRunning)
	{
		GetQueuedCompletionStatus(m_hComport, &bytesTrans, (LPDWORD)&handleInfo, (LPOVERLAPPED*)&ioInfo, INFINITE);
		socket = handleInfo->Socket;

		if (ioInfo->Mode == IOMode::Read)
		{
			puts("message received!");
			if (bytesTrans == 0)	//	EOF
			{
				closesocket(socket);
				free(handleInfo);
				delete ioInfo->WsaBuf.buf;
				free(ioInfo);

				continue;
			}

			for (int i = 0; i < bytesTrans; ++i)
			{
				if (ioInfo->CurPos == 0)
				{
					ioInfo->CurMessageID = (unsigned char)ioInfo->WsaBuf.buf[i] << 8;
					ioInfo->CurPos++;
				}
				else if (ioInfo->CurPos == 1)
				{
					ioInfo->CurMessageID += (unsigned char)ioInfo->WsaBuf.buf[i];
					ioInfo->CurPos++;
				}
				else if (ioInfo->CurPos == 2)
				{
					ioInfo->TotalSize = (unsigned char)ioInfo->WsaBuf.buf[i] << 8;
					ioInfo->CurPos++;
				}
				else if (ioInfo->CurPos == 3)
				{
					ioInfo->TotalSize += (unsigned char)ioInfo->WsaBuf.buf[i];
					ioInfo->CurMessage = new char[ioInfo->TotalSize - MESSAGE_HEADER_SIZE];
					ioInfo->CurPos++;
				}
				else
				{
					ioInfo->CurMessage[ioInfo->CurPos - MESSAGE_HEADER_SIZE] = ioInfo->WsaBuf.buf[i];

					//	last index
					if (ioInfo->CurPos == ioInfo->TotalSize - 1)
					{
						if (m_RecvMessageCallback != NULL)
						{
							m_RecvMessageCallback(m_pListener, socket, ioInfo);
						}

						delete ioInfo->CurMessage;
						ioInfo->CurPos = 0;
					}
					else
					{
						ioInfo->CurPos++;
					}
				}
			}

			int nResult = WSARecv(socket, &(ioInfo->WsaBuf), 1, NULL, &flags, &(ioInfo->Overlapped), NULL);
			if (nResult == SOCKET_ERROR && (WSAGetLastError() != ERROR_IO_PENDING))
			{
				closesocket(handleInfo->Socket);
				free(handleInfo);
				delete ioInfo->WsaBuf.buf;
				free(ioInfo);
			}
		}
		else
		{
			puts("message sent!");

			if (ioInfo->Disposable)
			{
				closesocket(handleInfo->Socket);
			}

			delete ioInfo->WsaBuf.buf;
			free(ioInfo);
		}
	}
}

UINT WINAPI NetworkCore::AccepterThreadStart(void* param)
{
	NetworkCore* pNetworkCore = (NetworkCore*)param;

	pNetworkCore->AccepterThread();

	return 0;
}

UINT WINAPI NetworkCore::WorkerThreadStart(void* param)
{
	NetworkCore* pNetworkCore = (NetworkCore*)param;

	pNetworkCore->WorkerThread();

	return 0;
}
#pragma endregion Threads