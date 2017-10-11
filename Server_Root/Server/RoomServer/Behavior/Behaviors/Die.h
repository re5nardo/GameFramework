#pragma once

#include "../IBehavior.h"

class Die : public IBehavior
{
public:
	Die(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID);
	virtual ~Die();

public:
	static const string NAME;

public:
	void Start(long long lStartTime, ...) override;
	void Initialize() override;
	void UpdateBody(long long lUpdateTime) override;
};