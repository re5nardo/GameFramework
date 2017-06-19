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

	void Skill::SetData(Sheet* pSheet, int nRow)
	{
		string behaviors = pSheet->cellType(nRow, 3) != CellType::CELLTYPE_EMPTY ? pSheet->readStr(nRow, 3) : "";
		string states = pSheet->cellType(nRow, 4) != CellType::CELLTYPE_EMPTY ? pSheet->readStr(nRow, 4) : "";

		m_nID = pSheet->readNum(nRow, 0);
		m_strName = pSheet->readStr(nRow, 1);
		m_strClassName = pSheet->readStr(nRow, 2);

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

		m_strStringParams = pSheet->cellType(nRow, 5) != CellType::CELLTYPE_EMPTY ? pSheet->readStr(nRow, 5) : "";
		m_fLength = pSheet->cellType(nRow, 6) != CellType::CELLTYPE_EMPTY ? pSheet->readNum(nRow, 6) : 0;
		m_fCoolTime = pSheet->readNum(nRow, 7);
		m_fMP = pSheet->readNum(nRow, 8);
	}
}