#pragma once

#include "../IState.h"
#include "../StateIDs.h"
#include <string>

using namespace std;

class FireBehavior : public IState
{
public:
	FireBehavior(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID, long long lStartTime);
	virtual ~FireBehavior();

public:
	static const string NAME;

public:
	static const unsigned short STATE_ID = StateID::FireBehavior;

private:
	int m_nTargetBehaviorID;

public:
	int GetID() override;
	void Initialize() override;
	void UpdateBody(long long lUpdateTime) override;
};