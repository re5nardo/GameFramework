#pragma once

#include "../IGameEvent.h"

namespace GameEvent
{
	class CharacterAttack : public IGameEvent
	{
	public:
		CharacterAttack();
		virtual ~CharacterAttack();

	public:
		int m_nAttackingEntityID;
		int m_nAttackedEntityID;
		int m_nDamage;

	public:
		FBS::GameEventType GetType() override;
		const char* Serialize(int* pLength = NULL) override;
	};
}