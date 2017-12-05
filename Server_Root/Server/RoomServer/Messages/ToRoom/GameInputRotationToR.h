#pragma once

#include "../../../CommonSources/Message/IMessage.h"
#include "../../../CommonSources/Message/MessageIDs.h"
#include "btBulletCollisionCommon.h"

class GameInputRotationToR : public IMessage
{
public:
	GameInputRotationToR();
	virtual ~GameInputRotationToR();

public:
	static const unsigned short MESSAGE_ID = GameInputRotationToR_ID;

public:
	int m_nPlayerIndex;
	btVector3 m_Rotation;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

