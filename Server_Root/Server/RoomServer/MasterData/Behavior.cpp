#include "stdafx.h"
#include "Behavior.h"
#include "../Util.h"

namespace MasterData
{
	Behavior::Behavior()
	{
	}

	Behavior::~Behavior()
	{
	}

	void Behavior::SetData(vector<string> data)
	{
		m_nID = atoi(data[0].c_str());
		m_strName = data[1];
		m_strClassName = data[2];
		m_fLength = atof(data[3].c_str());
		m_strStringParams = data[4];

		vector<string> vecString;
		vector<string> vecString2;
		Util::Parse(data[6], ',', &vecString);
		for (vector<string>::iterator it = vecString.begin(); it != vecString.end(); ++it)
		{
			Util::Parse(*it, ':', &vecString2);

			Action action;
			action.m_strID = vecString2[0];
			action.m_fTime = atof(vecString2[1].c_str());
			action.m_vecParams.resize(vecString2.size());
			copy(vecString2.begin() + 2, vecString2.end(), action.m_vecParams.begin());

			m_vecAction.push_back(action);
		}
	}
}

