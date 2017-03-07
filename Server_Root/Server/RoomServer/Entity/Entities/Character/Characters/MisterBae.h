#pragma once

#include "../ICharacter.h"

class MisterBae : public ICharacter
{
public:
	MisterBae();
	virtual ~MisterBae();

protected:
	void InitializeBehavior() override;

public:
	void Initialize() override;
};