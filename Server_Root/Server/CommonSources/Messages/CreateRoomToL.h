#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../../rapidjson/document.h"
#include "../../../rapidjson/stringbuffer.h"
#include "../../../rapidjson/writer.h"
#include "../../CommonSources/Message/MessageIDs.h"
#include "../../RoomServer/Data.h"
#include <vector>
#include <string>

using namespace rapidjson;

class CreateRoomToL : public IMessage
{
public:
	CreateRoomToL();
	virtual ~CreateRoomToL();

public:
	static const unsigned short MESSAGE_ID = CreateRoomToL_ID;

private:
	GenericStringBuffer<UTF8<>>*	m_buffer;
	Writer<StringBuffer, UTF8<>>*	m_writer;

public:
	int					m_nResult;				//  json field name : Result
	vector<string>		m_vecPlayers;			//  json field name : Players

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize() override;
	bool Deserialize(const char* pChar) override;
};

