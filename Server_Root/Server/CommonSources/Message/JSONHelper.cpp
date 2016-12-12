#include "stdafx.h"
#include "JSONHelper.h"

JSONHelper::JSONHelper()
{
}


JSONHelper::~JSONHelper()
{
}

#pragma region AddField
void JSONHelper::AddField(Document* pJsonObj, const char* pCharFieldName, __int32 value)
{
	pJsonObj->AddMember<__int32>(pCharFieldName, value, pJsonObj->GetAllocator());
}

void JSONHelper::AddField(Document* pJsonObj, const char* pCharFieldName, __int64 value)
{
	pJsonObj->AddMember<__int64>(pCharFieldName, value, pJsonObj->GetAllocator());
}

void JSONHelper::AddField(Document* pJsonObj, const char* pCharFieldName, unsigned __int64 value)
{
	pJsonObj->AddMember<unsigned __int64>(pCharFieldName, value, pJsonObj->GetAllocator());
}

void JSONHelper::AddField(Document* pJsonObj, const char* pCharFieldName, float value)
{
	pJsonObj->AddMember<float>(pCharFieldName, value, pJsonObj->GetAllocator());
}

void JSONHelper::AddField(Document* pJsonObj, const char* pCharFieldName, string value)
{
	Value field;
	field.SetString(value.c_str(), value.length(), pJsonObj->GetAllocator());

	pJsonObj->AddMember(pCharFieldName, field, pJsonObj->GetAllocator());
}

void JSONHelper::AddField(Document* pJsonObj, const char* pCharFieldName, Value value)
{
	pJsonObj->AddMember(pCharFieldName, value, pJsonObj->GetAllocator());
}

void JSONHelper::AddField(Document* pJsonObj, const char* pCharFieldName, vector<string> value)
{
	Value field(kArrayType);

	for (vector<string>::iterator it = value.begin(); it != value.end(); ++it)
	{
		Value element;
		element.SetString((*it).c_str(), (*it).length(), pJsonObj->GetAllocator());

		field.PushBack(element, pJsonObj->GetAllocator());
	}

	pJsonObj->AddMember(pCharFieldName, field, pJsonObj->GetAllocator());
}

void JSONHelper::AddField(Document* pJsonObj, const char* pCharFieldName, map<int, string> value)
{
	Value field(kObjectType);
	field.SetObject();
	for (map<int, string>::iterator itr = value.begin(); itr != value.end(); itr++)
	{
		//	key
		Value k;
		k.SetString(to_string(itr->first).c_str(), to_string(itr->first).length(), pJsonObj->GetAllocator());

		//	value
		Value v;
		v.SetString(itr->second.c_str(), itr->second.length(), pJsonObj->GetAllocator());

		field.AddMember(k, v, pJsonObj->GetAllocator());
	}
	pJsonObj->AddMember(pCharFieldName, field, pJsonObj->GetAllocator());
}
#pragma endregion


#pragma region GetField
bool JSONHelper::GetField(Document* pJsonObj, string strFieldName, __int32* pValue)
{
	if (!pJsonObj->HasMember(strFieldName.c_str()))
	{
		//Debug.LogWarning("JSONObject does not have field, field name : " + strFieldName);
		return false;
	}

	if (!(*pJsonObj)[strFieldName.c_str()].IsInt())
	{
		//Debug.LogWarning("Data type is invalid! It's not Int");
		return false;
	}

	*pValue = (*pJsonObj)[strFieldName.c_str()].GetInt();

	return true;
}

bool JSONHelper::GetField(Document* pJsonObj, string strFieldName, __int64* pValue)
{
	if (!pJsonObj->HasMember(strFieldName.c_str()))
	{
		//Debug.LogWarning("JSONObject does not have field, field name : " + strFieldName);
		return false;
	}

	if (!(*pJsonObj)[strFieldName.c_str()].IsInt64())
	{
		//Debug.LogWarning("Data type is invalid! It's not Int64");
		return false;
	}

	*pValue = (*pJsonObj)[strFieldName.c_str()].GetInt64();

	return true;
}

