#pragma once

#include "../IBehavior.h"

class Fire : public IBehavior
{
public:
	Fire(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID);
	virtual ~Fire();

public:
	static const string NAME;

private:
	IEntity* m_pTarget = NULL;

public:
	void Start(long long lStartTime, ...) override;
	void Initialize() override;
	void UpdateBody(long long lUpdateTime) override;
};