#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../rapidjson/document.h"
#include "../../rapidjson/stringbuffer.h"
#include "../../rapidjson/writer.h"
#include "Data.h"

using namespace rapidjson;

class GameEventMoveToS : public IMessage
{
public:
	GameEventMoveToS();
	virtual ~GameEventMoveToS();

public:
	static const unsigned short MESSAGE_ID = 20001;

private:
	GenericStringBuffer<UTF8<>>*	m_buffer;
	Writer<StringBuffer, UTF8<>>*	m_writer;

public:
	int m_nPlayerIndex;				//  json field name : PlayerIndex
	int m_nElapsedTime;				//  json field name : ElapsedTime
	Vector3 m_vec3Dest;				//  json field name : Pos_X, Pos_Y, Pos_Z

public:
	unsigned short GetID() override;
	const char* Serialize() override;
	bool Deserialize(const char* pChar) override;
};
