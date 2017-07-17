#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../CommonSources/Message/MessageIDs.h"
#include "btBulletCollisionCommon.h"

class GameEventMoveToR : public IMessage
{
public:
	GameEventMoveToR();
	virtual ~GameEventMoveToR();

public:
	static const unsigned short MESSAGE_ID = GameEventMoveToR_ID;

public:
	int m_nPlayerIndex;
	btVector3 m_vec3Dest;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

