#pragma once

#include "../IGameEvent.h"

namespace GameEvent
{
	class CharacterStatusChange : public IGameEvent
	{
	public:
		CharacterStatusChange();
		virtual ~CharacterStatusChange();

	public:
		int m_nEntityID;
		string m_strStatusField;
		string m_strReason;
		float m_fValue;

	public:
		FBS::GameEventType GetType() override;
		const char* Serialize(int* pLength = NULL) override;
	};
}