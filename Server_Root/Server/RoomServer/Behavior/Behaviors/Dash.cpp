#include "stdafx.h"
#include "Dash.h"
#include "../../Entity/IEntity.h"
#include "../BehaviorIDs.h"
#define _USE_MATH_DEFINES
#include <math.h>
#include "../../Game/BaeGameRoom.h"
#include "btBulletCollisionCommon.h"

const string Dash::NAME = "Dash";

Dash::Dash(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID) : IBehavior(pGameRoom, pEntity, nMasterDataID)
{
}

Dash::~Dash()
{
}

void Dash::Start(long long lStartTime, ...)
{
	__super::Start(lStartTime);

	va_list ap;
	va_start(ap, lStartTime);
	m_pGameRoom = va_arg(ap, BaeGameRoom*);
	va_end(ap);

	if (m_pEntity->GetBehavior(BehaviorID::IDLE)->IsActivated())
		m_pEntity->GetBehavior(BehaviorID::IDLE)->Stop(lStartTime);
}

void Dash::Initialize()
{
}

void Dash::UpdateBody(long long lUpdateTime)
{
	btVector3 vec3Pos = m_pEntity->GetPosition();
	float radian = m_pEntity->GetRotation().y() * M_PI / 180;
	float x = 2 * btSin(radian);
	float z = 2 * btCos(radian);
	btVector3 vec3PosOffset(x, 0, z);
	btVector3 vec3Target = vec3Pos + vec3PosOffset;

	btTransform trHit;
	if (m_pGameRoom->TryMove(m_pEntity->GetID(), vec3Target, trHit))
	{
		m_pEntity->SetPosition(vec3Target);
		m_pGameRoom->SetCollisionObjectPosition(m_pEntity->GetID(), vec3Target);
	}
	else
	{
		m_pEntity->SetPosition(trHit.getOrigin());
		m_pGameRoom->SetCollisionObjectPosition(m_pEntity->GetID(), trHit.getOrigin());
	}
}