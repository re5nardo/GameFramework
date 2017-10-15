#pragma once

#include "../../../CommonSources/Message/IMessage.h"
#include "../../../CommonSources/Message/MessageIDs.h"
#include "btBulletCollisionCommon.h"

class GameEventRunToR : public IMessage
{
public:
	GameEventRunToR();
	virtual ~GameEventRunToR();

public:
	static const unsigned short MESSAGE_ID = GameEventRunToR_ID;

public:
	int m_nPlayerIndex;
	btVector3 m_vec3Dest;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

