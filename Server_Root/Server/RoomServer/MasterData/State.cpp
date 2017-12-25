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

	void State::SetData(vector<string> data)
	{
		m_nID = atoi(data[0].c_str());
		m_strName = data[1];
		m_strClassName = data[2];
		m_fLength = atof(data[3].c_str());
		string stringparams = data[4];
		string corestates = data[5];
		string doubleparams1 = data[6];
		string doubleparams2 = data[7];
		Util::Parse(stringparams, ',', &m_vecStringParam);
		Util::Parse(corestates, ',', &m_vecCoreState);
		Util::Parse(doubleparams1, ',', &m_vecDoubleParam1);
		Util::Parse(doubleparams2, ',', &m_vecDoubleParam2);
	}
}