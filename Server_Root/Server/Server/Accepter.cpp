#include "stdafx.h"
#include "Accepter.h"
#include <Ws2tcpip.h>

Accepter::Accepter(const USHORT nPort)
{
	m_nPort = nPort;
	m_Socket = 0;
	m_bListening = false;
}

Accepter::~Accepter()
{
	if (m_Socket > 0)
	{
		closesocket(m_Socket);
	}
}

int Accepter::Start()
{
	if (m_bListening )
	{
		return 0;
	}

	m_Socket = WSASocketW(AF_INET, SOCK_STREAM, 0, NULL, 0, WSA_FLAG_OVERLAPPED);
	if (m_Socket == INVALID_SOCKET)
	{
		perror("socket() error!");
		return -1;
	}

	sockaddr_in address;
	memset(&address, 0, sizeof(address));
	address.sin_family = AF_INET;
	address.sin_addr.s_addr = INADDR_ANY;
	address.sin_port = htons((m_nPort));

	int nResult = bind(m_Socket, (SOCKADDR*)&address, sizeof(address));
	if (nResult == SOCKET_ERROR)
	{
		perror("bind() error!");
		return nResult;
	}

	nResult = listen(m_Socket, 5);
	if (nResult == SOCKET_ERROR)
	{
		perror("listen() error!");
		return nResult;
	}

	m_bListening = true;

	return nResult;
}

LPPER_HANDLE_DATA Accepter::Accept()
{
	if (m_bListening == false)
	{
		return 0;
	}

	SOCKET hClntSock;
	SOCKADDR_IN clntAdr;
	int addrLen = sizeof(clntAdr);
	hClntSock = accept(m_Socket, (SOCKADDR*)&clntAdr, &addrLen);

	LPPER_HANDLE_DATA handleInfo = (LPPER_HANDLE_DATA)malloc(sizeof(PER_HANDLE_DATA));
	handleInfo->Socket = hClntSock;
	memcpy(&(handleInfo->Address), &clntAdr, addrLen);

	return handleInfo;
}