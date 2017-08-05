#pragma once

#include "../IBehavior.h"
#include "btBulletCollisionCommon.h"

class Move : public IBehavior
{
public:
	Move(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID);
	virtual ~Move();

public:
	static const string NAME;

private:
	btVector3 m_vec3Dest;

public:
	void Start(long long lStartTime, ...) override;
	void Initialize() override;
	void UpdateBody(long long lUpdateTime) override;
};