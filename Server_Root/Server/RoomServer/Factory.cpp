#include "stdafx.h"
#include "Factory.h"
#include "Entity\Entities\Character\Character.h"
#include "MasterData\MasterDataManager.h"
#include "MasterData\Behavior.h"
#include "MasterData\Character.h"
#include "MasterData\Skill.h"
#include "MasterData\State.h"
#include "Behavior\Behaviors\Idle.h"
#include "Behavior\Behaviors\Move.h"
#include "Behavior\Behaviors\Dash.h"
#include "Behavior\Behaviors\AddState.h"
#include "Skill\Skills\ContinueSkill.h"
#include "Skill\Skills\FireSkill.h"
#include "State\States\Acceleration.h"
#include "State\States\Shield.h"

Factory::Factory()
{
}

Factory::~Factory()
{
}

ISkill* Factory::CreateSkill(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID)
{
	MasterData::Skill* pSkill = NULL;
	MasterDataManager::Instance()->GetData<MasterData::Skill>(nMasterDataID, pSkill);

	string strClassName = pSkill->m_strClassName;

	if (ContinueSkill::NAME.compare(strClassName) == 0)
	{
		return new ContinueSkill(pGameRoom, pEntity, nMasterDataID);
	}
	else if (FireSkill::NAME.compare(strClassName) == 0)
	{
		return new FireSkill(pGameRoom, pEntity, nMasterDataID);
	}

	return NULL;
}

IBehavior* Factory::CreateBehavior(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID)
{
	MasterData::Behavior* pMasterBehavior = NULL;
	MasterDataManager::Instance()->GetData<MasterData::Behavior>(nMasterDataID, pMasterBehavior);

	string strClassName = pMasterBehavior->m_strClassName;

	if (Idle::NAME.compare(strClassName) == 0)
	{
		return new Idle(pGameRoom, pEntity, nMasterDataID);
	}
	else if (Move::NAME.compare(strClassName) == 0)
	{
		return new Move(pGameRoom, pEntity, nMasterDataID);
	}
	else if (Dash::NAME.compare(strClassName) == 0)
	{
		return new Dash(pGameRoom, pEntity, nMasterDataID);
	}
	else if (AddState::NAME.compare(strClassName) == 0)
	{
		return new AddState(pGameRoom, pEntity, nMasterDataID);
	}

	return NULL;
}

IState* Factory::CreateState(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID, __int64 lStartTime)
{
	MasterData::State* pMasterState = NULL;
	MasterDataManager::Instance()->GetData<MasterData::State>(nMasterDataID, pMasterState);

	string strClassName = pMasterState->m_strClassName;

	if (Acceleration::NAME.compare(strClassName) == 0)
	{
		return new Acceleration(pEntity, nMasterDataID, lStartTime);
	}
	else if (Shield::NAME.compare(strClassName) == 0)
	{
		return new Shield(pEntity, nMasterDataID, lStartTime);
	}

	return NULL;
}

Character* Factory::CreateCharacter(BaeGameRoom* pGameRoom, int nID, int nMasterDataID)
{
	return new Character(pGameRoom, nID, nMasterDataID);
}