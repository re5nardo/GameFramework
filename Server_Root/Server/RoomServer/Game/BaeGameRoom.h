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
#include <utility>
#include "CollisionManager.h"
#include "../../FBSFiles/FBSData_generated.h"
#include "../Entity/Entities/Character/Character.h"

class IMessage;
class CreateRoomToR;
class GameEventMoveToR;
class GameEventIdleToR;
class GameEventStopToR;
class EnterRoomToR;
class PreparationStateToR;
class IMessage;
class ICharacterAI;
class GameInputSkillToR;
class IGameEvent;
class IEntity;
class GameEventDashToR;
class IBehavior;

using namespace std;
using namespace chrono;

class BaeGameRoom
{
public:
	BaeGameRoom(int nMatchID, vector<string> vecMatchedPlayerKey);
	virtual ~BaeGameRoom();

private:
	const long long TIME_STEP = 200;		//	<-- milliseconds, TickRate is 1000(1sec) / TIME_STEP

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
	map<int, pair<long long, IMessage*>>	m_mapPlayerInput;		//	key : PlayerIndex, value : <Time, Input Message>
	map<int, int>							m_mapPlayerEntity;		//	key : PlayerIndex, value : EntityID
	map<int, int>							m_mapEntityPlayer;		//	key : EntityID, value : PlayerIndex
	map<int, IEntity*>						m_mapEntity;			//	key : EntityID, value : Entity

private:
	list<ICharacterAI*> m_listDisturber;

private:
	bool m_bPlaying;

private:
	mutex m_LockPlayerInput;	//	플레이어별로 따로 mutex 두자

private:
	system_clock::time_point	m_StartTime;
	int							m_nTick;
	long long					m_lDeltaTime;			//	ElapsedTime after previous tick (Milliseconds)
	long long					m_lLastUpdateTime;		//	ElapsedTime after game was started (Milliseconds)

private:
	CollisionManager m_CollisionManager;
	map<int, int> m_mapEntityCollision;		//	key : EntityID,				value : CollisionObject ID
	map<int, int> m_mapCollisionEntity;		//	key : CollisionObject ID,	value : EntityID

private:
	list<IGameEvent*> m_listGameEvent;

private:
	list<int> m_listDestroyReserved;

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
	void OnGameInputSkillToR(GameInputSkillToR* pMsg, unsigned int socket);
	void OnGameEventDashToR(GameEventDashToR* pMsg, unsigned int socket);

private:
	int GetPlayerIndexByPlayerKey(string strPlayerKey);
	int GetPlayerIndexBySocket(unsigned int socket);
	bool IsValidPlayer(string strPlayerKey);
	bool IsAllPlayersReady();

public:
	int GetEntityIDByCollisionObjectID(int nCollisionObjectID);
	int GetCollisionObjectIDByEntityID(int nEntityID);

public:
	IEntity* GetEntity(int nEntityID);

private:
	long long GetElapsedTime();	//	Milliseconds

private:
	void LoadMap(int nMapID);
	void SetObstacles(int nMapID, int nRandomSeed);

public:
	void EntityMove(int nEntityID, IBehavior* pBehavior, btVector3& vec3To, int nTypes, long long lStartTime, long long lEndTime);

public:
	bool ContinuousCollisionDectection(int nTargetID, CollisionObject* pOther, btVector3& vec3To, btVector3& vec3Hit);
	bool DiscreteCollisionDectection(int nTargetID, int nOtherID, btVector3& vec3Hit);
	bool GetCollisionObjectsInRange(int nTargetID, btVector3& vec3To, int nTypes, list<CollisionObject*>* pObjects);

	bool CheckDiscreteCollisionDectection(int nEntityID, int nTypes, list<pair<int, btVector3>>* listHit);
	bool CheckContinuousCollisionDectectionFirst(int nEntityID, btVector3& vec3Dest, int nTypes, pair<int, btVector3>* hit);
	bool CheckContinuousCollisionDectection(int nEntityID, btVector3& vec3Dest, int nTypes, list<pair<int, btVector3>>* listHit);
	bool CehckExistInRange(btVector3& vec3Center, float fRadius, int nTypes, list<pair<int, btVector3>>* listItem);	//	Ignore Y-axis
	bool CehckExistInRange(int nEntityID, float fRadius, int nTypes, list<pair<int, btVector3>>* listItem);	//	Ignore Y-axis

public:
	bool IsChallenger(int nEntityID);
	bool IsDisturber(int nEntityID);

public:
	void SetCollisionObjectPosition(int nEntityID, btVector3& vec3Position);
	void SetCollisionObjectRotation(int nEntityID, btVector3& vec3Rotation);

public:
	void AddGameEvent(IGameEvent* pGameEvent);
	void AddPositionGameEvent(float fEventTime, int nEntityID, float fStartTime, float fEndTime, btVector3& vec3StartPosition, btVector3& vec3EndPosition);

public:
	bool CreateCharacter(int nMasterDataID, int* pEntityID, Character** pCharacter, Character::Role role);
	bool CreateProjectile(int nMasterDataID, int* pEntityID, Projectile** pProjectile, int nCreatorID);
	void DestroyEntity(int nEntityID);
	void TrimEntity();

private:
	void Loop();
	void UpdateTime();
	void ProcessInput();
	void Update();
	void LateUpdate();
	void SendWorldSnapShot();
	void SendWorldInfo();

private:
	static unsigned int __stdcall LoopThreadStart(void* param);
};

