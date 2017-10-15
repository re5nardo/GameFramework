#pragma once


//	Client -> Lobby (0 ~ 9999)
#define JoinLobbyToL_ID								0
#define SelectNormalGameToL_ID						1


//	Lobby -> Client (10000 ~ 19999)
#define JoinLobbyToC_ID							10000
#define SelectNormalGameToC_ID					10001


//	Lobby -> Room (20000 ~ 29999)
#define CreateRoomToR_ID						20000


//	Room -> Lobby (30000 ~ 39999)
#define CreateRoomToL_ID						30000


//	Client -> Room (40000 ~ 49999)
#define ReadyForStartToR_ID						40000
#define PreparationStateToR_ID					40001
#define GameEventRunToR_ID						40002
#define EnterRoomToR_ID							40003
#define GameEventStopToR_ID						40005
#define GameInputSkillToR_ID					40006
#define GameEventDashToR_ID						40007


//	Room -> Client (50000 ~ 59999)
#define PreparationStateToC_ID					50000
#define GameStartToC_ID							50001
#define EnterRoomToC_ID							50003
#define PlayerEnterRoomToC_ID					50004
#define WorldSnapShotToC_ID						50008
#define WorldInfoToC_ID							50009