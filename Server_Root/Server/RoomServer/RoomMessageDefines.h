#pragma once

enum Messages
{
	TEST_MESSAGE_ID = 0,

	//  ToS
	ReadyForStartToS_ID = 0,
	GameEventMoveToS_ID,
	JoinLobbyToS_ID,
	SelectNormalGameToS_ID,

	//  ToC
	GameStartToC_ID = 30000,
	GameEventMoveToC_ID,
	JoinLobbyToC_ID,
	SelectNormalGameToC_ID,
};