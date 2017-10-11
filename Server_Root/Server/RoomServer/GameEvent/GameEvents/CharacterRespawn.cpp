#include "stdafx.h"
#include "CharacterRespawn.h"
#include "../../../FBSFiles/CharacterRespawn_generated.h"

namespace GameEvent
{
	CharacterRespawn::CharacterRespawn()
	{
	}

	CharacterRespawn::~CharacterRespawn()
	{
	}

	FBS::GameEventType CharacterRespawn::GetType()
	{
		return FBS::GameEventType::GameEventType_CharacterRespawn;
	}

	const char* CharacterRespawn::Serialize(int* pLength)
	{
		FBS::Data::Vector3 position(m_vec3Position.x(), m_vec3Position.y(), m_vec3Position.z());

		FBS::GameEvent::CharacterRespawnBuilder data_builder(m_Builder);
		data_builder.add_EntityID(m_nEntityID);
		data_builder.add_Pos(&position);
		auto data = data_builder.Finish();

		m_Builder.Finish(data);

		*pLength = m_Builder.GetSize();

		return (char*)m_Builder.GetBufferPointer();
	}
}