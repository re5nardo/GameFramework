#include "stdafx.h"
#include "LobbyMessageConvertor.h"
#include "LobbyMessageHeader.h"

LobbyMessageConvertor::LobbyMessageConvertor()
{
}


LobbyMessageConvertor::~LobbyMessageConvertor()
{
}

IMessage* LobbyMessageConvertor::GetMessage(const unsigned short nMessageID, const char* pChar)
{
	IMessage* pMsg = NULL;

	if (nMessageID == JoinLobbyToL::MESSAGE_ID)
	{
		pMsg = new JoinLobbyToL();
	}
	else if (nMessageID == SelectNormalGameToL::MESSAGE_ID)
	{
		pMsg = new SelectNormalGameToL();
	}
	else if (nMessageID == CreateRoomToL::MESSAGE_ID)
	{
		pMsg = new CreateRoomToL();
	}

	if (pMsg != NULL)
	{
		pMsg->Deserialize(pChar);
	}

	return pMsg;
}