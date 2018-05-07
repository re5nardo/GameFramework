#pragma once

///////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////
//	@ PlayerKey : Player's unique key used in all server sides
//	@ PlayerIndex : Player's index used in game room (PlayerIndex is used for communication between Server and client)

#include <map>
#include <list>
#include <string>
#include <vector>
#include <chrono>
#include <mutex>
#include <utility>
#include "../../FBSFiles/FBSData_generated.h"
#include "../../FBSFiles/EnterRoomToC_generated.h"


class IMessage;
class EnterRoomToR;
class PreparationStateToR;
class PlayerInputToR;
class IPlayerInput;
class GameResultToR;

using namespace std;
using namespace chrono;

class BaeGameRoom2
{
public:
	BaeGameRoom2(int nMatchID, vector<string> vecMatchedPlayerKey);
	virtual ~BaeGameRoom2();

private:
	const long long TIME_STEP = 25;					//	<-- milliseconds, TickRate is 1000(1sec) / TIME_STEP
	const int TIME_LIMIT = 120;						//	<-- seconds

private:
	int								m_nMatchID;
	vector<string>					m_vecMatchedPlayerKey;					//	element : PlayerKey
	vector<FBS::PlayerInfo>			m_vecPlayerInfo;
	map<string, unsigned int>		m_mapPlayerKeySocket;					//	key : PlayerKey, value : Socket
	map<unsigned int, string>		m_mapSocketPlayerKey;					//	key : Socket, value : PlayerKey
	map<int, float>					m_mapPlayerIndexPreparationState;		//	key : PlayerIndex, value : PreparationState

private:
	list<IPlayerInput*> m_listPlayerInput;

private:
	bool m_bPlaying;

private:
	mutex m_LockPlayerInput;	//	플레이어별로 따로 mutex 두자

private:
	system_clock::time_point	m_StartTime;
	int							m_nTick;
	int							m_nEndTick;
	long long					m_lDeltaTime;			//	ElapsedTime after previous tick (Milliseconds)
	long long					m_lLastUpdateTime;		//	ElapsedTime after game was started (Milliseconds)

public:
	void OnRecvMessage(unsigned int socket, IMessage* pMsg);

private:
	void PrepareGame();
	void StartGame();
	void EndGame();
	void Reset();
	void SendToAllUsers(IMessage* pMsg, string strExclusionKey = "", bool bDelete = true);

	//	Protocol Handler
	void OnEnterRoomToR(EnterRoomToR* pMsg, unsigned int socket);
	void OnPreparationStateToR(PreparationStateToR* pMsg, unsigned int socket);
	void OnPlayerInputToR(PlayerInputToR* pMsg, unsigned int socket);
	void OnGameResultToR(GameResultToR* pMsg, unsigned int socket);

private:
	int GetPlayerIndexByPlayerKey(string strPlayerKey);
	int GetPlayerIndexBySocket(unsigned int socket);
	bool IsValidPlayer(string strPlayerKey);
	bool IsAllPlayersReady();

private:
	long long GetElapsedTime();	//	Milliseconds

public:
	long long GetLastUpdateTime();	//	Milliseconds

private:
	void Loop();
	void ProcessInput();
	void SendTickInfo();

private:
	static unsigned int __stdcall LoopThreadStart(void* param);
};