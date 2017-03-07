#include "stdafx.h"
#include "Idle.h"

Idle::Idle(IEntity* pEntity) : IBehavior(pEntity)
{
}

Idle::~Idle()
{
}

int Idle::GetID()
{
	return BEHAVIOR_ID;
}

void Idle::Initialize()
{
}

void Idle::Update(__int64 lUpdateTime)
{
	if (!m_bActivated)
		return;

	m_lLastUpdateTime = lUpdateTime;
}