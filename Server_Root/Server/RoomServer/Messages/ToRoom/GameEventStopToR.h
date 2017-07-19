#pragma once

#include "../../../CommonSources/Message/IMessage.h"
#include "../../../CommonSources/Message/MessageIDs.h"
#include "btBulletCollisionCommon.h"

class GameEventStopToR : public IMessage
{
public:
	GameEventStopToR();
	virtual ~GameEventStopToR();

public:
	static const unsigned short MESSAGE_ID = GameEventStopToR_ID;

public:
	int m_nPlayerIndex;
	btVector3 m_vec3Pos;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

