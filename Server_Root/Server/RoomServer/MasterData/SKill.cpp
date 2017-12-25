#include "stdafx.h"
#include "Skill.h"
#include "../Util.h"

namespace MasterData
{
	Skill::Skill()
	{
	}

	Skill::~Skill()
	{
	}

	void Skill::SetData(vector<string> data)
	{
		m_nID = atoi(data[0].c_str());
		m_strName = data[1];
		m_strClassName = data[2];

		string behaviors = data[3];
		string states = data[4];

		vector<string> vecString;
		vector<string> vecString2;
		Util::Parse(behaviors, ',', &vecString);
		for (vector<string>::iterator it = vecString.begin(); it != vecString.end(); ++it)
		{
			Util::Parse(*it, ':', &vecString2);
			m_vecBehavior.push_back(make_pair(atoi(vecString2[0].c_str()), atof(vecString2[1].c_str())));
		}

		Util::Parse(states, ',', &vecString);
		for (vector<string>::iterator it = vecString.begin(); it != vecString.end(); ++it)
		{
			Util::Parse(*it, ':', &vecString2);
			m_vecState.push_back(make_pair(atoi(vecString2[0].c_str()), atof(vecString2[1].c_str())));
		}

		m_strStringParams = data[5];
		m_fLength = atof(data[6].c_str());
		m_fCoolTime = atof(data[7].c_str());
		m_fMP = atof(data[8].c_str());
	}
}