#include "stdafx.h"
#include "Dash.h"
#include "../../Entity/IEntity.h"
#include "../../Util.h"
#include "../BehaviorIDs.h"
#define _USE_MATH_DEFINES
#include <math.h>
#include "../../Game/BaeGameRoom.h"
#include "../../GameEvent/GameEvents/Position.h"
#include "../../GameEvent/GameEvents/Rotation.h"
#include "../../Entity/Entities/Character/Character.h"

const string Dash::NAME = "Dash";

Dash::Dash(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID) : IBehavior(pGameRoom, pEntity, nMasterDataID)
{
	pCharacter = (Character*)pEntity;
}

Dash::~Dash()
{
}

void Dash::Start(long long lStartTime, ...)
{
	__super::Start(lStartTime);

	va_list ap;
	va_start(ap, lStartTime);
	btVector3* pVec3Dest = va_arg(ap, btVector3*);
	m_vec3Dest = *pVec3Dest;
	va_end(ap);

	if (m_pEntity->GetBehavior(BehaviorID::IDLE) != NULL && m_pEntity->GetBehavior(BehaviorID::IDLE)->IsActivated())
		m_pEntity->GetBehavior(BehaviorID::IDLE)->Stop(lStartTime);

	IBehavior* pMove = m_pEntity->GetBehavior(BehaviorID::MOVE);
	if (pMove != NULL)
	{
		pMove->Update(lStartTime);
		if (pMove->IsActivated())
		{
			pMove->Stop(lStartTime);
		}
	}
}

void Dash::Initialize()
{
}

void Dash::UpdateBody(long long lUpdateTime)
{
	if (m_fDeltaTime == 0)
		return;

	//	Rotation
	btVector3 vec3StartRotation = pCharacter->GetRotation();
	btVector3 vec3Move = m_vec3Dest - pCharacter->GetPosition();
	float fTargetRotation_Y = Util::GetAngle_Y(vec3Move);
	btVector3 vec3Rotation = pCharacter->GetRotation();

	if (fTargetRotation_Y < vec3Rotation.y())
	{
		fTargetRotation_Y += 360;
	}

	//	For lerp calculation
	//	Clockwise rotation
	if (fTargetRotation_Y - vec3Rotation.y() <= 180)
	{
		if (vec3Rotation.y() > fTargetRotation_Y)
			fTargetRotation_Y += 360;
	}
	//	CounterClockwise rotation
	else
	{
		if (vec3Rotation.y() < fTargetRotation_Y)
			vec3Rotation.setY(vec3Rotation.y() + 360);
	}

	float RotationTime = 0.05f;
	float fCurrent_Y = Util::Lerp(vec3Rotation.y(), fTargetRotation_Y, m_fDeltaTime / RotationTime);
	if (fCurrent_Y >= 360) fCurrent_Y -= 360;
	pCharacter->SetRotation(btVector3(vec3Rotation.x(), fCurrent_Y, vec3Rotation.z()));

	GameEvent::Rotation* pRotation = new GameEvent::Rotation();
	pRotation->m_fEventTime = m_lLastUpdateTime / 1000.0f;
	pRotation->m_nEntityID = pCharacter->GetID();
	pRotation->m_fStartTime = m_lLastUpdateTime / 1000.0f;
	pRotation->m_fEndTime = lUpdateTime / 1000.0f;
	pRotation->m_vec3StartRotation = vec3StartRotation;
	pRotation->m_vec3EndRotation = pCharacter->GetRotation();

	m_pGameRoom->AddGameEvent(pRotation);
	//	Rotation end

	btVector3 vec3Pos = pCharacter->GetPosition();
	btVector3 vec3ToMove = m_vec3Dest - vec3Pos;
	btVector3 vec3Moved = vec3ToMove.normalized() * pCharacter->GetDashSpeed() * m_fDeltaTime;
	btVector3 current = vec3Pos + vec3Moved;
	if (Util::IsEqual(vec3Moved, vec3ToMove) || vec3Moved.length2() > vec3ToMove.length2())
	{
		current = m_vec3Dest;
	}

	m_pGameRoom->EntityMove(pCharacter->GetID(), this, current, pCharacter->GetMoveCollisionTypes(), m_lLastUpdateTime, lUpdateTime);

	pCharacter->IncreaseDashPoint((m_pEntity->GetPosition() - vec3Pos).length());

	//	This behavior might be stopped already in EntityMove()
	if (!m_bActivated)
		return;

	bool bEnd = Util::IsEqual(vec3Moved, vec3ToMove) || vec3Moved.length2() > vec3ToMove.length2();
	if (bEnd)
	{
		current = m_vec3Dest;
		float elapsed = (current - vec3Pos).length() / pCharacter->GetDashSpeed();
		float fEndTime = m_lLastUpdateTime / 1000.0f + elapsed;

		Stop(fEndTime * 1000.0f);
	}
}