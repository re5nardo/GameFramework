
#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include <process.h>
#include <string.h>
#include <winsock2.h>
#include <windows.h>

#include "TestMessage.h"

#define BUF_SIZE	100
#define READ		3
#define WRITE		5

typedef struct
{
	SOCKET hclntSock;
	SOCKADDR_IN clntAdr;
}PER_HANDLE_DATA, *LPPER_HANDLE_DATA;

typedef struct
{
	OVERLAPPED overlapped;
	WSABUF wsaBuf;
	char buffer[BUF_SIZE];
	int rwMode;
}PER_IO_DATA, *LPPER_IO_DATA;

unsigned int WINAPI EchoThreadMain(LPVOID CompletionPortIO);
void ErrorHandling(char* message);
void ClearSingltons();

int _tmain(int argc, _TCHAR* argv[])
{
	WSADATA wsaData;
	HANDLE hComport;
	SYSTEM_INFO sysInfo;
	LPPER_IO_DATA ioInfo;
	LPPER_HANDLE_DATA handleInfo;
	SOCKET hServSock;
	SOCKADDR_IN servAdr;
	int i = 0;
	DWORD recvBytes, flags = 0;

	if (argc != 2)
	{
		printf("Usage : %s <port>\n", argv[0]);
		exit(0);
	}

	if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0)
		ErrorHandling("WSAStartup() error!");

	hServSock = socket(PF_INET, SOCK_STREAM, 0);
	if (hServSock == INVALID_SOCKET)
		ErrorHandling("socket() error!");

	hComport = CreateIoCompletionPort(INVALID_HANDLE_VALUE, NULL, 0, 0);
	GetSystemInfo(&sysInfo);
	for (i = 0; i < sysInfo.dwNumberOfProcessors; ++i)
	{
		_beginthreadex(NULL, 0, EchoThreadMain, (LPVOID)hComport, 0, NULL);
	}

	hServSock = WSASocketW(AF_INET, SOCK_STREAM, 0, NULL, 0, WSA_FLAG_OVERLAPPED);
	memset(&servAdr, 0, sizeof(servAdr));
	servAdr.sin_family = AF_INET;
	servAdr.sin_addr.s_addr = htonl(INADDR_ANY);
	servAdr.sin_port = htons(atoi(argv[1]));

	if (bind(hServSock, (SOCKADDR*)&servAdr, sizeof(servAdr)) == SOCKET_ERROR)
		ErrorHandling("bind() error!");

	if (listen(hServSock, 5) == SOCKET_ERROR)
		ErrorHandling("listen() error!");
	
	while (1)
	{
		SOCKET hClntSock;
		SOCKADDR_IN clntAdr;
		int addrLen = sizeof(clntAdr);
		hClntSock = accept(hServSock, (SOCKADDR*)&clntAdr, &addrLen);
		handleInfo = (LPPER_HANDLE_DATA)malloc(sizeof(PER_HANDLE_DATA));
		handleInfo->hclntSock = hClntSock;
		memcpy(&(handleInfo->clntAdr), &clntAdr, addrLen);

		CreateIoCompletionPort((HANDLE)hClntSock, hComport, (DWORD)handleInfo, 0);

		ioInfo = (LPPER_IO_DATA)malloc(sizeof(PER_IO_DATA));
		memset(&(ioInfo->overlapped), 0, sizeof(OVERLAPPED));
		ioInfo->wsaBuf.len = BUF_SIZE;
		ioInfo->rwMode = READ;
		WSARecv(handleInfo->hclntSock, &(ioInfo->wsaBuf), 1, &recvBytes, &flags, &(ioInfo->overlapped), NULL);
	}

	//closesocket(hClntSock);
	closesocket(hServSock);
	WSACleanup();

	ClearSingltons();

	return 0;
}


unsigned int WINAPI EchoThreadMain(LPVOID CompletionPortIO)
{
	HANDLE hComPort = (HANDLE)CompletionPortIO;
	SOCKET sock;
	DWORD bytesTrans;
	LPPER_HANDLE_DATA handleInfo;
	LPPER_IO_DATA ioInfo;
	DWORD flags = 0;

	while (1)
	{
		GetQueuedCompletionStatus(hComPort, &bytesTrans, (LPDWORD)&handleInfo, (LPOVERLAPPED*)&ioInfo, INFINITE);
		sock = handleInfo->hclntSock;

		if (ioInfo->rwMode == READ)
		{
			puts("message received!");
			if (bytesTrans == 0)	//	EOF
			{
				closesocket(sock);
				free(handleInfo);
				free(ioInfo);
				continue;
			}

			memset(&(ioInfo->overlapped), 0, sizeof(OVERLAPPED));
			ioInfo->wsaBuf.len = bytesTrans;
			ioInfo->rwMode = WRITE;
			WSASend(sock, &(ioInfo->wsaBuf), 1, NULL, 0, &(ioInfo->overlapped), NULL);

			ioInfo = (LPPER_IO_DATA)malloc(sizeof(PER_IO_DATA));
			memset(&(ioInfo->overlapped), 0, sizeof(OVERLAPPED));
			ioInfo->wsaBuf.len = BUF_SIZE;
			ioInfo->wsaBuf.buf = ioInfo->buffer;
			ioInfo->rwMode = READ;
			WSARecv(sock, &(ioInfo->wsaBuf), 1, NULL, &flags, &(ioInfo->overlapped), NULL);
		}
		else
		{
			puts("message sent!");
			free(ioInfo);
		}
	}

	return 0;
}


/*
unsigned short messageID;
unsigned short nLength;
char opinfo[BUF_SIZE];
recv(hClntSock, (char*)&messageID, 2, 0);
recv(hClntSock, (char*)&nLength, 2, 0);

recvLen = 0;
while (nLength > recvLen)
{
recvCnt = recv(hClntSock, &opinfo[recvLen], BUFSIZ - 1, 0);
recvLen += recvCnt;
}*/

void ErrorHandling(char* message)
{
	fputs(message, stderr);
	fputc('\n', stderr);
	exit(1);
}

void ClearSingltons()
{
}