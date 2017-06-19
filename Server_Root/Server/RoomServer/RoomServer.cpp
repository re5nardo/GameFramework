// RoomServer.cpp : �ܼ� ���� ���α׷��� ���� �������� �����մϴ�.
//

#include "stdafx.h"
#include "Room.h"
#include "MasterData\MasterDataManager.h"

void ClearSingltons();



int _tmain(int argc, _TCHAR* argv[])
{
	MasterDataManager::Instance()->DownloadMasterData("");

	Room* pRoom = new Room(9111);

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