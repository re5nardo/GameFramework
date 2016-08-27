#pragma once

#include <list>
#include "../../../rapidjson/document.h"

using namespace rapidjson;
using namespace std;

class JSONHelper
{
public:
	JSONHelper();
	virtual ~JSONHelper();

public:

	static void AddField(Document* pJsonObj, const char* pCharFieldName, __int32 value);
	static void AddField(Document* pJsonObj, const char* pCharFieldName, __int64 value);
	static void AddField(Document* pJsonObj, const char* pCharFieldName, unsigned __int64 value);
	static void AddField(Document* pJsonObj, const char* pCharFieldName, float value);
	static void AddField(Document* pJsonObj, const char* pCharFieldName, string value);
	static void AddField(Document* pJsonObj, const char* pCharFieldName, Value value);
	template<typename T>
	static void AddField(Document* pJsonObj, const char* pCharFieldName, list<T> value);

	static bool GetField(Document* pJsonObj, string strFieldName, __int32* pValue);
	static bool GetField(Document* pJsonObj, string strFieldName, __int64* pValue);
	static bool GetField(Document* pJsonObj, string strFieldName, unsigned __int64* pValue);
	static bool GetField(Document* pJsonObj, string strFieldName, float* pValue);
	static bool GetField(Document* pJsonObj, string strFieldName, string* pValue);
	static bool GetField(Document* pJsonObj, string strFieldName, list<__int32>* pValue);
};

