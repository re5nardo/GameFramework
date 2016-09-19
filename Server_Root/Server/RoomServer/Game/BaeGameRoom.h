#pragma once

#include <map>
#include <string>

class IMessage;
class CreateRoomToR;
class GameEventMoveToS;

using namespace std;

class BaeGameRoom
{
public:
	BaeGameRoom();
	virtual ~BaeGameRoom();

private:
	map<string, unsigned int>			m_mapPlayerSocket;

private:
	void OnRecvMessage(unsigned int socket, IMessage* pMsg);

private:
	void SendToAllUsers(IMessage* pMsg);

	//	Protocol Handler
	void OnCreateRoomToR(CreateRoomToR* pMsg, unsigned int socket);
	void OnGameEventMoveToS(GameEventMoveToS* pMsg, unsigned int socket);
};

