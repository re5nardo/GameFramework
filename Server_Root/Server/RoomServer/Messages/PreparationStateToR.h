#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../CommonSources/Message/MessageIDs.h"
#ifdef max
#undef max
#undef min
#endif
#include "flatbuffers/flatbuffers.h"

using namespace flatbuffers;

class PreparationStateToR : public IMessage
{
public:
	PreparationStateToR();
	virtual ~PreparationStateToR();

public:
	static const unsigned short MESSAGE_ID = PreparationStateToR_ID;

private:
	FlatBufferBuilder m_Builder;

public:
	float m_fState;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

