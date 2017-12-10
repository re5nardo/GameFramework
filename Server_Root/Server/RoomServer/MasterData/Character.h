#pragma once

#include "IMasterData.h"
#include <string>
#include <vector>

using namespace std;

namespace MasterData
{
	class Character : public IMasterData
	{
	public:
		Character();
		virtual ~Character();

	public:
		string m_strName;
		string m_strClassName;
		vector<int> m_vecSkillID;
		vector<int> m_vecBehaviorID;
		int m_nHP;
		int m_nMP;
		float m_fMaximumSpeed;
		float m_fMPChargeRate;
		float m_fSize;
		float m_fHeight;
		float m_fDefault_Y;
		int m_nDefaultBehaviorID = -1;

	public:
		void SetData(Sheet* pSheet, int nRow) override;
	};
}