#include "stdafx.h"
#include "JoinLobbyToC.h"
#include "NetworkDefines.h"
#include "../../rapidjson/document.h"
#include "../../rapidjson/stringbuffer.h"
#include "../../rapidjson/writer.h"

using namespace rapidjson;


JoinLobbyToC::JoinLobbyToC()
{
}


JoinLobbyToC::~JoinLobbyToC()
{
}

unsigned short JoinLobbyToC::GetID()
{
	return (unsigned short)Messages::Join_Lobby_ToC;
}

string JoinLobbyToC::Serialize()
{
	Document document;
	document.SetObject();

	Value Result(m_nResult);
	document.AddMember("Result", Result, document.GetAllocator());

	GenericStringBuffer<UTF8<>> buffer;
	Writer<StringBuffer, UTF8<>> writer(buffer);
	document.Accept(writer);

	return buffer.GetString();
}

bool JoinLobbyToC::Deserialize(string strJson)
{
	Document document;
	document.Parse<0>(strJson.c_str());
	if (!document.IsObject())
	{
		return false;
	}

	if (document.HasMember("Result") && document["Result"].IsInt())
	{
		m_nResult = document["Result"].GetInt();
	}
	else
	{
		return false;
	}

	return true;
}