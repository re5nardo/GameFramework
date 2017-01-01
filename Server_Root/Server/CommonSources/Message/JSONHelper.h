#pragma once

#include <list>
#include <vector>
#include <string>
#include <map>
#include "../../../rapidjson/document.h"

class IJSONObjectConvertible;

using namespace rapidjson;
using namespace std;

class JSONHelper
{
public:
	JSONHelper();
	virtual ~JSONHelper();

public:
	static void AddField(Value* pJsonObj, Document* pDocument, const char* pCharFieldName, __int32 value);
	static void AddField(Value* pJsonObj, Document* pDocument, const char* pCharFieldName, __int64 value);
	static void AddField(Value* pJsonObj, Document* pDocument, const char* pCharFieldName, unsigned __int64 value);
	static void AddField(Value* pJsonObj, Document* pDocument, const char* pCharFieldName, float value);
	static void AddField(Value* pJsonObj, Document* pDocument, const char* pCharFieldName, string value);
	static void AddField(Value* pJsonObj, Document* pDocument, const char* pCharFieldName, Value value);
	static void AddField(Value* pJsonObj, Document* pDocument, const char* pCharFieldName, vector<string> value);
	static void AddField(Value* pJsonObj, Document* pDocument, const char* pCharFieldName, map<int, string> value);
	static void AddField(Value* pJsonObj, Document* pDocument, const char* pCharFieldName, IJSONObjectConvertible* pValue);

	template<typename T>
	static void AddField(Value* pJsonObj, Document* pDocument, const char* pCharFieldName, list<T> value)
	{
		Value field(kArrayType);

		for (list<T>::iterator it = value.begin(); it != value.end(); ++it)
		{
			field.PushBack<T>(*it, pJsonObj->GetAllocator());
		}

		pJsonObj->AddMember(pCharFieldName, field, pDocument->GetAllocator());
	}

	template<typename T>
	static void AddField(Value* pJsonObj, Document* pDocument, const char* pCharFieldName, vector<T> value)
	{
		Value field(kArrayType);

		for (vector<T>::iterator it = value.begin(); it != value.end(); ++it)
		{
			field.PushBack<T>(*it, pJsonObj->GetAllocator());
		}

		pJsonObj->AddMember(pCharFieldName, field, pDocument->GetAllocator());
	}

	static bool GetField(Value* pJsonObj, string strFieldName, __int32* pValue);
	static bool GetField(Value* pJsonObj, string strFieldName, __int64* pValue);
	static bool GetField(Value* pJsonObj, string strFieldName, unsigned __int64* pValue);
	static bool GetField(Value* pJsonObj, string strFieldName, float* pValue);
	static bool GetField(Value* pJsonObj, string strFieldName, string* pValue);
	static bool GetField(Value* pJsonObj, string strFieldName, list<__int32>* pValue);
	static bool GetField(Value* pJsonObj, string strFieldName, vector<string>* pValue);
	static bool GetField(Value* pJsonObj, string strFieldName, map<int, string>* pValue);
	static bool GetField(Value* pJsonObj, string strFieldName, IJSONObjectConvertible* pValue);
};

