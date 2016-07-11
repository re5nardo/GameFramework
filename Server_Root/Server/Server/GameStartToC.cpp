#include "stdafx.h"
#include "GameStartToC.h"
#include "NetworkDefines.h"
#include "../../rapidjson/document.h"
#include "../../rapidjson/stringbuffer.h"
#include "../../rapidjson/writer.h"

using namespace rapidjson;


GameStartToC::GameStartToC()
{
}


GameStartToC::~GameStartToC()
{
}

unsigned short GameStartToC::GetID()
{
	return (unsigned short)Messages::Game_Start_ToC;
}

string GameStartToC::Serialize()
{
	Document document;
	document.SetObject();

	Value GameElapsedTime(m_lGameElapsedTime);
	document.AddMember("GameElapsedTime", GameElapsedTime, document.GetAllocator());

	GenericStringBuffer<UTF8<>> buffer;
	Writer<StringBuffer, UTF8<>> writer(buffer);
	document.Accept(writer);

	return buffer.GetString();
}

bool GameStartToC::Deserialize(string strJson)
{
	Document document;
	document.Parse<0>(strJson.c_str());
	if (!document.IsObject())
	{
		return false;
	}

	if (document.HasMember("GameElapsedTime") && document["GameElapsedTime"].IsUint64())
	{
		m_lGameElapsedTime = document["GameElapsedTime"].GetUint64();
	}
	else
	{
		return false;
	}

	return true;
}