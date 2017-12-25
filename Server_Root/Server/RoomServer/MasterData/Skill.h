#pragma once

#include "IMasterData.h"
#include <string>
#include <utility>
#include <vector>

using namespace std;

namespace MasterData
{
	class Skill : public IMasterData
	{
	public:
		Skill();
		virtual ~Skill();

	public:
		string m_strName;
		string m_strClassName;
		vector<pair<int, float>> m_vecBehavior;
		vector<pair<int, float>> m_vecState;
		string m_strStringParams;
		float m_fLength;
		float m_fCoolTime;
		float m_fMP;

	public:
		void SetData(vector<string> data) override;
	};
}