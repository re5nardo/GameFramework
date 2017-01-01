#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../../rapidjson/document.h"
#include "../../../rapidjson/stringbuffer.h"
#include "../../../rapidjson/writer.h"
#include "../../CommonSources/Message/MessageIDs.h"

using namespace rapidjson;

class ReadyForStartToR : public IMessage
{
public:
	ReadyForStartToR();
	virtual ~ReadyForStartToR();

public:
	static const unsigned short MESSAGE_ID = ReadyForStartToR_ID;

private:
	GenericStringBuffer<UTF8<>>*	m_buffer;
	Writer<StringBuffer, UTF8<>>*	m_writer;

public:
	int m_nPlayerIndex;		//	json field name : PlayerIndex

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize() override;
	bool Deserialize(const char* pChar) override;
};

