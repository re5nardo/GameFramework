#pragma once

#include "../ISkill.h"
#include <string>
#include <utility>
#include <vector>

class IBehavior;

using namespace std;

class FireSkill : public ISkill
{
public:
	FireSkill(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID);
	virtual ~FireSkill();

public:
	static const string NAME;

private:
	vector<pair<int, float>> m_vecBehavior;
	vector<pair<int, float>> m_vecState;
	float	m_fLength;
	float	m_fCooltime;
	float	m_fMP;

protected:
	bool IsCoolTime(long long lTime) override;
	bool IsValidToStart(long long lTime) override;

public:
	void Initialize() override;
	void UpdateBody(long long lUpdateTime) override;
	void ProcessInput(long long lTime, BaeGameRoom* pBaeGameRoom, GameInputSkillToR* pMsg) override;
};