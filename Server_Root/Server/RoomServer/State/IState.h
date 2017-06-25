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

public:
	int GetMasterDataID();

public:
	virtual int GetID() = 0;
	virtual void Initialize() = 0;
	virtual void Update(__int64 lUpdateTime) = 0;
};