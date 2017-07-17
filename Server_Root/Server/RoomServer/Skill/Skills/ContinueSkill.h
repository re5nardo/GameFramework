#pragma once

#include "../ISkill.h"
#include <string>

class IBehavior;

using namespace std;

class ContinueSkill : public ISkill
{
public:
	ContinueSkill(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID);
	virtual ~ContinueSkill();

public:
	static const string NAME;

private:
	bool m_bContinue = false;
	IBehavior* m_pTargetBehavior = NULL;
	BaeGameRoom* m_pBaeGameRoom = NULL;

private:
	int		m_nTargetBehaviorID;
	float	m_fLength;
	float	m_fCooltime;
	float	m_fMP;

protected:
	bool IsCoolTime(__int64 lTime) override;
	bool IsValidToStart(__int64 lTime) override;
	void Start(__int64 lStartTime, ...) override;

public:
	void Initialize() override;
	void Update(__int64 lUpdateTime) override;
	void ProcessInput(__int64 lTime, BaeGameRoom* pBaeGameRoom, GameInputSkillToR* pMsg) override;
};