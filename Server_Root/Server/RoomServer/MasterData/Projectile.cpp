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

	void Projectile::SetData(Sheet* pSheet, int nRow)
	{
		m_nID = pSheet->readNum(nRow, 0);
		m_strName = pSheet->readStr(nRow, 1);
		m_strClassName = pSheet->readStr(nRow, 2);
		if (pSheet->cellType(nRow, 3) == CellType::CELLTYPE_NUMBER)
		{
			m_vecBehaviorID.push_back(pSheet->readNum(nRow, 3));
		}
		else if (pSheet->cellType(nRow, 3) == CellType::CELLTYPE_STRING)
		{
			Util::Parse(pSheet->readStr(nRow, 3), ',', &m_vecBehaviorID);
		}
		m_fSize = pSheet->readNum(nRow, 4);
	}
}