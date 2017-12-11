#include "stdafx.h"
#include "Move.h"
#include "../../Entity/IEntity.h"
#include <math.h>
#include "../../Util.h"
#include "../../Game/BaeGameRoom.h"
#include "../../GameEvent/GameEvents/Position.h"
#include "../../GameEvent/GameEvents/Rotation.h"
#include "../BehaviorIDs.h"

const string Move::NAME = "Move";

Move::Move(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID) : IBehavior(pGameRoom, pEntity, nMasterDataID)
{
}

Move::~Move()
{
}

void Move::Start(long long lStartTime, ...)
{
	__super::Start(lStartTime);

	va_list ap;
	va_start(ap, lStartTime);
	btVector3* pVec3Dest = va_arg(ap, btVector3*);
	m_vec3Dest = *pVec3Dest;
	va_end(ap);

	if (m_pEntity->GetBehavior(BehaviorID::IDLE) != NULL && m_pEntity->GetBehavior(BehaviorID::IDLE)->IsActivated())
		m_pEntity->GetBehavior(BehaviorID::IDLE)->Stop(lStartTime);
}

void Move::Initialize()
{
}

void Move::UpdateBody(long long lUpdateTime)
{
	if (m_fDeltaTime == 0)
		return;

	////	Rotation
	//btVector3 vec3StartRotation = m_pEntity->GetRotation();
	//btVector3 vec3Move = m_vec3Dest - m_pEntity->GetPosition();
	//float fTargetRotation_Y = Util::GetAngle_Y(vec3Move);
	//btVector3 vec3Rotation = m_pEntity->GetRotation();

	//if (fTargetRotation_Y < vec3Rotation.y())
	//{
	//	fTargetRotation_Y += 360;
	//}

	////	For lerp calculation
	////	Clockwise rotation
	//if (fTargetRotation_Y - vec3Rotation.y() <= 180)
	//{
	//	if (vec3Rotation.y() > fTargetRotation_Y)
	//		fTargetRotation_Y += 360;
	//}
	////	CounterClockwise rotation
	//else
	//{
	//	if (vec3Rotation.y() < fTargetRotation_Y)
	//		vec3Rotation.setY(vec3Rotation.y() + 360);
	//}

	//float RotationTime = 0.05f;
	//float fCurrent_Y = Util::Lerp(vec3Rotation.y(), fTargetRotation_Y, m_fDeltaTime / RotationTime);
	//if (fCurrent_Y >= 360) fCurrent_Y -= 360;
	//m_pEntity->SetRotation(btVector3(vec3Rotation.x(), fCurrent_Y, vec3Rotation.z()));

	//GameEvent::Rotation* pRotation = new GameEvent::Rotation();
	//pRotation->m_fEventTime = m_lLastUpdateTime / 1000.0f;
	//pRotation->m_nEntityID = m_pEntity->GetID();
	//pRotation->m_fStartTime = m_lLastUpdateTime / 1000.0f;
	//pRotation->m_fEndTime = lUpdateTime / 1000.0f;
	//pRotation->m_vec3StartRotation = vec3StartRotation;
	//pRotation->m_vec3EndRotation = m_pEntity->GetRotation();

	//m_pGameRoom->AddGameEvent(pRotation);
	////	Rotation end

	btVector3 vec3Pos = m_pEntity->GetPosition();
	btVector3 vec3ToMove = m_vec3Dest - vec3Pos;
	btVector3 vec3Moved = vec3ToMove.normalized() * m_pEntity->GetSpeed() * m_fDeltaTime;
	btVector3 current = vec3Pos + vec3Moved;
	if (Util::IsEqual(vec3Moved, vec3ToMove) || vec3Moved.length2() > vec3ToMove.length2())
	{
		current = m_vec3Dest;
	}

	m_pGameRoom->EntityMove(m_pEntity->GetID(), this, current, m_pEntity->GetMoveCollisionTypes(), m_lLastUpdateTime, lUpdateTime);

	if (m_pEntity->GetEntityType() == FBS::Data::EntityType::EntityType_Character)
	{
		((Character*)m_pEntity)->OnMoved((m_pEntity->GetPosition() - vec3Pos).length(), lUpdateTime);
	}

	//	This behavior might be stopped already in EntityMove()
	if (!m_bActivated)
		return;

	bool bEnd = Util::IsEqual(vec3Moved, vec3ToMove) || vec3Moved.length2() > vec3ToMove.length2();
	if (bEnd || m_pEntity->GetSpeed() <= 0)
	{
		current = m_vec3Dest;
		float elapsed = (current - vec3Pos).length() / m_pEntity->GetSpeed();
		float fEndTime = m_lLastUpdateTime / 1000.0f + elapsed;

		Stop(fEndTime * 1000.0f);
	}
}