#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../rapidjson/document.h"
#include "../../rapidjson/stringbuffer.h"
#include "../../rapidjson/writer.h"
#include "../../CommonSources/Message/MessageIDs.h"
#include "Data.h"

using namespace rapidjson;

class GameEventStopToR : public IMessage
{
public:
	GameEventStopToR();
	virtual ~GameEventStopToR();

public:
	static const unsigned short MESSAGE_ID = GameEventStopToR_ID;

private:
	GenericStringBuffer<UTF8<>>*	m_buffer;
	Writer<StringBuffer, UTF8<>>*	m_writer;

public:
	int m_nPlayerIndex;				//  json field name : PlayerIndex
	Vector3 m_vec3Pos;				//  json field name : Pos_X, Pos_Y, Pos_Z

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize() override;
	bool Deserialize(const char* pChar) override;
};

