#include "stdafx.h"
#include "Behavior.h"

namespace MasterData
{
	Behavior::Behavior()
	{
	}

	Behavior::~Behavior()
	{
	}

	void Behavior::SetData(Sheet* pSheet, int nRow)
	{
		m_nID = pSheet->readNum(nRow, 0);
		m_strName = pSheet->readStr(nRow, 1);
		m_strClassName = pSheet->readStr(nRow, 2);
		m_strStringParams = pSheet->cellType(nRow, 3) != CellType::CELLTYPE_EMPTY ? pSheet->readStr(nRow, 3) : "";
	}
}

