#pragma once

#include <vector>
#include <map>
#include "IMessage.h"

using namespace std;

class TestMessage : public IMessage
{
public:
	TestMessage();
	virtual ~TestMessage();

private:
	string 						m_strName;					//	json field name : name
	int 						m_nAge;						//	json field name : age

public:
	vector<int>					m_vecFavoriteNumbers;		//	json field name : favoriteNumbers
	map<string, string>			m_mapOptions;				//	json field name : options
	
public:
	unsigned short GetID() override;
	const char* Serialize() override;
	bool Deserialize(const char* pChar) override;

public:
	void SetName(string strName);
	void SetAge(int nAge);
};