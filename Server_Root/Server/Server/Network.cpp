#include "stdafx.h"
#include "Network.h"
#include "Accepter.h"
#include "NetworkDefines.h"
#include "IMessage.h"
#include "TestMessage.h"
#include <process.h>

Network::Network(const USHORT nPort)
{
	m_pAccepter = NULL;
	m_hComport = NULL;
	m_bRunning = false;
	m_nPort = nPort;
	m_RecvMessageCallback = NULL;
}

Network::~Network()
{
	if (m_pAccepter != NULL)
	{
		delete m_pAccepter;
	}
}

int Network::Start()
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

void Network::Stop()
{
	m_bRunning = false;

	WSACleanup();
}

void Network::SetRecvMessageCallback(void(*handler)(SOCKET socket, IMessage* pMsg))
{
	m_RecvMessageCallback = handler;
}

IMessage* Network::GetIMessage(USHORT nMessageID, string strJson)
{
	IMessage* pMsg = NULL;

	if (nMessageID == Messages::TEST_MESSAGE_ID)
	{
		pMsg = new TestMessage();
	}

	if (pMsg != NULL)
	{
		pMsg->Deserialize(strJson);
	}

	return pMsg;
}

void Network::AccepterThread()
{
	while (m_bRunning)
	{
		LPPER_HANDLE_DATA handleInfo = m_pAccepter->Accept();

		CreateIoCompletionPort((HANDLE)handleInfo->Socket, m_hComport, (DWORD)handleInfo, 0);

		LPPER_IO_DATA ioInfo = (LPPER_IO_DATA)malloc(sizeof(PER_IO_DATA));
		memset(&(ioInfo->Overlapped), 0, sizeof(OVERLAPPED));
		ioInfo->WsaBuf.len = BUF_SIZE;
		ioInfo->WsaBuf.buf = (char*)ioInfo->Buffer;
		ioInfo->Mode = IOMode::Read;
		ioInfo->CurPos = 0;
		DWORD flags = 0;

		int nResult = WSARecv(handleInfo->Socket, &(ioInfo->WsaBuf), 1, NULL, &flags, &(ioInfo->Overlapped), NULL);
		if (nResult == SOCKET_ERROR && (WSAGetLastError() != ERROR_IO_PENDING))
		{
			closesocket(handleInfo->Socket);
			free(handleInfo);
			free(ioInfo);
		}
	}
}

void Network::WorkerThread()
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
				free(ioInfo);

				continue;
			}

			for (int i = 0; i < bytesTrans; ++i)
			{
				if (ioInfo->CurPos == 0)
				{
					ioInfo->CurMessageID = (UINT)ioInfo->Buffer[i] << 8;
					ioInfo->CurPos++;
				}
				else if (ioInfo->CurPos == 1)
				{
					ioInfo->CurMessageID += (UINT)ioInfo->Buffer[i];
					ioInfo->CurPos++;
				}
				else if (ioInfo->CurPos == 2)
				{
					ioInfo->TotalSize = (UINT)ioInfo->Buffer[i] << 8;
					ioInfo->CurPos++;
				}
				else if (ioInfo->CurPos == 3)
				{
					ioInfo->TotalSize += (UINT)ioInfo->Buffer[i];
					ioInfo->CurMessage = new char[ioInfo->TotalSize];
					ioInfo->CurPos++;
				}
				else
				{
					//	last index
					if (ioInfo->CurPos == ioInfo->TotalSize - 1)
					{
						ioInfo->CurMessage[ioInfo->CurPos - MESSAGE_HEADER_SIZE] = ioInfo->Buffer[i];

						if (m_RecvMessageCallback != NULL)
						{
							m_RecvMessageCallback(socket, GetIMessage(ioInfo->CurMessageID, ioInfo->CurMessage));
						}
						
						delete ioInfo->CurMessage;
						ioInfo->CurPos = 0;
					}
					else
					{
						ioInfo->CurMessage[ioInfo->CurPos - MESSAGE_HEADER_SIZE] = ioInfo->Buffer[i];
						ioInfo->CurPos++;
					}
				}
			}

			int nResult = WSARecv(socket, &(ioInfo->WsaBuf), 1, NULL, &flags, &(ioInfo->Overlapped), NULL);
			if (nResult == SOCKET_ERROR && (WSAGetLastError() != ERROR_IO_PENDING))
			{
				closesocket(handleInfo->Socket);
				free(handleInfo);
				free(ioInfo);
			}
		}
		else
		{
			//puts("message sent!");
			//free(ioInfo);
		}
	}
}

UINT WINAPI Network::AccepterThreadStart(void* param)
{
	Network* pNetwork = (Network*)param;

	pNetwork->AccepterThread();

	return 0;
}

UINT WINAPI Network::WorkerThreadStart(void* param)
{
	Network* pNetwork = (Network*)param;

	pNetwork->WorkerThread();

	return 0;
}