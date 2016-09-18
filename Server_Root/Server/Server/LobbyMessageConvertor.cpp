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

	if (nMessageID == JoinLobbyToS::MESSAGE_ID)
	{
		pMsg = new JoinLobbyToS();
	}
	else if (nMessageID == SelectNormalGameToS::MESSAGE_ID)
	{
		pMsg = new SelectNormalGameToS();
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