#pragma once

#include "../ICharacterAI.h"
#include "btBulletCollisionCommon.h"

class MamboRabbitAI : public ICharacterAI
{
public:
	MamboRabbitAI(BaeGameRoom* pGameRoom, int nMasterDataID, long long lStartTime);
	virtual ~MamboRabbitAI();

private:
	btVector3 m_vec3Position = btVector3(0, 0, 0);
	btVector3 m_vec3Rotation = btVector3(0, 0, 0);

private:
	const float SPAWN_TIME = 0;
	const float FIRE_COOLTIME = 3;
	const int FIRE_BEHAVIOR_ID = 14;

private:
	float m_fCoolTime = FIRE_COOLTIME;

public:
	void UpdateBody(long long lUpdateTime) override;

public:
	void SetData(btVector3& vec3StartPosition, btVector3& vec3StartRotation);
};