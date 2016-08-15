#pragma once

#include "IMessage.h"
#include "../../rapidjson/document.h"
#include "../../rapidjson/stringbuffer.h"
#include "../../rapidjson/writer.h"

using namespace rapidjson;

class SelectNormalGameToC : public IMessage
{
public:
	SelectNormalGameToC();
	virtual ~SelectNormalGameToC();

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

