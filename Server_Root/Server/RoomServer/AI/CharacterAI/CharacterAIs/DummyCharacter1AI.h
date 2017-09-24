#pragma once

#include "../ICharacterAI.h"
#include "btBulletCollisionCommon.h"

class DummyCharacter1AI : public ICharacterAI
{
public:
	DummyCharacter1AI(BaeGameRoom* pGameRoom, int nMasterDataID, long long lStartTime);
	virtual ~DummyCharacter1AI();

private:
	btVector3 m_vec3StartPosition = btVector3(0, 0, 0);
	btVector3 m_vec3StartRotation = btVector3(0, 0, 0);
	btVector3 m_vec3DestPosition = btVector3(0, 0, 0);

public:
	void UpdateBody(long long lUpdateTime) override;

public:
	void SetData(btVector3& vec3StartPosition, btVector3& vec3StartRotation, btVector3& vec3DestPosition);
};