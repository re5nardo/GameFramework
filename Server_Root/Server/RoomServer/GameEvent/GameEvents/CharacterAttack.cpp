#include "stdafx.h"
#include "CharacterAttack.h"
#include "../../../FBSFiles/CharacterAttack_generated.h"

namespace GameEvent
{
	CharacterAttack::CharacterAttack()
	{
	}

	CharacterAttack::~CharacterAttack()
	{
	}

	FBS::GameEventType CharacterAttack::GetType()
	{
		return FBS::GameEventType::GameEventType_CharacterAttack;
	}

	const char* CharacterAttack::Serialize(int* pLength)
	{
		FBS::GameEvent::CharacterAttackBuilder data_builder(m_Builder);
		data_builder.add_AttackingEntityID(m_nAttackingEntityID);
		data_builder.add_AttackedEntityID(m_nAttackedEntityID);
		data_builder.add_Damage(m_nDamage);
		auto data = data_builder.Finish();

		m_Builder.Finish(data);

		*pLength = m_Builder.GetSize();

		return (char*)m_Builder.GetBufferPointer();
	}
}