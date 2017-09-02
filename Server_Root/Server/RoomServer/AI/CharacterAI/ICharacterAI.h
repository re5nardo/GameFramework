#pragma once

class BaeGameRoom;
class Character;

class ICharacterAI
{
public:
	ICharacterAI(BaeGameRoom* pGameRoom, int nMasterDataID, long long lStartTime);
	virtual ~ICharacterAI();

protected:
	int m_nMasterDataID = -1;

protected:
	BaeGameRoom* m_pGameRoom = NULL;
	Character* m_pCharacter = NULL;

protected:
	long long m_lStartTime;					//	ElapsedTime after game was started (Milliseconds)
	long long m_lLastUpdateTime = -1;		//	ElapsedTime after game was started (Milliseconds)
	float m_fDeltaTime;						//	CurrentTime - PreviousTime (seconds)
	float m_fCurrentTime;					//	ElapsedTime after behavior was started (seconds)
	float m_fPreviousTime;					//	ElapsedTime after behavior was started (seconds)

public:
	int GetMasterDataID();
	Character* GetCharacter();

public:
	void Update(long long lUpdateTime);

public:
	virtual void UpdateBody(long long lUpdateTime) = 0;
};