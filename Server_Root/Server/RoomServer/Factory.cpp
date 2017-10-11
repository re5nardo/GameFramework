#include "stdafx.h"
#include "Factory.h"
#include "Entity\Entities\Character\Character.h"
//#include "Entity\Entities\Character\CharacterAI.h"
#include "MasterData\MasterDataManager.h"
#include "MasterData\Behavior.h"
#include "MasterData\Character.h"
#include "MasterData\Skill.h"
#include "MasterData\State.h"
#include "Behavior\Behaviors\General.h"
#include "Behavior\Behaviors\Move.h"
#include "Behavior\Behaviors\Dash.h"
#include "Behavior\Behaviors\Die.h"
#include "Skill\Skills\ContinueSkill.h"
#include "Skill\Skills\FireSkill.h"
#include "State\States\Acceleration.h"
#include "State\States\Shield.h"
#include "State\States\FireBehavior.h"
#include "State\States\GeneralState.h"
#include "State\States\ChallengerDisturbing.h"
#include "Entity\Entities\Projectile\Projectile.h"

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

	if (General::NAME.compare(strClassName) == 0)
	{
		return new General(pGameRoom, pEntity, nMasterDataID);
	}
	else if (Move::NAME.compare(strClassName) == 0)
	{
		return new Move(pGameRoom, pEntity, nMasterDataID);
	}
	else if (Dash::NAME.compare(strClassName) == 0)
	{
		return new Dash(pGameRoom, pEntity, nMasterDataID);
	}
	else if (Die::NAME.compare(strClassName) == 0)
	{
		return new Die(pGameRoom, pEntity, nMasterDataID);
	}

	return NULL;
}

IState* Factory::CreateState(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID, long long lStartTime)
{
	MasterData::State* pMasterState = NULL;
	MasterDataManager::Instance()->GetData<MasterData::State>(nMasterDataID, pMasterState);

	string strClassName = pMasterState->m_strClassName;

	if (Acceleration::NAME.compare(strClassName) == 0)
	{
		return new Acceleration(pGameRoom, pEntity, nMasterDataID, lStartTime);
	}
	else if (Shield::NAME.compare(strClassName) == 0)
	{
		return new Shield(pGameRoom, pEntity, nMasterDataID, lStartTime);
	}
	else if (FireBehavior::NAME.compare(strClassName) == 0)
	{
		return new FireBehavior(pGameRoom, pEntity, nMasterDataID, lStartTime);
	}
	else if (GeneralState::NAME.compare(strClassName) == 0)
	{
		return new GeneralState(pGameRoom, pEntity, nMasterDataID, lStartTime);
	}
	else if (ChallengerDisturbing::NAME.compare(strClassName) == 0)
	{
		return new ChallengerDisturbing(pGameRoom, pEntity, nMasterDataID, lStartTime);
	}

	return NULL;
}

Character* Factory::CreateCharacter(BaeGameRoom* pGameRoom, int nID, int nMasterDataID, Character::Role role)
{
	return new Character(pGameRoom, nID, nMasterDataID, role);
}

//CharacterAI* Factory::CreateCharacterAI(BaeGameRoom* pGameRoom, int nID, int nMasterDataID)
//{
//	return new CharacterAI(pGameRoom, nID, nMasterDataID);
//}

Projectile* Factory::CreateProjectile(BaeGameRoom* pGameRoom, int nProjectorID, int nID, int nMasterDataID)
{
	return new Projectile(pGameRoom, nProjectorID, nID, nMasterDataID);
}