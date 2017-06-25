#pragma once

#include <stdarg.h>

class IEntity;

class IBehavior
{
public:
	IBehavior(IEntity* pEntity, int nMasterDataID);
	virtual ~IBehavior();

protected:
	int m_nMasterDataID = -1;

protected:
	IEntity* m_pEntity;

protected:
	bool	m_bActivated = false;
	__int64	m_lStartTime;			//	ElapsedTime after game was started (Milliseconds)
	__int64	m_lLastUpdateTime;		//	ElapsedTime after game was started (Milliseconds)

public:
	int GetMasterDataID();

public:
	virtual void Initialize() = 0;
	virtual void Update(__int64 lUpdateTime) = 0;

public:
	virtual void Start(__int64 lStartTime, ...);
	virtual void Stop();

public:
	float GetTime();
	bool IsActivated();
};