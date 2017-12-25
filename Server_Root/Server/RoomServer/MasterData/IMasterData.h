#pragma once

#include <vector>
#include <string>

using namespace std;

class IMasterData
{
public:
	IMasterData();
	virtual ~IMasterData();

public:
	int m_nID;

public:
	virtual void SetData(vector<string> data) = 0;
};