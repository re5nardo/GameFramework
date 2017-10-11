#include "stdafx.h"
#include "State.h"
#include "../Util.h"

namespace MasterData
{
	State::State()
	{
	}

	State::~State()
	{
	}

	void State::SetData(Sheet* pSheet, int nRow)
	{
		string stringparams = pSheet->cellType(nRow, 4) != CellType::CELLTYPE_EMPTY ? pSheet->readStr(nRow, 4) : "";
		string corestates = pSheet->cellType(nRow, 5) != CellType::CELLTYPE_EMPTY ? pSheet->readStr(nRow, 5) : "";
		string doubleparams1 = pSheet->cellType(nRow, 6) != CellType::CELLTYPE_EMPTY ? pSheet->readStr(nRow, 6) : "";
		string doubleparams2 = pSheet->cellType(nRow, 7) != CellType::CELLTYPE_EMPTY ? pSheet->readStr(nRow, 7) : "";

		m_nID = pSheet->readNum(nRow, 0);
		m_strName = pSheet->readStr(nRow, 1);
		m_strClassName = pSheet->readStr(nRow, 2);
		m_fLength = pSheet->readNum(nRow, 3);
		Util::Parse(stringparams, ',', &m_vecStringParam);
		Util::Parse(corestates, ',', &m_vecCoreState);
		Util::Parse(doubleparams1, ',', &m_vecDoubleParam1);
		Util::Parse(doubleparams2, ',', &m_vecDoubleParam2);
	}
}