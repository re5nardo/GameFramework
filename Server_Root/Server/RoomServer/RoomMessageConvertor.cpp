#include "stdafx.h"
#include "RoomMessageConvertor.h"
#include "RoomMessageHeader.h"
#include "RoomMessageDefines.h"

RoomMessageConvertor::RoomMessageConvertor()
{
}


RoomMessageConvertor::~RoomMessageConvertor()
{
}

IMessage* RoomMessageConvertor::GetMessage(const unsigned short nMessageID, const char* pChar)
{
	IMessage* pMsg = NULL;

	/*if (nMessageID == Messages::JoinLobbyToS_ID)
	{
		pMsg = new JoinLobbyToS();
	}
	else if (nMessageID == Messages::SelectNormalGameToS_ID)
	{
		pMsg = new SelectNormalGameToS();
	}*/

	if (pMsg != NULL)
	{
		pMsg->Deserialize(pChar);
	}

	return pMsg;
}

