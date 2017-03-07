#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../CommonSources/Message/MessageIDs.h"
#ifdef max
#undef max
#undef min
#endif
#include "flatbuffers/flatbuffers.h"
#include "WorldSnapShotToC_Data_generated.h"
#include "../Entity/IEntity.h"
#include <map>

using namespace flatbuffers;

class WorldSnapShotToC : public IMessage
{
public:
	WorldSnapShotToC();
	virtual ~WorldSnapShotToC();

public:
	static const unsigned short MESSAGE_ID = WorldSnapShotToC_ID;

private:
	FlatBufferBuilder m_Builder;

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

