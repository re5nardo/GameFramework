#pragma once

class BaeGameRoom;
class Character;

class CharacterAI
{
public:
	CharacterAI(BaeGameRoom* pGameRoom, int nID, int nMasterDataID);
	virtual ~CharacterAI();

protected:
	BaeGameRoom* m_pGameRoom;
	Character* m_pCharacter;

protected:
	int m_nID = -1;
	int m_nMasterDataID = -1;

public:
	virtual void Initialize();
	virtual void Update(long long lUpdateTime);

public:
	Character* GetCharacter();
};