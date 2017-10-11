#pragma once

#include "../IState.h"
#include "../StateIDs.h"
#include <string>

using namespace std;

class GeneralState : public IState
{
public:
	GeneralState(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID, long long lStartTime);
	virtual ~GeneralState();

public:
	static const string NAME;

public:
	static const unsigned short STATE_ID = StateID::GeneralState;

public:
	int GetID() override;
	void Initialize() override;
	void UpdateBody(long long lUpdateTime) override;
};