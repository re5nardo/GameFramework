#pragma once

#include "NetworkDefines.h"

using namespace std;

class Accepter
{
public:
	Accepter(const USHORT nPort);
	virtual ~Accepter();

private:
	SOCKET				m_Socket;
	USHORT				m_nPort;
	bool				m_bListening;

public:
	int						Start();
	LPPER_HANDLE_DATA		Accept();
};