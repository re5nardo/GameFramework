#include "stdafx.h"
#include "ReqMove.h"
#include "NetworkDefines.h"
#include "../../rapidjson/document.h"
#include "../../rapidjson/stringbuffer.h"
#include "../../rapidjson/writer.h"

using namespace rapidjson;

ReqMove::ReqMove()
{
}


ReqMove::~ReqMove()
{
}

unsigned short ReqMove::GetID()
{
	return (unsigned short)Messages::REQ_MOVE_ID;
}

string ReqMove::Serialize()
{
	Document document;
	document.SetObject();

	Value pos_x(m_vec3Position.x);
	document.AddMember("pos_x", pos_x, document.GetAllocator());

	Value pos_y(m_vec3Position.y);
	document.AddMember("pos_y", pos_y, document.GetAllocator());

	Value pos_z(m_vec3Position.z);
	document.AddMember("pos_z", pos_z, document.GetAllocator());

	GenericStringBuffer<UTF8<>> buffer;
	Writer<StringBuffer, UTF8<>> writer(buffer);
	document.Accept(writer);

	return buffer.GetString();
}

bool ReqMove::Deserialize(string strJson)
{
	Document document;
	document.Parse<0>(strJson.c_str());
	if (!document.IsObject())
	{
		return false;
	}

	if (document.HasMember("pos_x") && document["pos_x"].IsDouble())
	{
		m_vec3Position.x = (float)document["pos_x"].GetDouble();
	}
	else
	{
		return false;
	}

	if (document.HasMember("pos_y") && document["pos_y"].IsDouble())
	{
		m_vec3Position.y = (float)document["pos_y"].GetDouble();
	}
	else
	{
		return false;
	}

	if (document.HasMember("pos_z") && document["pos_z"].IsDouble())
	{
		m_vec3Position.z = (float)document["pos_z"].GetDouble();
	}
	else
	{
		return false;
	}
	
	return true;
}