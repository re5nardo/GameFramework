#include "stdafx.h"
#include "Lobby.h"
#include "../CommonSources/tinyxml2.h"

void ClearSingltons();
int GetBindPort();

int _tmain(int argc, _TCHAR* argv[])
{
	Lobby* pLobby = new Lobby(GetBindPort());

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

//	temp..
int GetBindPort()
{
	tinyxml2::XMLDocument doc;
	doc.LoadFile("LobbyServerConfig.xml");

	tinyxml2::XMLElement* pPort = doc.FirstChildElement("Port");

	return atoi(pPort->GetText());
}