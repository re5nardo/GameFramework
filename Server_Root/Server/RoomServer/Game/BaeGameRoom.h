#pragma once

#include <map>
#include <string>
#include <vector>

class IMessage;
class CreateRoomToR;
class GameEventMoveToR;
class EnterRoomToR;

using namespace std;

class BaeGameRoom
{
public:
	BaeGameRoom(int nMatchID, vector<string> vecMatchedPlayers);
	virtual ~BaeGameRoom();

private:
	int m_nMatchID;
	vector<string> m_vecMatchedPlayers;
	vector<string> m_vecPlayers;

	map<string, unsigned int>			m_mapPlayerSocket;

public:
	void OnRecvMessage(unsigned int socket, IMessage* pMsg);

private:
	void SendToAllUsers(IMessage* pMsg);

	//	Protocol Handler
	void OnCreateRoomToR(CreateRoomToR* pMsg, unsigned int socket);
	void OnGameEventMoveToR(GameEventMoveToR* pMsg, unsigned int socket);
	void OnEnterRoomToR(EnterRoomToR* pMsg, unsigned int socket);
};

