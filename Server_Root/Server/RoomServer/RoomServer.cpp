// RoomServer.cpp : 콘솔 응용 프로그램에 대한 진입점을 정의합니다.
//

#include "stdafx.h"
#include "Room.h"
#include "MasterData\MasterDataManager.h"
#include "../CommonSources/tinyxml2.h"

void ClearSingltons();
int GetBindPort();

int _tmain(int argc, _TCHAR* argv[])
{
	MasterDataManager::Instance()->DownloadMasterData("");

	Room* pRoom = new Room(GetBindPort());

	int nExit = -1;
	while (true)
	{
		scanf_s("%d", &nExit);
		if (nExit == 9)
		{
			break;
		}
	}

	delete pRoom;

	ClearSingltons();

	return 0;
}

void ClearSingltons()
{
}

int GetBindPort()
{
	tinyxml2::XMLDocument doc;
	doc.LoadFile("RoomServerConfig.xml");

	tinyxml2::XMLElement* pPort = doc.FirstChildElement("Port");

	return atoi(pPort->GetText());
}