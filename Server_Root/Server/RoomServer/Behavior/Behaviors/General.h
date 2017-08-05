#pragma once

#include "../IBehavior.h"

class General : public IBehavior
{
public:
	General(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID);
	virtual ~General();

public:
	static const string NAME;

private:
	bool m_bStopIdle = true;

public:
	void Start(long long lStartTime, ...) override;
	void Initialize() override;
	void UpdateBody(long long lUpdateTime) override;
};