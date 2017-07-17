#pragma once

#include "../IBehavior.h"
#include <string>

class BaeGameRoom;

using namespace std;

class Dash : public IBehavior
{
public:
	Dash(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID);
	virtual ~Dash();

private:
	BaeGameRoom* m_pGameRoom = NULL;

public:
	static const string NAME;

public:
	void Start(__int64 lStartTime, ...) override;
	void Initialize() override;
	void Update(__int64 lUpdateTime) override;
};