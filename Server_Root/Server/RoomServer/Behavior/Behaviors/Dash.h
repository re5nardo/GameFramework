#pragma once

#include "../IBehavior.h"

class Dash : public IBehavior
{
public:
	Dash(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID);
	virtual ~Dash();

public:
	static const string NAME;

public:
	void Start(long long lStartTime, ...) override;
	void Initialize() override;
	void UpdateBody(long long lUpdateTime) override;
};