#include "stdafx.h"
#include "Network.h"
#include "NetworkCore.h"
#include "NetworkDefines.h"
#include "IMessage.h"
#include <process.h>

Network::Network(void* pListener, const USHORT nPort, IMessageConvertor* pMessageConvertor)
{
	m_pNetworkCore = new NetworkCore(this, nPort);
	m_pNetworkCore->SetRecvMessageCallback(OnRecvMessage);
	m_RecvMessageCallback = NULL;

	m_pListener = pListener;
	m_pMessageConvertor = pMessageConvertor;
}

Network::~Network()
{
	if (m_pNetworkCore != NULL)
	{
		delete m_pNetworkCore;
	}
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

void Network::Send(SOCKET socket, IMessage* pMsg, bool bDelete)
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

	if (bDelete)
		delete pMsg;
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