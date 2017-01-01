#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../../rapidjson/document.h"
#include "../../../rapidjson/stringbuffer.h"
#include "../../../rapidjson/writer.h"
#include "../../CommonSources/Message/MessageIDs.h"
#include "../../RoomServer/Data.h"

using namespace rapidjson;

class GameEventTeleportToR : public IMessage
{
public:
	GameEventTeleportToR();
	virtual ~GameEventTeleportToR();

public:
	static const unsigned short MESSAGE_ID = GameEventTeleportToR_ID;

private:
	GenericStringBuffer<UTF8<>>*	m_buffer;
	Writer<StringBuffer, UTF8<>>*	m_writer;

public:
	int m_nPlayerIndex;					//  json field name : PlayerIndex
	Vector3 m_vec3Start;				//  Json field name : Start_X, Start_Y, Start_Z
	Vector3 m_vec3Dest;					//  Json field name : Dest_X, Dest_Y, Dest_Z
	int m_nState;						//  Json field name : State


public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize() override;
	bool Deserialize(const char* pChar) override;
};

