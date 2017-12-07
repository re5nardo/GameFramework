#pragma once

#include "IMasterData.h"
#include <string>
#include <vector>

using namespace std;

namespace MasterData
{
	class Item : public IMasterData
	{
	public:
		Item();
		virtual ~Item();

	public:
		string m_strName;
		string m_strClassName;
		vector<int> m_vecBehaviorID;
		float m_fSize;
		float m_fLifespan;
		float m_fHeight;
		float m_fDefault_Y;
		int m_nDefaultBehaviorID = -1;
		string m_strEffectType;
		int m_nEffectParam;

	public:
		void SetData(Sheet* pSheet, int nRow) override;
	};
}