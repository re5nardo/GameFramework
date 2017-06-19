#pragma once

class IEntity;

class IState
{
public:
	IState(IEntity* pEntity, __int64 lStartTime);
	virtual ~IState();

protected:
	IEntity* m_pEntity = NULL;

protected:
	__int64	m_lStartTime = 0;			//	ElapsedTime after game was started (Milliseconds)
	__int64	m_lLastUpdateTime = 0;		//	ElapsedTime after game was started (Milliseconds)

public:
	virtual int GetID() = 0;
	virtual void Update(__int64 lUpdateTime) = 0;
};