#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../../rapidjson/document.h"
#include "../../../rapidjson/stringbuffer.h"
#include "../../../rapidjson/writer.h"
#include "../../CommonSources/Message/MessageIDs.h"

using namespace rapidjson;

class PlayerEnterRoomToC : public IMessage
{
public:
	PlayerEnterRoomToC();
	virtual ~PlayerEnterRoomToC();

public:
	static const unsigned short MESSAGE_ID = PlayerEnterRoomToC_ID;

private:
	GenericStringBuffer<UTF8<>>*	m_buffer;
	Writer<StringBuffer, UTF8<>>*	m_writer;

public:
	int		m_nPlayerIndex;			//  json field name : PlayerIndex
	string	m_strCharacterID;		//  json field name : CharacterID

public:
	unsigned short GetID() override;
	const char* Serialize() override;
	bool Deserialize(const char* pChar) override;
};

