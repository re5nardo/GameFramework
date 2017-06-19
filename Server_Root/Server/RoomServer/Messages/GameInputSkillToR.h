#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../CommonSources/Message/MessageIDs.h"
#include "btBulletCollisionCommon.h"
#ifdef max
#undef max
#undef min
#endif
#include "flatbuffers/flatbuffers.h"
#include <vector>

using namespace flatbuffers;

class GameInputSkillToR : public IMessage
{
public:
	GameInputSkillToR();
	virtual ~GameInputSkillToR();

public:
	static const unsigned short MESSAGE_ID = GameInputSkillToR_ID;

private:
	FlatBufferBuilder m_Builder;

public:
	int m_nPlayerIndex;
	int m_nSkillID;
	vector<btVector3> m_vecVector3;
	vector<int> m_vecInt;
	vector<float> m_vecFloat;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

