#pragma once

#include <stdarg.h>

class IEntity;
class BaeGameRoom;

class ISkill
{
public:
	ISkill(IEntity* pEntity, int nMasterDataID);
	virtual ~ISkill();

protected:
	int m_nMasterDataID = -1;

protected:
	IEntity* m_pEntity;

protected:
	bool	m_bActivated = false;
	__int64	m_lStartTime;			//	ElapsedTime after game was started (Milliseconds)
	__int64	m_lLastUpdateTime;		//	ElapsedTime after game was started (Milliseconds)
	__int64	m_lEndTime = -1;		//	ElapsedTime after game was started (Milliseconds)

protected:
	virtual bool IsCoolTime(__int64 lTime) = 0;
	virtual bool IsValidToStart(__int64 lTime) = 0;
	virtual void Start(__int64 lStartTime, ...);

public:
	int GetMasterDataID();
	virtual void Initialize() = 0;
	virtual void Update(__int64 lUpdateTime) = 0;
	virtual void ProcessInput(__int64 lTime, BaeGameRoom* pBaeGameRoom) = 0;

public:
	float GetTime();
	bool IsActivated();
};