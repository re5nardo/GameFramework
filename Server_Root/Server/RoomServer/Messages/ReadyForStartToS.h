#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../rapidjson/document.h"
#include "../../rapidjson/stringbuffer.h"
#include "../../rapidjson/writer.h"

using namespace rapidjson;

class ReadyForStartToS : public IMessage
{
public:
	ReadyForStartToS();
	virtual ~ReadyForStartToS();

private:
	GenericStringBuffer<UTF8<>>*	m_buffer;
	Writer<StringBuffer, UTF8<>>*	m_writer;

public:
	int m_nPlayerIndex;		//	json field name : PlayerIndex

public:
	unsigned short GetID() override;
	const char* Serialize() override;
	bool Deserialize(const char* pChar) override;
};

