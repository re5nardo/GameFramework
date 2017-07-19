#pragma once

#include "../../../CommonSources/Message/IMessage.h"
#include "../../../CommonSources/Message/MessageIDs.h"
#include "../../Entity/IEntity.h"
#include <map>

class WorldSnapShotToC : public IMessage
{
public:
	WorldSnapShotToC();
	virtual ~WorldSnapShotToC();

public:
	static const unsigned short MESSAGE_ID = WorldSnapShotToC_ID;

public:
	int m_nTick;
	float m_fTime;
	map<int, IEntity*> m_mapEntity;
	//vector<IEntity*> m_vecEntity;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

