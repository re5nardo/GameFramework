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
	__int64	m_lStartTime;			//	ElapsedTime after game was started (Milliseconds)
	__int64	m_lLastUpdateTime;		//	ElapsedTime after game was started (Milliseconds)
	__int64	m_lEndTime = -1;		//	ElapsedTime after game was started (Milliseconds)
	float m_fDeltaTime;				//	CurrentTime - PreviousTime (seconds)
	float m_fCurrentTime;			//	ElapsedTime after behavior was started (seconds)
	float m_fPreviousTime;			//	ElapsedTime after behavior was started (seconds)

public:
	int GetMasterDataID();

public:
	void Update(__int64 lUpdateTime);
	void Stop(__int64 lTime);

protected:
	virtual bool IsCoolTime(__int64 lTime) = 0;
	virtual bool IsValidToStart(__int64 lTime) = 0;
	virtual void Start(__int64 lStartTime, ...);

public:
	virtual void Initialize() = 0;
	virtual void UpdateBody(__int64 lUpdateTime) = 0;
	virtual void ProcessInput(__int64 lTime, BaeGameRoom* pBaeGameRoom, GameInputSkillToR* pMsg) = 0;

public:
	float GetTime();
	bool IsActivated();
};