#pragma once

#include "IMasterData.h"
#include <string>
#include <vector>

using namespace std;

namespace MasterData
{
	class Projectile : public IMasterData
	{
	public:
		Projectile();
		virtual ~Projectile();

	public:
		string m_strName;
		string m_strClassName;
		vector<int> m_vecBehaviorID;
		float m_fSize;

	public:
		void SetData(Sheet* pSheet, int nRow) override;
	};
}