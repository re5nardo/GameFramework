#pragma once

#include "../../../CommonSources/Message/IMessage.h"
#include "../../../CommonSources/Message/MessageIDs.h"
#include <vector>
#include "../../../FBSFiles/GameEndToC_generated.h"

class GameEndToC : public IMessage
{
public:
	GameEndToC();
	virtual ~GameEndToC();

public:
	static const unsigned short MESSAGE_ID = GameEndToC_ID;

public:
	vector<FBS::Data::PlayerRankInfo> m_vecPlayerRankInfo;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

