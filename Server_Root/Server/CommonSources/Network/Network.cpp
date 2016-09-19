#include "stdafx.h"
#include "Network.h"
#include "NetworkCore.h"
#include "NetworkDefines.h"
#include "../Message/IMessage.h"
#include <process.h>
#include <WS2tcpip.h>

Network::Network()
{

}

Network::~Network()
{
	if (m_pNetworkCore != NULL)
	{
		delete m_pNetworkCore;
	}
}

void Network::Initialize(void* pListener, const USHORT nPort, IMessageConvertor* pMessageConvertor)
{
	m_pNetworkCore = new NetworkCore(this, nPort);
	m_pNetworkCore->SetRecvMessageCallback(OnRecvMessage);
	m_RecvMessageCallback = NULL;

	m_pListener = pListener;
	m_pMessageConvertor = pMessageConvertor;
}

int Network::Start()
{
	return m_pNetworkCore->Start();
}

void Network::Stop()
{
	m_pNetworkCore->Stop();
}

void Network::SetRecvMessageCallback(void(*handler)(void* pListener, SOCKET socket, IMessage* pMsg))
{
	m_RecvMessageCallback = handler;
}

void Network::SetAcceptCallback(void(*handler)(void* pListener, SOCKET socket))
{
	m_pNetworkCore->SetAcceptCallback(handler);
}

void Network::Send(SOCKET socket, IMessage* pMsg, bool bDelete, bool bDisposable)
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

	m_pNetworkCore->Send(socket, pCharCopiedData, nTotalSize, bDisposable);

	if (bDelete)
		delete pMsg;
}

int Network::Send(const char* pIP, const USHORT nPort, IMessage* pMsg, bool bDisposable)
{
	SOCKET hSocket = WSASocketW(PF_INET, SOCK_STREAM, 0, NULL, 0, WSA_FLAG_OVERLAPPED);
	if (hSocket == INVALID_SOCKET)
	{
		perror("socket() error!");
		return -1;
	}

	sockaddr_in servAddr;
	memset(&servAddr, 0, sizeof(servAddr));
	servAddr.sin_family = AF_INET;
	inet_pton(AF_INET, pIP, &servAddr.sin_addr.s_addr);
	servAddr.sin_port = htons(nPort);

	int nResult = connect(hSocket, (SOCKADDR*)&servAddr, sizeof(servAddr));
	if (nResult == SOCKET_ERROR)
	{
		perror("connect() error");
		return nResult;
	}

	LPPER_HANDLE_DATA handleInfo = (LPPER_HANDLE_DATA)malloc(sizeof(PER_HANDLE_DATA));
	handleInfo->Socket = hSocket;
	memcpy(&(handleInfo->Address), &servAddr, sizeof(servAddr));

	CreateIoCompletionPort((HANDLE)handleInfo->Socket, m_pNetworkCore->GetCompletionPortHandle(), (DWORD)handleInfo, 0);

	Send(hSocket, pMsg, true, bDisposable);

	LPPER_IO_DATA ioInfo = (LPPER_IO_DATA)malloc(sizeof(PER_IO_DATA));
	memset(&(ioInfo->Overlapped), 0, sizeof(OVERLAPPED));
	ioInfo->WsaBuf.len = BUF_SIZE;
	ioInfo->WsaBuf.buf = new char[BUF_SIZE];
	ioInfo->Mode = IOMode::Read;
	ioInfo->Disposable = false;
	ioInfo->CurPos = 0;
	DWORD flags = 0;

	nResult = WSARecv(hSocket, &(ioInfo->WsaBuf), 1, NULL, &flags, &(ioInfo->Overlapped), NULL);
	if (nResult == SOCKET_ERROR && (WSAGetLastError() != ERROR_IO_PENDING))
	{
		closesocket(handleInfo->Socket);
		free(handleInfo);
		delete ioInfo->WsaBuf.buf;
		free(ioInfo);
	}

	return 0;
}

void Network::OnRecvMessage(unsigned int socket, LPPER_IO_DATA data)
{
	m_RecvMessageCallback(m_pListener, socket, m_pMessageConvertor->GetMessage(data->CurMessageID, data->CurMessage));
}


//	Static Functions for Callback
void Network::OnRecvMessage(void* pNetwork, unsigned int socket, LPPER_IO_DATA data)
{
	if (pNetwork == NULL || data == NULL)
	{
		return;
	}

	((Network*)pNetwork)->OnRecvMessage(socket, data);
}