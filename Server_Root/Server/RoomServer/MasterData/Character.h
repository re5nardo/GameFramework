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
		float m_fHP;
		float m_fMP;
		float m_fMoveSpeed;
		float m_fSize;
		float m_fHeight;
		float m_fDefault_Y;
		int m_nDefaultBehaviorID = -1;

	public:
		void SetData(Sheet* pSheet, int nRow) override;
	};
}