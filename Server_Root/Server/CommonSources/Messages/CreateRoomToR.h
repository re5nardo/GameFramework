#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../rapidjson/document.h"
#include "../../rapidjson/stringbuffer.h"
#include "../../rapidjson/writer.h"
#include "../../CommonSources/Message/MessageIDs.h"
#include "Data.h"
#include <vector>
#include <string>

using namespace rapidjson;

class CreateRoomToR : public IMessage
{
public:
	CreateRoomToR();
	virtual ~CreateRoomToR();

public:
	static const unsigned short MESSAGE_ID = CreateRoomToR_ID;

private:
	GenericStringBuffer<UTF8<>>*	m_buffer;
	Writer<StringBuffer, UTF8<>>*	m_writer;

public:
	int					m_nMatchID;				//  json field name : MatchID
	vector<string>		m_vecPlayers;			//  json field name : Players

public:
	unsigned short GetID() override;
	const char* Serialize() override;
	bool Deserialize(const char* pChar) override;
};

