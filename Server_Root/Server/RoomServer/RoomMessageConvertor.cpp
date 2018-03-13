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
	else if (nMessageID == GameEventRunToR::MESSAGE_ID)
	{
		pMsg = new GameEventRunToR();
	}
	else if (nMessageID == EnterRoomToR::MESSAGE_ID)
	{
		pMsg = new EnterRoomToR();
	}
	else if (nMessageID == PreparationStateToR::MESSAGE_ID)
	{
		pMsg = new PreparationStateToR();
	}
	else if (nMessageID == GameEventStopToR::MESSAGE_ID)
	{
		pMsg = new GameEventStopToR();
	}
	else if (nMessageID == GameInputSkillToR::MESSAGE_ID)
	{
		pMsg = new GameInputSkillToR();
	}
	else if (nMessageID == GameInputMoveToR::MESSAGE_ID)
	{
		pMsg = new GameInputMoveToR();
	}
	else if (nMessageID == GameInputRotationToR::MESSAGE_ID)
	{
		pMsg = new GameInputRotationToR();
	}
	else if (nMessageID == PlayerInputToR::MESSAGE_ID)
	{
		pMsg = new PlayerInputToR();
	}
	else if (nMessageID == GameResultToR::MESSAGE_ID)
	{
		pMsg = new GameResultToR();
	}

	if (pMsg != NULL)
	{
		pMsg->Deserialize(pChar);
	}

	return pMsg;
}

