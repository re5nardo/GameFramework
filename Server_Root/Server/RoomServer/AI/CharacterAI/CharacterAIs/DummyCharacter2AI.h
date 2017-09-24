#pragma once

#include "../ICharacterAI.h"
#include "btBulletCollisionCommon.h"

class DummyCharacter2AI : public ICharacterAI
{
public:
	DummyCharacter2AI(BaeGameRoom* pGameRoom, int nMasterDataID, long long lStartTime);
	virtual ~DummyCharacter2AI();

private:
	btVector3 m_vec3Position = btVector3(0, 0, 0);
	btVector3 m_vec3Rotation = btVector3(0, 0, 0);

public:
	void UpdateBody(long long lUpdateTime) override;

public:
	void SetData(btVector3& vec3StartPosition, btVector3& vec3StartRotation);
};