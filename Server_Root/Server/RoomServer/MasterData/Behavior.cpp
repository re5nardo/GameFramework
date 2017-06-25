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
		m_fLength = pSheet->readNum(nRow, 3);
		m_strStringParams = pSheet->cellType(nRow, 4) == CellType::CELLTYPE_STRING ? pSheet->readStr(nRow, 4) : "";
	}
}

