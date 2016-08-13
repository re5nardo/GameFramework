#pragma once

#include <WinSock2.h>

enum Messages
{
	TEST_MESSAGE_ID = 0,

	//  ToS
	ReadyForStartToS_ID = 0,
	GameEventMoveToS_ID,
	JoinLobbyToS_ID,

	//  ToC
	GameStartToC_ID = 30000,
	GameEventMoveToC_ID,
	JoinLobbyToC_ID,
};

#define BUF_SIZE				256
#define MESSAGE_HEADER_SIZE		4		//	msg id(2) + msg length info(2)

enum IOMode
{
	Read = 0,
	Write,
};

typedef struct
{
	SOCKET			Socket;
	SOCKADDR_IN		Address;
}PER_HANDLE_DATA, *LPPER_HANDLE_DATA;

typedef struct
{
	OVERLAPPED		Overlapped;
	WSABUF			WsaBuf;
	USHORT			CurMessageID;
	USHORT			CurPos;
	USHORT			TotalSize;
	char*			CurMessage;
	IOMode			Mode;
}PER_IO_DATA, *LPPER_IO_DATA;