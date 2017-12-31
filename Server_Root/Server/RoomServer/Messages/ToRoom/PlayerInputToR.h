#pragma once

#include "../../../CommonSources/Message/IMessage.h"
#include "../../../CommonSources/Message/MessageIDs.h"
#include "../../../FBSFiles/PlayerInputToR_generated.h"


class IPlayerInput;

class PlayerInputToR : public IMessage
{
public:
	PlayerInputToR();
	virtual ~PlayerInputToR();

public:
	static const unsigned short MESSAGE_ID = PlayerInputToR_ID;

public:
	FBS::PlayerInputType m_Type;
	IPlayerInput* m_PlayerInput = NULL;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

