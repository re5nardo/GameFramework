#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../CommonSources/Message/MessageIDs.h"
#ifdef max
#undef max
#undef min
#endif
#include "flatbuffers/flatbuffers.h"

using namespace flatbuffers;

class SelectNormalGameToC : public IMessage
{
public:
	SelectNormalGameToC();
	virtual ~SelectNormalGameToC();

public:
	static const unsigned short MESSAGE_ID = SelectNormalGameToC_ID;

private:
	FlatBufferBuilder m_Builder;

public:
	int m_nResult;
	int m_nExpectedTime;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

