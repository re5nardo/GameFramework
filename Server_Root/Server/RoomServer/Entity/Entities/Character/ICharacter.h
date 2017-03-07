#pragma once

#include "../../IEntity.h"
#include "../../../Data.h"

class ICharacter : public IEntity
{
public:
	ICharacter();
	virtual ~ICharacter();

protected:
	Stat m_DefaultStat;
	Stat m_CurrentStat;

public:
	virtual void Initialize() = 0;

public:
	float GetSpeed() override;

public:
	void SetStat(Stat stat);
};