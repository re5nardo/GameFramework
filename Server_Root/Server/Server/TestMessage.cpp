#include "stdafx.h"
#include "TestMessage.h"
#include "NetworkDefines.h"
#include "../../rapidjson/document.h"
#include "../../rapidjson/stringbuffer.h"
#include "../../rapidjson/writer.h"

using namespace rapidjson;

TestMessage::TestMessage()
{
}


TestMessage::~TestMessage()
{
}


unsigned short TestMessage::GetID()
{
	return (unsigned short)Messages::TEST_MESSAGE_ID;
}

const char* TestMessage::Serialize()
{
	Document document;
	document.SetObject();

	Value name(m_strName.c_str());
	document.AddMember("name", name, document.GetAllocator());

	Value age(m_nAge);
	document.AddMember("age", age, document.GetAllocator());

	Value favoriteNumbers(kArrayType);
	for (vector<int>::iterator itr = m_vecFavoriteNumbers.begin(); itr != m_vecFavoriteNumbers.end(); itr++)
	{
		favoriteNumbers.PushBack(Value().SetInt(*itr), document.GetAllocator());
	}
	document.AddMember("favoriteNumbers", favoriteNumbers, document.GetAllocator());

	Value options(kObjectType);
	options.SetObject();
	for (map<string, string>::iterator itr = m_mapOptions.begin(); itr != m_mapOptions.end(); itr++)
	{
		Value value = itr->second.c_str();
		options.AddMember(itr->first.c_str(), value, document.GetAllocator());
	}
	document.AddMember("options", options, document.GetAllocator());

	GenericStringBuffer<UTF8<>> buffer;
	Writer<StringBuffer, UTF8<>> writer(buffer);
	document.Accept(writer);

	return buffer.GetString();
}

bool TestMessage::Deserialize(const char* pChar)
{
	Document document;
	document.Parse<0>(pChar);
	if (!document.IsObject())
	{
		return false;
	}
	
	if (document.HasMember("name") && document["name"].IsString())
	{
		m_strName = document["name"].GetString();
	}
	else
	{
		return false;
	}

	if (document.HasMember("age") && document["age"].IsNumber())
	{
		m_nAge = document["age"].GetInt();
	}
	else
	{
		return false;
	}

	if (document.HasMember("favoriteNumbers") && document["favoriteNumbers"].IsArray())
	{
		const Value& values = document["favoriteNumbers"];
		for (SizeType i = 0; i < values.Size(); i++)
		{
			m_vecFavoriteNumbers.push_back(values[i].GetInt());
		}
	}
	else
	{
		return false;
	}

	if (document.HasMember("options") && document["options"].IsObject())
	{
		const Value& values = document["options"];
		for (Value::ConstMemberIterator itr = values.MemberBegin(); itr != values.MemberEnd(); ++itr)
		{
			m_mapOptions[itr->name.GetString()] = itr->value.GetString();
		}
	}
	else
	{
		return false;
	}

	return true;
}

void TestMessage::SetName(string strName)
{
	m_strName = strName;
}

void TestMessage::SetAge(int nAge)
{
	m_nAge = nAge;
}