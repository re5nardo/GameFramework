#pragma once

#include "../../IEntity.h"

class BaeGameRoom;

class Item : public IEntity
{
public:
	Item(BaeGameRoom* pGameRoom, long long lTime, int nID, int nMasterDataID);
	virtual ~Item();

private:
	long long m_lSpawnedTime;
	float m_fLifespan;

public:
	void Initialize() override;
	float GetMoveSpeed() override;
	FBS::Data::EntityType GetEntityType() override;
	void NotifyGameEvent(IGameEvent* pGameEvent) override;
	bool IsTerrainPassable() override;
	int GetMoveCollisionTypes() override;
	void LateUpdate(long long lUpdateTime) override;
};