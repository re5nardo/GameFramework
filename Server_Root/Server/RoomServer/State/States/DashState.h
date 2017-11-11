#pragma once

#include "../IState.h"
#include "../StateIDs.h"
#include <string>

using namespace std;

class DashState : public IState
{
public:
	DashState(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID, long long lStartTime);
	virtual ~DashState();

public:
	static const string NAME;

public:
	static const unsigned short STATE_ID = StateID::DashState;

private:
	long long m_lExpiredTime;

public:
	int GetID() override;
	void Initialize() override;
	void UpdateBody(long long lUpdateTime) override;

public:
	void Prolong(long long lTime);
};