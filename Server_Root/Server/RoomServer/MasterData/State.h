#pragma once

#include "IMasterData.h"
#include <string>
#include <vector>

using namespace std;

namespace MasterData
{
	class State : public IMasterData
	{
	public:
		State();
		virtual ~State();

	public:
		string m_strName;
		string m_strClassName;
		float m_fLength;
		vector<string> m_vecStringParam;
		vector<string> m_vecCoreState;
		vector<double> m_vecDoubleParam1;
		vector<double> m_vecDoubleParam2;

	public:
		void SetData(vector<string> data) override;
	};
}