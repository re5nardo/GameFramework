#pragma once

#include <stdarg.h>

class IEntity;
class BaeGameRoom;

class IBehavior
{
public:
	IBehavior(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID);
	virtual ~IBehavior();

protected:
	int m_nMasterDataID = -1;

protected:
	BaeGameRoom* m_pGameRoom;
	IEntity* m_pEntity;

protected:
	bool m_bActivated = false;

protected:
	long long m_lStartTime;			//	ElapsedTime after game was started (Milliseconds)
	long long m_lLastUpdateTime;	//	ElapsedTime after game was started (Milliseconds)
	float m_fDeltaTime;				//	CurrentTime - PreviousTime (seconds)
	float m_fCurrentTime;			//	ElapsedTime after behavior was started (seconds)
	float m_fPreviousTime;			//	ElapsedTime after behavior was started (seconds)

public:
	int GetMasterDataID();

public:
	void Update(long long lUpdateTime);
	void Stop(long long lTime);

public:
	virtual void Initialize() = 0;
	virtual void UpdateBody(long long lUpdateTime) = 0;

public:
	virtual void Start(long long lStartTime, ...);

public:
	float GetTime();
	bool IsActivated();
};