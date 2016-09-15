#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../../rapidjson/document.h"
#include "../../../rapidjson/stringbuffer.h"
#include "../../../rapidjson/writer.h"

using namespace rapidjson;

class SelectNormalGameToC : public IMessage
{
public:
	SelectNormalGameToC();
	virtual ~SelectNormalGameToC();

public:
	static const unsigned short MESSAGE_ID = 10001;

private:
	GenericStringBuffer<UTF8<>>*	m_buffer;
	Writer<StringBuffer, UTF8<>>*	m_writer;

public:
	int m_nResult;           //  json field name : Result
	int m_nExpectedTime;     //  json field name : ExpectedTime

public:
	unsigned short GetID() override;
	const char* Serialize() override;
	bool Deserialize(const char* pChar) override;
};
