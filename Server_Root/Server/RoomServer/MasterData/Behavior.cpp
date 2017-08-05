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

	void Behavior::SetData(Sheet* pSheet, int nRow)
	{
		m_nID = pSheet->readNum(nRow, 0);
		m_strName = pSheet->readStr(nRow, 1);
		m_strClassName = pSheet->readStr(nRow, 2);
		m_fLength = pSheet->readNum(nRow, 3);
		m_strStringParams = pSheet->cellType(nRow, 4) == CellType::CELLTYPE_STRING ? pSheet->readStr(nRow, 4) : "";

		if (pSheet->cellType(nRow, 6) == CellType::CELLTYPE_STRING)
		{
			vector<string> vecString;
			vector<string> vecString2;
			Util::Parse(pSheet->readStr(nRow, 6), ',', &vecString);
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
}

