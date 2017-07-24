#pragma once

#include <stdarg.h>

class IEntity;
class BaeGameRoom;
class GameInputSkillToR;
class BaeGameRoom;

class ISkill
{
public:
	ISkill(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID);
	virtual ~ISkill();

protected:
	int m_nMasterDataID = -1;

protected:
	BaeGameRoom* m_pGameRoom;
	IEntity* m_pEntity;

protected:
	bool	m_bActivated = false;

protected:
	long long m_lStartTime;			//	ElapsedTime after game was started (Milliseconds)
	long long m_lLastUpdateTime;	//	ElapsedTime after game was started (Milliseconds)
	long long m_lEndTime = -1;		//	ElapsedTime after game was started (Milliseconds)
	float m_fDeltaTime;				//	CurrentTime - PreviousTime (seconds)
	float m_fCurrentTime;			//	ElapsedTime after behavior was started (seconds)
	float m_fPreviousTime;			//	ElapsedTime after behavior was started (seconds)

public:
	int GetMasterDataID();

public:
	void Update(long long lUpdateTime);
	void Stop(long long lTime);

protected:
	virtual bool IsCoolTime(long long lTime) = 0;
	virtual bool IsValidToStart(long long lTime) = 0;
	virtual void Start(long long lStartTime, ...);

public:
	virtual void Initialize() = 0;
	virtual void UpdateBody(long long lUpdateTime) = 0;
	virtual void ProcessInput(long long lTime, BaeGameRoom* pBaeGameRoom, GameInputSkillToR* pMsg) = 0;

public:
	float GetTime();
	bool IsActivated();
};