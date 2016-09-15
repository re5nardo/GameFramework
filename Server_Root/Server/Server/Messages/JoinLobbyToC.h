#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../../rapidjson/document.h"
#include "../../../rapidjson/stringbuffer.h"
#include "../../../rapidjson/writer.h"

using namespace rapidjson;

class JoinLobbyToC : public IMessage
{
public:
	JoinLobbyToC();
	virtual ~JoinLobbyToC();

public:
	static const unsigned short MESSAGE_ID = 10000;

private:
	GenericStringBuffer<UTF8<>>*	m_buffer;
	Writer<StringBuffer, UTF8<>>*	m_writer;

public:
	int m_nResult;     //  json field name : Result

public:
	unsigned short GetID() override;
	const char* Serialize() override;
	bool Deserialize(const char* pChar) override;
};

