#pragma once

#include "IMasterData.h"
#include <string>
#include <vector>

using namespace std;

namespace MasterData
{
	class Behavior : public IMasterData
	{
	public:
		Behavior();
		virtual ~Behavior();

	public:
		struct Action
		{
			string m_strID;
			float m_fTime;
			vector<string> m_vecParams;
		};

	public:
		string	m_strName;
		string	m_strClassName;
		float	m_fLength;
		string	m_strStringParams;
		vector<Action> m_vecAction;

	public:
		void SetData(Sheet* pSheet, int nRow) override;
	};
}