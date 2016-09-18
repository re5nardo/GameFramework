#include "stdafx.h"
#include "RoomMessageConvertor.h"
#include "RoomMessageHeader.h"

RoomMessageConvertor::RoomMessageConvertor()
{
}


RoomMessageConvertor::~RoomMessageConvertor()
{
}

IMessage* RoomMessageConvertor::GetMessage(const unsigned short nMessageID, const char* pChar)
{
	IMessage* pMsg = NULL;

	if (nMessageID == CreateRoomToR::MESSAGE_ID)
	{
		pMsg = new CreateRoomToR();
	}

	if (pMsg != NULL)
	{
		pMsg->Deserialize(pChar);
	}

	return pMsg;
}

