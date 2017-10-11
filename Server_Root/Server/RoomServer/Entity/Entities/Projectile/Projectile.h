#pragma once

#include "../../IEntity.h"
#include "../../../Data.h"

class BaeGameRoom;
class Character;

class Projectile : public IEntity
{
public:
	Projectile(BaeGameRoom* pGameRoom, int nProjectorID, int nID, int nMasterDataID);
	virtual ~Projectile();

private:
	int m_nProjectorID;
	vector<string> m_vecLifeInfo;

public:
	void Initialize() override;
	float GetMoveSpeed() override;
	FBS::Data::EntityType GetEntityType() override;
	void NotifyGameEvent(IGameEvent* pGameEvent) override;
	bool IsTerrainPassable() override;

public:
	int GetProjectorID();
	Character* GetProjector();
};