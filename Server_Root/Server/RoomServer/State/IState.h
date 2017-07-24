#pragma once

class IEntity;

class IState
{
public:
	IState(IEntity* pEntity, int nMasterDataID, __int64 lStartTime);
	virtual ~IState();

protected:
	int m_nMasterDataID = -1;

protected:
	IEntity* m_pEntity;

protected:
	__int64	m_lStartTime = 0;			//	ElapsedTime after game was started (Milliseconds)
	__int64	m_lLastUpdateTime = 0;		//	ElapsedTime after game was started (Milliseconds)
	float m_fDeltaTime;					//	CurrentTime - PreviousTime (seconds)
	float m_fCurrentTime;				//	ElapsedTime after behavior was started (seconds)
	float m_fPreviousTime;				//	ElapsedTime after behavior was started (seconds)

public:
	int GetMasterDataID();

public:
	void Update(__int64 lUpdateTime);
	void Remove(__int64 lTime);

public:
	virtual int GetID() = 0;
	virtual void Initialize() = 0;
	virtual void UpdateBody(__int64 lUpdateTime) = 0;
};