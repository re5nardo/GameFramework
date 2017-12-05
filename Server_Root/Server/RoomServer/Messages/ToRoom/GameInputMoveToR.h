#pragma once

#include "../../../CommonSources/Message/IMessage.h"
#include "../../../CommonSources/Message/MessageIDs.h"
#include "../../../FBSFiles/GameInputMoveToR_generated.h"

class GameInputMoveToR : public IMessage
{
public:
	GameInputMoveToR();
	virtual ~GameInputMoveToR();

public:
	static const unsigned short MESSAGE_ID = GameInputMoveToR_ID;

public:
	int m_nPlayerIndex;
	FBS::MoveDirection m_Direction;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

