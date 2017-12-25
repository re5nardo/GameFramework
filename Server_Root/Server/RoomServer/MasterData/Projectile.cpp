#include "stdafx.h"
#include "Projectile.h"
#include "../Util.h"

namespace MasterData
{
	Projectile::Projectile()
	{
	}

	Projectile::~Projectile()
	{
	}

	void Projectile::SetData(vector<string> data)
	{
		m_nID = atoi(data[0].c_str());
		m_strName = data[1];
		m_strClassName = data[2];
		Util::Parse(data[3], ',', &m_vecBehaviorID);
		m_fSize = atof(data[4].c_str());
		m_strLifeInfo = data[6];

		m_fHeight = atof(data[7].c_str());
		m_fDefault_Y = atof(data[8].c_str());
		m_nDefaultBehaviorID = atoi(data[9].c_str());
	}
}