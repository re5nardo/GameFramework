#include "stdafx.h"
#include "ICharacter.h"

ICharacter::ICharacter()
{
}

ICharacter::~ICharacter()
{
}

float ICharacter::GetSpeed()
{
	return m_CurrentStat.fSpeed;
}

void ICharacter::SetStat(Stat stat)
{
	m_DefaultStat = stat;
	m_CurrentStat = stat;
}