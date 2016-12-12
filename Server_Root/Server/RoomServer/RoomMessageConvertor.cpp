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
	else if (nMessageID == GameEventMoveToR::MESSAGE_ID)
	{
		pMsg = new GameEventMoveToR();
	}
	else if (nMessageID == EnterRoomToR::MESSAGE_ID)
	{
		pMsg = new EnterRoomToR();
	}
	else if (nMessageID == PreparationStateToR::MESSAGE_ID)
	{
		pMsg = new PreparationStateToR();
	}
	else if (nMessageID == GameEventIdleToR::MESSAGE_ID)
	{
		pMsg = new GameEventIdleToR();
	}
	else if (nMessageID == GameEventStopToR::MESSAGE_ID)
	{
		pMsg = new GameEventStopToR();
	}

	if (pMsg != NULL)
	{
		pMsg->Deserialize(pChar);
	}

	return pMsg;
}

