#pragma once

#include "CoreState.h"
#include <vector>

class BaeGameRoom;
class IEntity;

using namespace std;

class IState
{
public:
	IState(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID, long long lStartTime);
	virtual ~IState();

protected:
	int m_nMasterDataID = -1;

protected:
	float m_fLength;
	vector<CoreState> m_vecCoreState;

protected:
	BaeGameRoom* m_pGameRoom;
	IEntity* m_pEntity;

protected:
	long long m_lStartTime;					//	ElapsedTime after game was started (Milliseconds)
	long long m_lLastUpdateTime = 1;		//	ElapsedTime after game was started (Milliseconds)
	float m_fDeltaTime;						//	CurrentTime - PreviousTime (seconds)
	float m_fCurrentTime;					//	ElapsedTime after behavior was started (seconds)
	float m_fPreviousTime;					//	ElapsedTime after behavior was started (seconds)

public:
	bool m_bDestroyReserved = false;

public:
	int GetMasterDataID();
	long long GetStartTime();

public:
	void Update(long long lUpdateTime);

public:
	bool HasCoreState(CoreState coreState);

public:
	virtual int GetID() = 0;
	virtual void Initialize() = 0;
	virtual void UpdateBody(long long lUpdateTime) = 0;

public:
	virtual void OnCollision(IEntity* pOther, long long lTime);
};