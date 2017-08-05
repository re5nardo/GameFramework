#pragma once

#include "../../IEntity.h"
#include "../../../Data.h"

class BaeGameRoom;

class Projectile : public IEntity
{
public:
	Projectile(BaeGameRoom* pGameRoom, int nID, int nMasterDataID);
	virtual ~Projectile();

public:
	void Initialize() override;
	float GetMoveSpeed() override;
	void Update(long long lUpdateTime) override;
};