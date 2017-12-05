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
	Character* m_Projector;
	vector<string> m_vecLifeInfo;

public:
	void Initialize() override;
	float GetSpeed() override;
	float GetMaximumSpeed() override;
	FBS::Data::EntityType GetEntityType() override;
	void NotifyGameEvent(IGameEvent* pGameEvent) override;
	bool IsTerrainPassable() override;
	int GetMoveCollisionTypes() override;

public:
	int GetProjectorID();
	Character* GetProjector();
};