#include "stdafx.h"
#include "Network.h"
#include "Accepter.h"
#include "NetworkDefines.h"
#include "IMessage.h"
#include "TestMessage.h"
#include "GameEvent_Move_ToS.h"
#include "ReadyForStartToS.h"
#include "JoinLobbyToS.h"
#include <process.h>

Network::Network(void* pListener, const USHORT nPort)
{
	m_pAccepter = NULL;
	m_hComport = NULL;
	m_bRunning = false;
	m_RecvMessageCallback = NULL;

	m_pListener = pListener;
	m_nPort = nPort;
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

void Network::SetRecvMessageCallback(void(*handler)(void* pListener, SOCKET socket, IMessage* pMsg))
{
	m_RecvMessageCallback = handler;
}

void Network::SetAcceptCallback(void(*handler)(void* pListener, SOCKET socket))
{
	m_AcceptCallback = handler;
}

IMessage* Network::GetIMessage(USHORT nMessageID, char* pChar)
{
	IMessage* pMsg = NULL;

	if (nMessageID == Messages::TEST_MESSAGE_ID)
	{ 
		pMsg = new TestMessage();
	}
	else if (nMessageID == Messages::Ready_For_Start_ToS)
	{
		pMsg = new ReadyForStartToS();
	}
	else if (nMessageID == Messages::Game_Event_Move_ToS)
	{
		pMsg = new GameEvent_Move_ToS();
	}
	else if (nMessageID == Messages::Join_Lobby_ToS)
	{
		pMsg = new JoinLobbyToS();
	}

	if (pMsg != NULL)
	{
		pMsg->Deserialize(pChar);
	}

	return pMsg;
}

void Network::Send(SOCKET socket, IMessage* pMsg)
{
	const char* pCharSerializedData = pMsg->Serialize();
	int nSerializedDSize = strlen(pCharSerializedData);
	int nTotalSize = MESSAGE_HEADER_SIZE + nSerializedDSize;

	char* pCharCopiedData = new char[nTotalSize];

	*pCharCopiedData = pMsg->GetID() >> 8;
	*(pCharCopiedData + 1) = pMsg->GetID() & 0x00FF;
	*(pCharCopiedData + 2) = nTotalSize >> 8;
	*(pCharCopiedData + 3) = nTotalSize & 0x00FF;
	for (int i = 0; i < strlen(pCharSerializedData); ++i)
	{
		*(pCharCopiedData + 4 + i) = *(pCharSerializedData + i);
	}

	LPPER_IO_DATA ioInfo = (LPPER_IO_DATA)malloc(sizeof(PER_IO_DATA));
	memset(&(ioInfo->Overlapped), 0, sizeof(OVERLAPPED));
	ioInfo->WsaBuf.len = nTotalSize;
	ioInfo->WsaBuf.buf = pCharCopiedData;
	ioInfo->Mode = IOMode::Write;

	int nResult = WSASend(socket, &(ioInfo->WsaBuf), 1, NULL, 0, &(ioInfo->Overlapped), NULL);
	if (nResult == SOCKET_ERROR && (WSAGetLastError() != ERROR_IO_PENDING))
	{
		closesocket(socket);
		delete ioInfo->WsaBuf.buf;
		free(ioInfo);
	}

	delete pMsg;
}

#pragma region
void Network::AccepterThread()
{
	while (m_bRunning)
	{
		LPPER_HANDLE_DATA handleInfo = m_pAccepter->Accept();

		CreateIoCompletionPort((HANDLE)handleInfo->Socket, m_hComport, (DWORD)handleInfo, 0);

		LPPER_IO_DATA ioInfo = (LPPER_IO_DATA)malloc(sizeof(PER_IO_DATA));
		memset(&(ioInfo->Overlapped), 0, sizeof(OVERLAPPED));
		ioInfo->WsaBuf.len = BUF_SIZE;
		ioInfo->WsaBuf.buf = new char[BUF_SIZE];
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

		m_AcceptCallback(m_pListener, handleInfo->Socket);
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
					ioInfo->CurMessageID = (USHORT)ioInfo->WsaBuf.buf[i] << 8;
					ioInfo->CurPos++;
				}
				else if (ioInfo->CurPos == 1)
				{
					ioInfo->CurMessageID += (USHORT)ioInfo->WsaBuf.buf[i];
					ioInfo->CurPos++;
				}
				else if (ioInfo->CurPos == 2)
				{
					ioInfo->TotalSize = (USHORT)ioInfo->WsaBuf.buf[i] << 8;
					ioInfo->CurPos++;
				}
				else if (ioInfo->CurPos == 3)
				{
					ioInfo->TotalSize += (USHORT)ioInfo->WsaBuf.buf[i];
					ioInfo->CurMessage = new char[ioInfo->TotalSize - MESSAGE_HEADER_SIZE + 1/*'\0'*/];
					ioInfo->CurPos++;
				}
				else
				{
					//	last index
					if (ioInfo->CurPos == ioInfo->TotalSize - 1)
					{
						ioInfo->CurMessage[ioInfo->CurPos - MESSAGE_HEADER_SIZE] = ioInfo->WsaBuf.buf[i];
						ioInfo->CurMessage[ioInfo->CurPos - MESSAGE_HEADER_SIZE + 1] = '\0';

						if (m_RecvMessageCallback != NULL)
						{
							m_RecvMessageCallback(m_pListener, socket, GetIMessage(ioInfo->CurMessageID, ioInfo->CurMessage));
						}
						
						delete ioInfo->CurMessage;
						ioInfo->CurPos = 0;
					}
					else
					{
						ioInfo->CurMessage[ioInfo->CurPos - MESSAGE_HEADER_SIZE] = ioInfo->WsaBuf.buf[i];
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
			delete ioInfo->WsaBuf.buf;
			free(ioInfo);
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
#pragma endregion Threads