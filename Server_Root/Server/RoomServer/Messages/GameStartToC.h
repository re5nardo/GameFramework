#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../rapidjson/document.h"
#include "../../rapidjson/stringbuffer.h"
#include "../../rapidjson/writer.h"

using namespace rapidjson;

class GameStartToC : public IMessage
{
public:
	GameStartToC();
	virtual ~GameStartToC();

private:
	GenericStringBuffer<UTF8<>>*	m_buffer;
	Writer<StringBuffer, UTF8<>>*	m_writer;

public:
	unsigned __int64 m_lGameElapsedTime;		//	json field name : GameElapsedTime

public:
	unsigned short GetID() override;
	const char* Serialize() override;
	bool Deserialize(const char* pChar) override;
};

