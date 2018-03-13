#pragma once

#include "../../../CommonSources/Message/IMessage.h"
#include "../../../CommonSources/Message/MessageIDs.h"
#include <vector>
#include "../../../FBSFiles/GameResultToR_generated.h"

class GameResultToR : public IMessage
{
public:
	GameResultToR();
	virtual ~GameResultToR();

public:
	static const unsigned short MESSAGE_ID = GameResultToR_ID;

public:
	vector<FBS::Data::PlayerRankInfo> m_vecPlayerRankInfo;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

