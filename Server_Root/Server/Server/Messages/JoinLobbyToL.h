#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../CommonSources/Message/MessageIDs.h"
#include <string>
#ifdef max
#undef max
#undef min
#endif
#include "flatbuffers/flatbuffers.h"

using namespace flatbuffers;

class JoinLobbyToL : public IMessage
{
public:
	JoinLobbyToL();
	virtual ~JoinLobbyToL();

public:
	static const unsigned short MESSAGE_ID = JoinLobbyToL_ID;

private:
	FlatBufferBuilder m_Builder;

public:
	string m_strPlayerKey;
	int m_nAuthKey;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

