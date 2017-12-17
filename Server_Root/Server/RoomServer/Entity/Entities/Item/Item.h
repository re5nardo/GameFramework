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
	string m_strEffectType;
	int m_nEffectParam;

public:
	void Initialize() override;
	float GetSpeed() override;
	float GetMaximumSpeed() override;
	FBS::Data::EntityType GetEntityType() override;
	void NotifyGameEvent(IGameEvent* pGameEvent) override;
	bool IsTerrainPassable() override;
	int GetMoveCollisionTypes() override;
	int GetAttackTargetTypes() override;
	void LateUpdate(long long lUpdateTime) override;
	void OnCollision(IEntity* pOther, long long lTime) override;
};