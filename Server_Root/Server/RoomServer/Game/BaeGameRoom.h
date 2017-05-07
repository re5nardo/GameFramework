#pragma once

///////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////
//	@ PlayerKey : Player's unique key used in all server sides
//	@ PlayerIndex : Player's index used in game room (PlayerIndex is used for communication between Server and client)

#include <map>
#include <string>
#include <vector>
#include <chrono>
#include <mutex>
#include "CollisionManager.h"

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
	const __int64 TIME_STEP = 15;		//	<-- milliseconds, So TickRate is 66.6666... (1000(1sec) / 15 = 66.6666...)

private:
	mutex m_LockEntitySequence;
	int m_nEntitySequence = 0;

private:
	int								m_nMatchID;
	vector<string>					m_vecMatchedPlayerKey;					//	element : PlayerKey
	map<string, unsigned int>		m_mapPlayerKeySocket;					//	key : PlayerKey, value : Socket
	map<unsigned int, string>		m_mapSocketPlayerKey;					//	key : Socket, value : PlayerKey
	map<int, float>					m_mapPlayerIndexPreparationState;		//	key : PlayerIndex, value : PreparationState

private:
	map<int, ICharacter*>	m_mapCharacter;			//	key : PlayerIndex, value : Character
	map<int, IMessage*>		m_mapPlayerInput;		//	key : PlayerIndex, value : Input Message
	//map<int, int>			m_mapPlayerEntity;		//	key : PlayerIndex, value : Entity ID
	map<int, int>			m_mapEntityPlayer;		//	key : Entity ID, value : PlayerIndex

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
	CollisionManager m_CollisionManager;
	map<int, int> m_mapPlayerCollision;		//	key : PlayerIndex, value : CollisionObject ID

public:
	void OnRecvMessage(unsigned int socket, IMessage* pMsg);

private:
	void StartGame();
	void EndGame();
	void Reset();
	void SendToAllUsers(IMessage* pMsg, string strExclusionKey = "", bool bDelete = true);

	//	Protocol Handler
	void OnGameEventMoveToR(GameEventMoveToR* pMsg, unsigned int socket);
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
	void LoadMap(int nID);

public:
	bool TryMove(int nEntityID, btVector3& vec3Dest, btTransform& trHit);

public:
	void SetPlayerCollision();
	void SetCollisionObjectPosition(int nEntityID, btVector3& vec3Position);
	void SetCollisionObjectRotation(int nEntityID, btVector3& vec3Rotation);

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

