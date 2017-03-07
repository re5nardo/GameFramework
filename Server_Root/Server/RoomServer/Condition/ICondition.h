#pragma once

class ICondition
{
public:
	ICondition();
	virtual ~ICondition();

public:
	virtual int GetID() = 0;
};