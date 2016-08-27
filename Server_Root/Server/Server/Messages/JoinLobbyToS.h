#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../../rapidjson/document.h"
#include "../../../rapidjson/stringbuffer.h"
#include "../../../rapidjson/writer.h"

using namespace rapidjson;

class JoinLobbyToS : public IMessage
{
public:
	JoinLobbyToS();
	virtual ~JoinLobbyToS();

private:
	GenericStringBuffer<UTF8<>>*	m_buffer;
	Writer<StringBuffer, UTF8<>>*	m_writer;

public:
	string				m_strPlayerKey;			//	json field name : PlayerKey
	int					m_nAuthKey;             //  json field name : AuthKey

public:
	unsigned short GetID() override;
	const char* Serialize() override;
	bool Deserialize(const char* pChar) override;
};

