#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../../rapidjson/document.h"
#include "../../../rapidjson/stringbuffer.h"
#include "../../../rapidjson/writer.h"
#include "../../CommonSources/Message/MessageIDs.h"

using namespace rapidjson;

class PreparationStateToR : public IMessage
{
public:
	PreparationStateToR();
	virtual ~PreparationStateToR();

public:
	static const unsigned short MESSAGE_ID = PreparationStateToR_ID;

private:
	GenericStringBuffer<UTF8<>>*	m_buffer;
	Writer<StringBuffer, UTF8<>>*	m_writer;

public:
	float				m_fState;				//	json field name : State

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize() override;
	bool Deserialize(const char* pChar) override;
};
