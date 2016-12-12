#pragma once

#include <map>
#include <string>
#include <vector>
#include <chrono>

class IMessage;
class CreateRoomToR;
class GameEventMoveToR;
class GameEventIdleToR;
class GameEventStopToR;
class EnterRoomToR;
class PreparationStateToR;
class IMessage;

using namespace std;
using namespace chrono;

class BaeGameRoom
{
public:
	BaeGameRoom(int nMatchID, vector<string> vecMatchedPlayerKey);
	virtual ~BaeGameRoom();

private:
	int								m_nMatchID;
	vector<string>					m_vecMatchedPlayerKey;
	map<string, unsigned int>		m_mapPlayerKeySocket;				
	map<unsigned int, string>		m_mapSocketPlayerKey;
	map<int, float>					m_mapPlayerIndexPreparationState;		//	key : PlayerIndex, value : PreparationState

private:
	system_clock::time_point			m_StartTime;
	vector<pair<__int64, IMessage*>>	m_vecGameEventRecord;	//	Milliseconds

public:
	void OnRecvMessage(unsigned int socket, IMessage* pMsg);

private:
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
};

