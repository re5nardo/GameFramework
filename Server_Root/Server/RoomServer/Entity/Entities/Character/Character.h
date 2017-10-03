#pragma once

#include "../../IEntity.h"
#include "../../../Data.h"

class BaeGameRoom;

class Character : public IEntity
{
public:
	enum Role
	{
		Challenger = 0,
		Disturber,
	};

public:
	Character(BaeGameRoom* pGameRoom, int nID, int nMasterDataID, Role role);
	virtual ~Character();

private:
	Role m_Role = Role::Challenger;

protected:
	list<ISkill*> m_listSkill;

protected:
	Stat m_DefaultStat;
	Stat m_CurrentStat;

public:
	float fMoveSpeedPercent = 100;

public:
	void SetRole(Role role);
	Role GetRole();

public:
	void Initialize() override;
	float GetMoveSpeed() override;
	FBS::Data::EntityType GetEntityType() override;
	void NotifyGameEvent(IGameEvent* pGameEvent) override;
	bool IsMovableOnCollision(IEntity* pOther) override;
	void OnCollision(IEntity* pOther, long long lTime) override;
	bool IsTerrainPassable() override;

public:
	void UpdateSkills(long long lUpdateTime);

public:
	ISkill* GetSkill(int nID);
	list<ISkill*> GetAllSkills();
	list<ISkill*> GetActivatedSkills();
	bool IsSkilling();

public:
	void SetStat(Stat stat);
	float GetCurrentMP();
	void SetCurrentMP(float fMP);

public:
	void PlusMoveSpeed(float fValue);
	void MinusMoveSpeed(float fValue);
};