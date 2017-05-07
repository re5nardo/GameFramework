#pragma once

#include "../ICharacter.h"

class MisterBae : public ICharacter
{
public:
	MisterBae(int nID);
	virtual ~MisterBae();

protected:
	void InitializeBehavior() override;

public:
	void Initialize() override;
};