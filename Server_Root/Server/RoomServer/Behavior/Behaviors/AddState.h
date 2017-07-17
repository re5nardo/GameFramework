#pragma once

#include "../IBehavior.h"
#include <string>

class BaeGameRoom;

using namespace std;

class AddState : public IBehavior
{
public:
	AddState(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID);
	virtual ~AddState();

public:
	static const string NAME;

private:
	float	m_fLength;
	int		m_nStateID;
	float	m_fTime;

public:
	void Start(__int64 lStartTime, ...) override;
	void Initialize() override;
	void Update(__int64 lUpdateTime) override;
};