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
	bool IsCoolTime(long long lTime) override;
	bool IsValidToStart(long long lTime) override;
	void Start(long long lStartTime, ...) override;

public:
	void Initialize() override;
	void UpdateBody(long long lUpdateTime) override;
	void ProcessInput(long long lTime, BaeGameRoom* pBaeGameRoom, GameInputSkillToR* pMsg) override;
};