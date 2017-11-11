#pragma once

#include "../IBehavior.h"
#include "btBulletCollisionCommon.h"

class Character;

class Dash : public IBehavior
{
public:
	Dash(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID);
	virtual ~Dash();

public:
	static const string NAME;

private:
	Character* pCharacter;		//	Only Character uses Dash Behavior
	btVector3 m_vec3Dest;

public:
	void Start(long long lStartTime, ...) override;
	void Initialize() override;
	void UpdateBody(long long lUpdateTime) override;
};