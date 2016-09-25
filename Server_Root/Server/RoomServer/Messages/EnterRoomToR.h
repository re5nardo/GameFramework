#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../../rapidjson/document.h"
#include "../../../rapidjson/stringbuffer.h"
#include "../../../rapidjson/writer.h"
#include "../../CommonSources/Message/MessageIDs.h"

using namespace rapidjson;

class EnterRoomToR : public IMessage
{
public:
	EnterRoomToR();
	virtual ~EnterRoomToR();

public:
	static const unsigned short MESSAGE_ID = EnterRoomToR_ID;

private:
	GenericStringBuffer<UTF8<>>*	m_buffer;
	Writer<StringBuffer, UTF8<>>*	m_writer;

public:
	string				m_strPlayerKey;			//	json field name : PlayerKey
	int					m_nAuthKey;             //  json field name : AuthKey
	int					m_nMatchID;				//  json field name : MatchID

public:
	unsigned short GetID() override;
	const char* Serialize() override;
	bool Deserialize(const char* pChar) override;
};

