#pragma once

#include "../CommonSources/Singleton.h"

class IBehavior;
class ISkill;
class IEntity;
class Character;
class IState;
class BaeGameRoom;

class Factory : public Singleton<Factory>
{
public:
	Factory();
	virtual ~Factory();

public:
	ISkill*		CreateSkill(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID);
	IBehavior*	CreateBehavior(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID);
	IState*		CreateState(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID, __int64 lStartTime);
	Character*	CreateCharacter(BaeGameRoom* pGameRoom, int nID, int nMasterDataID);
};