#include "stdafx.h"
#include "JoinLobbyToS.h"
#include "NetworkDefines.h"
#include "../../rapidjson/document.h"
#include "../../rapidjson/stringbuffer.h"
#include "../../rapidjson/writer.h"

using namespace rapidjson;


JoinLobbyToS::JoinLobbyToS()
{
}


JoinLobbyToS::~JoinLobbyToS()
{
}

unsigned short JoinLobbyToS::GetID()
{
	return (unsigned short)Messages::Join_Lobby_ToS;
}

string JoinLobbyToS::Serialize()
{
	Document document;
	document.SetObject();

	Value PlayerNumber(m_nPlayerNumber);
	document.AddMember("PlayerNumber", PlayerNumber, document.GetAllocator());

	Value AuthKey(m_nAuthKey);
	document.AddMember("AuthKey", AuthKey, document.GetAllocator());

	GenericStringBuffer<UTF8<>> buffer;
	Writer<StringBuffer, UTF8<>> writer(buffer);
	document.Accept(writer);

	return buffer.GetString();
}

bool JoinLobbyToS::Deserialize(string strJson)
{
	Document document;
	document.Parse<0>(strJson.c_str());
	if (!document.IsObject())
	{
		return false;
	}

	if (document.HasMember("PlayerNumber") && document["PlayerNumber"].IsUint64())
	{
		m_nPlayerNumber = document["PlayerIndex"].GetUint64();
	}
	else
	{
		return false;
	}

	if (document.HasMember("AuthKey") && document["AuthKey"].IsInt())
	{
		m_nAuthKey = document["AuthKey"].GetInt();
	}
	else
	{
		return false;
	}

	return true;
}