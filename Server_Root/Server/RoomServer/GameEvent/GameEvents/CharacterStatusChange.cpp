#include "stdafx.h"
#include "CharacterStatusChange.h"
#include "../../../FBSFiles/CharacterStatusChange_generated.h"

namespace GameEvent
{
	CharacterStatusChange::CharacterStatusChange()
	{
	}

	CharacterStatusChange::~CharacterStatusChange()
	{
	}

	FBS::GameEventType CharacterStatusChange::GetType()
	{
		return FBS::GameEventType::GameEventType_CharacterStatusChange;
	}

	const char* CharacterStatusChange::Serialize(int* pLength)
	{
		auto statusField = m_Builder.CreateString(m_strStatusField);
		auto reason = m_Builder.CreateString(m_strReason);

		FBS::GameEvent::CharacterStatusChangeBuilder data_builder(m_Builder);
		data_builder.add_EntityID(m_nEntityID);
		data_builder.add_StatusField(statusField);
		data_builder.add_Reason(reason);
		data_builder.add_Value(m_fValue);
		auto data = data_builder.Finish();

		m_Builder.Finish(data);

		*pLength = m_Builder.GetSize();

		return (char*)m_Builder.GetBufferPointer();
	}
}