bool JSONHelper::GetField(Document* pJsonObj, string strFieldName, unsigned __int64* pValue)
{
	if (!pJsonObj->HasMember(strFieldName.c_str()))
	{
		//Debug.LogWarning("JSONObject does not have field, field name : " + strFieldName);
		return false;
	}

	if (!(*pJsonObj)[strFieldName.c_str()].IsUint64())
	{
		//Debug.LogWarning("Data type is invalid! It's not Uint64");
		return false;
	}

	*pValue = (*pJsonObj)[strFieldName.c_str()].GetUint64();

	return true;
}

bool JSONHelper::GetField(Document* pJsonObj, string strFieldName, float* pValue)
{
	if (!pJsonObj->HasMember(strFieldName.c_str()))
	{
		//Debug.LogWarning("JSONObject does not have field, field name : " + strFieldName);
		return false;
	}

	if ((*pJsonObj)[strFieldName.c_str()].IsDouble())
	{
		*pValue = (*pJsonObj)[strFieldName.c_str()].GetDouble();
	}
	else if ((*pJsonObj)[strFieldName.c_str()].IsInt())
	{
		*pValue = (*pJsonObj)[strFieldName.c_str()].GetInt();
	}
	else
	{
		//Debug.LogWarning("Data type is invalid! It's not Float");
		return false;
	}

	return true;
}

bool JSONHelper::GetField(Document* pJsonObj, string strFieldName, string* pValue)
{
	if (!pJsonObj->HasMember(strFieldName.c_str()))
	{
		//Debug.LogWarning("JSONObject does not have field, field name : " + strFieldName);
		return false;
	}

	if (!(*pJsonObj)[strFieldName.c_str()].IsString())
	{
		//Debug.LogWarning("Data type is invalid! It's not String");
		return false;
	}

	*pValue = (*pJsonObj)[strFieldName.c_str()].GetString();

	return true;
}

bool JSONHelper::GetField(Document* pJsonObj, string strFieldName, list<__int32>* pValue)
{
	if (!pJsonObj->HasMember(strFieldName.c_str()))
	{
		//Debug.LogWarning("JSONObject does not have field, field name : " + strFieldName);
		return false;
	}

	if (!(*pJsonObj)[strFieldName.c_str()].IsArray())
	{
		//Debug.LogWarning("Data type is invalid! It's not Array");
		return false;
	}

	const Value& values = (*pJsonObj)[strFieldName.c_str()];
	for (SizeType i = 0; i < values.Size(); i++)
	{
		(*pValue).push_back(values[i].GetInt());
	}

	return true;
}

bool JSONHelper::GetField(Document* pJsonObj, string strFieldName, vector<string>* pValue)
{
	if (!pJsonObj->HasMember(strFieldName.c_str()))
	{
		//Debug.LogWarning("JSONObject does not have field, field name : " + strFieldName);
		return false;
	}

	if (!(*pJsonObj)[strFieldName.c_str()].IsArray())
	{
		//Debug.LogWarning("Data type is invalid! It's not Array");
		return false;
	}

	const Value& values = (*pJsonObj)[strFieldName.c_str()];
	for (SizeType i = 0; i < values.Size(); i++)
	{
		(*pValue).push_back(values[i].GetString());
	}

	return true;
}

bool JSONHelper::GetField(Document* pJsonObj, string strFieldName, map<int, string>* pValue)
{
	if (!pJsonObj->HasMember(strFieldName.c_str()))
	{
		//Debug.LogWarning("JSONObject does not have field, field name : " + strFieldName);
		return false;
	}

	if (!(*pJsonObj)[strFieldName.c_str()].IsObject())
	{
		//Debug.LogWarning("Data type is invalid! It's not Array");
		return false;
	}

	const Value& values = (*pJsonObj)[strFieldName.c_str()];
	for (Value::ConstMemberIterator itr = values.MemberBegin(); itr != values.MemberEnd(); ++itr)
	{
		(*pValue)[atoi(itr->name.GetString())] = itr->value.GetString();
	}

	return true;
}
#pragma endregion