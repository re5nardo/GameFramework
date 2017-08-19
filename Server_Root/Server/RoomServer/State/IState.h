#pragma once

class BaeGameRoom;
class IEntity;

class IState
{
public:
	IState(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID, long long lStartTime);
	virtual ~IState();

protected:
	int m_nMasterDataID = -1;

protected:
	float m_fLength;

protected:
	BaeGameRoom* m_pGameRoom;
	IEntity* m_pEntity;

protected:
	long long m_lStartTime;				//	ElapsedTime after game was started (Milliseconds)
	long long m_lLastUpdateTime;		//	ElapsedTime after game was started (Milliseconds)
	float m_fDeltaTime;					//	CurrentTime - PreviousTime (seconds)
	float m_fCurrentTime;				//	ElapsedTime after behavior was started (seconds)
	float m_fPreviousTime;				//	ElapsedTime after behavior was started (seconds)

public:
	int GetMasterDataID();

public:
	void Update(long long lUpdateTime);
	void Remove(long long lTime);

public:
	virtual int GetID() = 0;
	virtual void Initialize() = 0;
	virtual void UpdateBody(long long lUpdateTime) = 0;
};