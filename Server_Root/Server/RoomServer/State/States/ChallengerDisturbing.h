#pragma once

#include "../IState.h"
#include "../StateIDs.h"
#include <string>
#include <map>

using namespace std;

class ChallengerDisturbing : public IState
{
public:
	ChallengerDisturbing(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID, long long lStartTime);
	virtual ~ChallengerDisturbing();

public:
	static const string NAME;

public:
	static const unsigned short STATE_ID = StateID::ChallengerDisturbing;

private:
	map<int, float> m_mapDisturbingInfo;

public:
	int GetID() override;
	void Initialize() override;
	void UpdateBody(long long lUpdateTime) override;

public:
	void OnCollision(IEntity* pOther, long long lTime) override;
};