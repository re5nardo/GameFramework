// RoomServer.cpp : 콘솔 응용 프로그램에 대한 진입점을 정의합니다.
//

#include "stdafx.h"
#include "Room.h"

void ClearSingltons();



int _tmain(int argc, _TCHAR* argv[])
{
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