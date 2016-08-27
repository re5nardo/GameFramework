#include "stdafx.h"
#include "LobbyMessageConvertor.h"
#include "LobbyMessageHeader.h"
#include "LobbyMessageDefines.h"

LobbyMessageConvertor::LobbyMessageConvertor()
{
}


LobbyMessageConvertor::~LobbyMessageConvertor()
{
}

IMessage* LobbyMessageConvertor::GetMessage(const unsigned short nMessageID, const char* pChar)
{
	IMessage* pMsg = NULL;

	if (nMessageID == Messages::ReadyForStartToS_ID)
	{
		pMsg = new ReadyForStartToS();
	}
	else if (nMessageID == Messages::GameEventMoveToS_ID)
	{
		pMsg = new GameEventMoveToS();
	}
	else if (nMessageID == Messages::JoinLobbyToS_ID)
	{
		pMsg = new JoinLobbyToS();
	}
	else if (nMessageID == Messages::SelectNormalGameToS_ID)
	{
		pMsg = new SelectNormalGameToS();
	}

	if (pMsg != NULL)
	{
		pMsg->Deserialize(pChar);
	}

	return pMsg;
}