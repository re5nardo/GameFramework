#pragma once

#include "libxl.h"

using namespace libxl;

class IMasterData
{
public:
	IMasterData();
	virtual ~IMasterData();

public:
	int m_nID;

public:
	virtual void SetData(Sheet* pSheet, int nRow) = 0;
};