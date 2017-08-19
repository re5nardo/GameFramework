#pragma once

#include "../IState.h"
#include "../StateIDs.h"
#include <string>
#include <utility>
#include <vector>

using namespace std;

class Acceleration : public IState
{
public:
	Acceleration(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID, long long lStartTime);
	virtual ~Acceleration();

public:
	static const string NAME;

public:
	static const unsigned short STATE_ID = StateID::Acceleration;

private:
	vector<pair<float, float>> m_vecEvent;

public:
	int GetID() override;
	void Initialize() override;
	void UpdateBody(long long lUpdateTime) override;
};