#pragma once

#include "../../IEntity.h"
#include "../../../Data.h"

class BaeGameRoom;

class Character : public IEntity
{
public:
	Character(BaeGameRoom* pGameRoom, int nID, int nMasterDataID);
	virtual ~Character();

protected:
	list<ISkill*> m_listSkill;

protected:
	Stat m_DefaultStat;
	Stat m_CurrentStat;

public:
	float fMoveSpeedPercent = 100;

public:
	void Initialize() override;
	float GetMoveSpeed() override;
	FBS::Data::EntityType GetEntityType() override;
	void NotifyGameEvent(IGameEvent* pGameEvent) override;

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