#include "stdafx.h"
#include "Item.h"
#include "../Util.h"

namespace MasterData
{
	Item::Item()
	{
	}

	Item::~Item()
	{
	}

	void Item::SetData(vector<string> data)
	{
		m_nID = atoi(data[0].c_str());
		m_strName = data[1];
		m_strClassName = data[2];
		Util::Parse(data[3], ',', &m_vecBehaviorID);
		m_fSize = atof(data[4].c_str());
		m_fLifespan = atof(data[6].c_str());
		m_fHeight = atof(data[7].c_str());
		m_fDefault_Y = atof(data[8].c_str());
		m_nDefaultBehaviorID = atoi(data[9].c_str());
		m_strEffectType = data[10];
		m_nEffectParam = atoi(data[11].c_str());
	}
}