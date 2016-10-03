#pragma once

#include <map>
#include <string>
#include <vector>

class IMessage;
class CreateRoomToR;
class GameEventMoveToR;
class EnterRoomToR;
class PreparationStateToR;

using namespace std;

class BaeGameRoom
{
public:
	BaeGameRoom(int nMatchID, vector<string> vecMatchedPlayerKey);
	virtual ~BaeGameRoom();

private:
	int								m_nMatchID;
	map<string, unsigned int>		m_mapPlayerKeySocket;				
	map<unsigned int, string>		m_mapSocketPlayerKey;
	map<string, int>				m_mapPlayerKeyPlayerIndex;
	map<int, string>				m_mapPlayerIndexPlayerKey;
	map<int, float>					m_mapPlayerIndexPreparationState;		//	key : PlayerIndex, value : PreparationState

public:
	void OnRecvMessage(unsigned int socket, IMessage* pMsg);

private:
	void SendToAllUsers(IMessage* pMsg, string strExclusionKey = "");

	//	Protocol Handler
	void OnGameEventMoveToR(GameEventMoveToR* pMsg, unsigned int socket);
	void OnEnterRoomToR(EnterRoomToR* pMsg, unsigned int socket);
	void OnPreparationStateToR(PreparationStateToR* pMsg, unsigned int socket);

private:
	int GetPlayerIndexByPlayerKey(string strPlayerKey);
	int GetPlayerIndexBySocket(unsigned int socket);
	bool IsValidPlayer(string strPlayerKey);
	bool IsAllPlayersReady();
};

