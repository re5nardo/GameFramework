#pragma once

#include "../CommonSources/Singleton.h"

class IBehavior;
class ISkill;
class IEntity;
class Character;
class IState;

class Factory : public Singleton<Factory>
{
public:
	Factory();
	virtual ~Factory();

public:
	ISkill*		CreateSkill(IEntity* pEntity, int nMasterDataID);
	IBehavior*	CreateBehavior(IEntity* pEntity, int nMasterDataID);
	IState*		CreateState(IEntity* pEntity, int nMasterDataID, __int64 lStartTime);
	Character*	CreateCharacter(int nID, int nMasterDataID);
};