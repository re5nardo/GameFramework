#include "stdafx.h"
#include "Character.h"
#include "../Util.h"

namespace MasterData
{
	Character::Character()
	{
	}

	Character::~Character()
	{
	}

	void Character::SetData(Sheet* pSheet, int nRow)
	{
		m_nID = pSheet->readNum(nRow, 0);
		m_strName = pSheet->readStr(nRow, 1);
		m_strClassName = pSheet->readStr(nRow, 2);
		Util::Parse(pSheet->readStr(nRow, 3), ',', &m_vecSkillID);
		Util::Parse(pSheet->readStr(nRow, 4), ',', &m_vecBehaviorID);
		m_fHP = pSheet->readNum(nRow, 5);
		m_fMP = pSheet->readNum(nRow, 6);
		m_fMoveSpeed = pSheet->readNum(nRow, 7);
		m_fSize = pSheet->readNum(nRow, 8);
	}
}