#pragma once

#include "../../IEntity.h"
#include "../../../Data.h"

class BaeGameRoom;

class Projectile : public IEntity
{
public:
	Projectile(BaeGameRoom* pGameRoom, int nID, int nMasterDataID);
	virtual ~Projectile();

private:
	int m_nCasterID;
	vector<string> m_vecLifeInfo;

public:
	void Initialize() override;
	float GetMoveSpeed() override;
	FBS::Data::EntityType GetEntityType() override;
	void NotifyGameEvent(IGameEvent* pGameEvent) override;

public:
	int GetCasterID();
};