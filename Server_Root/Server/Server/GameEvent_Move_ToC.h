#pragma once

#include "IMessage.h"
#include "../../rapidjson/document.h"
#include "../../rapidjson/stringbuffer.h"
#include "../../rapidjson/writer.h"
#include "Data.h"

using namespace rapidjson;

class GameEvent_Move_ToC : public IMessage
{
public:
	GameEvent_Move_ToC();
	virtual ~GameEvent_Move_ToC();

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

