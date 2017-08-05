#pragma once

#include "../IBehavior.h"

class StopBehavior : public IBehavior
{
public:
	StopBehavior(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID);
	virtual ~StopBehavior();

public:
	static const string NAME;

public:
	void Start(long long lStartTime, ...) override;
	void Initialize() override;
	void UpdateBody(long long lUpdateTime) override;
};