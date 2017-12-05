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
	CharacterStat m_DefaultStat;
	CharacterStat m_CurrentStat;

private:
	float m_fSpeed = 0;

private:
	int m_nMovePoint = 0;

public:
	float m_fSpeedPercent = 100;

public:
	void SetRole(Role role);
	Role GetRole();

public:
	void Initialize() override;
	float GetSpeed() override;
	float GetMaximumSpeed() override;
	FBS::Data::EntityType GetEntityType() override;
	void NotifyGameEvent(IGameEvent* pGameEvent) override;
	bool IsTerrainPassable() override;
	int GetMoveCollisionTypes() override;

public:
	void UpdateSkills(long long lUpdateTime);
	void LateUpdate(long long lUpdateTime) override;

public:
	ISkill* GetSkill(int nID);
	list<ISkill*> GetAllSkills();
	list<ISkill*> GetActivatedSkills();
	bool IsSkilling();

public:
	void InitStat(CharacterStat stat);
	float GetCurrentMP();
	void SetCurrentMP(float fMP);

public:
	void SetMoveSpeed(float fSpeed);
	void PlusMoveSpeed(float fValue);
	void MinusMoveSpeed(float fValue);

public:
	void OnAttacked(int nAttackingEntityID, int nDamage, long long lTime);
	void OnRespawn(long long lTime);

public:
	bool IsAlive();

public:
	void IncreaseMovePoint(int nPoint);
};