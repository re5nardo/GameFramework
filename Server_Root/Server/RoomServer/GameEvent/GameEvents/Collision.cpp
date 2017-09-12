#include "stdafx.h"
#include "Collision.h"
#include "../../../FBSFiles/Collision_generated.h"

namespace GameEvent
{
	Collision::Collision()
	{
	}

	Collision::~Collision()
	{
	}

	FBS::GameEventType Collision::GetType()
	{
		return FBS::GameEventType::GameEventType_Collision;
	}

	const char* Collision::Serialize(int* pLength)
	{
		FBS::Data::Vector3 position(m_vec3Position.x(), m_vec3Position.y(), m_vec3Position.z());

		FBS::GameEvent::CollisionBuilder data_builder(m_Builder);
		data_builder.add_EntityID(m_nEntityID);
		data_builder.add_Pos(&position);

		auto data = data_builder.Finish();

		m_Builder.Finish(data);

		*pLength = m_Builder.GetSize();

		return (char*)m_Builder.GetBufferPointer();
	}
}