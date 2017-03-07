#include "stdafx.h"
#include "MisterBae.h"
#include "../../../../Behavior/Behaviors/Idle.h"
#include "../../../../Behavior/Behaviors/Move.h"

MisterBae::MisterBae()
{
	InitializeBehavior();
}

MisterBae::~MisterBae()
{
}

void MisterBae::InitializeBehavior()
{
	m_listBehavior.push_back(new Idle(this));
	m_listBehavior.push_back(new Move(this));
}

void MisterBae::Initialize()
{
}
