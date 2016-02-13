
#include "stdafx.h"
#include "Lobby.h"


void ClearSingltons();

int _tmain(int argc, _TCHAR* argv[])
{
	Lobby* pLobby = new Lobby(9110);

	int nExit = -1;
	while (true)
	{
		scanf_s("%d", &nExit);
		if (nExit == 9)
		{
			break;
		}
	}

	delete pLobby;

	ClearSingltons();

	return 0;
}

void ClearSingltons()
{
}