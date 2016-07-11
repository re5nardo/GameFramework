#include "stdafx.h"
#include "ReadyForStartToS.h"
#include "NetworkDefines.h"
#include "../../rapidjson/document.h"
#include "../../rapidjson/stringbuffer.h"
#include "../../rapidjson/writer.h"

using namespace rapidjson;


ReadyForStartToS::ReadyForStartToS()
{
}


ReadyForStartToS::~ReadyForStartToS()
{
}

unsigned short ReadyForStartToS::GetID()
{
	return (unsigned short)Messages::Ready_For_Start_ToS;
}

string ReadyForStartToS::Serialize()
{
	Document document;
	document.SetObject();

	Value PlayerIndex(m_nPlayerIndex);
	document.AddMember("PlayerIndex", PlayerIndex, document.GetAllocator());

	GenericStringBuffer<UTF8<>> buffer;
	Writer<StringBuffer, UTF8<>> writer(buffer);
	document.Accept(writer);

	return buffer.GetString();
}

bool ReadyForStartToS::Deserialize(string strJson)
{
	Document document;
	document.Parse<0>(strJson.c_str());
	if (!document.IsObject())
	{
		return false;
	}

	if (document.HasMember("PlayerIndex") && document["PlayerIndex"].IsInt())
	{
		m_nPlayerIndex = document["PlayerIndex"].GetInt();
	}
	else
	{
		return false;
	}

	return true;
}