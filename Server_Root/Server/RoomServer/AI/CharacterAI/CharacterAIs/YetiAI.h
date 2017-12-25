#pragma once

#include "../ICharacterAI.h"
#include "btBulletCollisionCommon.h"

class YetiAI : public ICharacterAI
{
public:
	YetiAI(BaeGameRoom* pGameRoom, int nMasterDataID, long long lStartTime);
	virtual ~YetiAI();

private:
	btVector3 m_vec3Position = btVector3(0, 0, 0);
	btVector3 m_vec3Rotation = btVector3(0, 0, 0);
	btVector3 m_vec3DestPosition = btVector3(0, 0, 0);

private:
	const float SPAWN_TIME = 0;

public:
	void UpdateBody(long long lUpdateTime) override;

public:
	void SetData(btVector3& vec3StartPosition, btVector3& vec3StartRotation, btVector3& vec3DestPosition);
};