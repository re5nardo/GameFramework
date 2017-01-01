#include "stdafx.h"
#include "JSONHelper.h"
#include "IJSONObjectConvertible.h"

JSONHelper::JSONHelper()
{
}


JSONHelper::~JSONHelper()
{
}

#pragma region AddField
void JSONHelper::AddField(Value* pJsonObj, Document* pDocument, const char* pCharFieldName, __int32 value)
{
	pJsonObj->AddMember<__int32>(pCharFieldName, value, pDocument->GetAllocator());
}

void JSONHelper::AddField(Value* pJsonObj, Document* pDocument, const char* pCharFieldName, __int64 value)
{
	pJsonObj->AddMember<__int64>(pCharFieldName, value, pDocument->GetAllocator());
}

void JSONHelper::AddField(Value* pJsonObj, Document* pDocument, const char* pCharFieldName, unsigned __int64 value)
{
	pJsonObj->AddMember<unsigned __int64>(pCharFieldName, value, pDocument->GetAllocator());
}

void JSONHelper::AddField(Value* pJsonObj,Document* pDocument, const char* pCharFieldName, float value)
{
	pJsonObj->AddMember<float>(pCharFieldName, value, pDocument->GetAllocator());
}

void JSONHelper::AddField(Value* pJsonObj, Document* pDocument, const char* pCharFieldName, string value)
{
	Value field;
	field.SetString(value.c_str(), value.length(), pDocument->GetAllocator());

	pJsonObj->AddMember(pCharFieldName, field, pDocument->GetAllocator());
}

void JSONHelper::AddField(Value* pJsonObj, Document* pDocument, const char* pCharFieldName, Value value)
{
	pJsonObj->AddMember(pCharFieldName, value, pDocument->GetAllocator());
}

void JSONHelper::AddField(Value* pJsonObj, Document* pDocument, const char* pCharFieldName, vector<string> value)
{
	Value field(kArrayType);

	for (vector<string>::iterator it = value.begin(); it != value.end(); ++it)
	{
		Value element;
		element.SetString((*it).c_str(), (*it).length(), pDocument->GetAllocator());

		field.PushBack(element, pDocument->GetAllocator());
	}

	pJsonObj->AddMember(pCharFieldName, field, pDocument->GetAllocator());
}

void JSONHelper::AddField(Value* pJsonObj, Document* pDocument, const char* pCharFieldName, map<int, string> value)
{
	Value field(kObjectType);
	field.SetObject();
	for (map<int, string>::iterator itr = value.begin(); itr != value.end(); itr++)
	{
		//	key
		Value k;
		k.SetString(to_string(itr->first).c_str(), to_string(itr->first).length(), pDocument->GetAllocator());

		//	value
		Value v;
		v.SetString(itr->second.c_str(), itr->second.length(), pDocument->GetAllocator());

		field.AddMember(k, v, pDocument->GetAllocator());
	}
	pJsonObj->AddMember(pCharFieldName, field, pDocument->GetAllocator());
}

void JSONHelper::AddField(Value* pJsonObj, Document* pDocument, const char* pCharFieldName, IJSONObjectConvertible* pValue)
{
	pJsonObj->AddMember(pCharFieldName, pValue->GetJSONObject(pDocument), pDocument->GetAllocator());
}
#pragma endregion


#pragma region GetField
bool JSONHelper::GetField(Value* pJsonObj, string strFieldName, __int32* pValue)
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

bool JSONHelper::GetField(Value* pJsonObj, string strFieldName, __int64* pValue)
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

bool JSONHelper::GetField(Value* pJsonObj, string strFieldName, unsigned __int64* pValue)
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

bool JSONHelper::GetField(Value* pJsonObj, string strFieldName, float* pValue)
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

bool JSONHelper::GetField(Value* pJsonObj, string strFieldName, string* pValue)
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

bool JSONHelper::GetField(Value* pJsonObj, string strFieldName, list<__int32>* pValue)
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

bool JSONHelper::GetField(Value* pJsonObj, string strFieldName, vector<string>* pValue)
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

bool JSONHelper::GetField(Value* pJsonObj, string strFieldName, map<int, string>* pValue)
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

bool JSONHelper::GetField(Value* pJsonObj, string strFieldName, IJSONObjectConvertible* pValue)
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

	pValue->SetJSONObject(&((*pJsonObj)[strFieldName.c_str()]));

	return true;
}
#pragma endregion