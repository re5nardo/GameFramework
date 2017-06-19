#pragma once

#include "IMasterData.h"
#include <string>

using namespace std;

namespace MasterData
{
	class Behavior : public IMasterData
	{
	public:
		Behavior();
		virtual ~Behavior();

	public:
		string m_strName;
		string m_strClassName;
		string m_strStringParams;

	public:
		void SetData(Sheet* pSheet, int nRow) override;
	};
}