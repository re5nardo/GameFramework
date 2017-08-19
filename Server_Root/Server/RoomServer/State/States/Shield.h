#pragma once

#include "../IState.h"
#include "../StateIDs.h"
#include <string>

using namespace std;

class Shield : public IState
{
public:
	Shield(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID, long long lStartTime);
	virtual ~Shield();

public:
	static const string NAME;

public:
	static const unsigned short STATE_ID = StateID::Shield;

public:
	int GetID() override;
	void Initialize() override;
	void UpdateBody(long long lUpdateTime) override;
};