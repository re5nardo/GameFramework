#pragma once

#include <map>
#include <string>
#include <vector>
#include <chrono>
#include <mutex>
#include "MapManager.h"

class IMessage;
class CreateRoomToR;
class GameEventMoveToR;
class GameEventIdleToR;
class GameEventStopToR;
class EnterRoomToR;
class PreparationStateToR;
class IMessage;
class ICharacter;

using namespace std;
using namespace chrono;

class BaeGameRoom
{
public:
	BaeGameRoom(int nMatchID, vector<string> vecMatchedPlayerKey);
	virtual ~BaeGameRoom();

private:
	const __int64 TIME_STEP = 15;		//	Milliseconds
	//	So TickRate is 66.6666...    (1000(1sec) / 15 = 66.6666...)

private:
	int								m_nMatchID;
	vector<string>					m_vecMatchedPlayerKey;
	map<string, unsigned int>		m_mapPlayerKeySocket;				
	map<unsigned int, string>		m_mapSocketPlayerKey;
	map<int, float>					m_mapPlayerIndexPreparationState;		//	key : PlayerIndex, value : PreparationState

private:
	map<int, ICharacter*> m_mapCharacter;

private:
	map<int, IMessage*> m_mapPlayerInput;

private:
	bool m_bPlaying;

private:
	mutex m_LockPlayerInput;	//	플레이어별로 따로 mutex 두자

private:
	system_clock::time_point	m_StartTime;
	int							m_nTick;
	__int64						m_lDeltaTime;			//	ElapsedTime after previous tick (Milliseconds)
	__int64						m_lLastUpdateTime;		//	ElapsedTime after game was started (Milliseconds)

private:
	MapManager m_MapManager;

public:
	void OnRecvMessage(unsigned int socket, IMessage* pMsg);

private:
	void StartGame();
	void EndGame();
	void Reset();
	void SendToAllUsers(IMessage* pMsg, string strExclusionKey = "", bool bDelete = true);

	//	Protocol Handler
	void OnGameEventMoveToR(GameEventMoveToR* pMsg, unsigned int socket);
	void OnGameEventIdleToR(GameEventIdleToR* pMsg, unsigned int socket);
	void OnGameEventStopToR(GameEventStopToR* pMsg, unsigned int socket);
	void OnEnterRoomToR(EnterRoomToR* pMsg, unsigned int socket);
	void OnPreparationStateToR(PreparationStateToR* pMsg, unsigned int socket);

private:
	int GetPlayerIndexByPlayerKey(string strPlayerKey);
	int GetPlayerIndexBySocket(unsigned int socket);
	bool IsValidPlayer(string strPlayerKey);
	bool IsAllPlayersReady();

private:
	__int64 GetElapsedTime();	//	Milliseconds

private:
	void Loop();
	void UpdateTime();
	void ProcessInput();
	void Update();
	void LateUpdate();
	void SendWorldSnapShot();

private:
	static unsigned int __stdcall LoopThreadStart(void* param);
};

