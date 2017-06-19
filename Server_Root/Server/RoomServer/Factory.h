#pragma once

#include "../CommonSources/Singleton.h"

class IBehavior;
class ISkill;
class IEntity;
class Character;

class Factory : public Singleton<Factory>
{
public:
	Factory();
	virtual ~Factory();

public:
	ISkill*		CreateSkill(IEntity* pEntity, int nMasterDataID);
	IBehavior*	CreateBehavior(IEntity* pEntity, int nMasterDataID);
	Character*	CreateCharacter(int nID, int nMasterDataID);
};