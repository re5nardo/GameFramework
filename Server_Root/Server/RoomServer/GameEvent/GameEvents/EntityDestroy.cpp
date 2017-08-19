#include "stdafx.h"
#include "EntityDestroy.h"
#include "../../../FBSFiles/EntityDestroy_generated.h"

namespace GameEvent
{
	EntityDestroy::EntityDestroy()
	{
	}

	EntityDestroy::~EntityDestroy()
	{
	}

	FBS::GameEventType EntityDestroy::GetType()
	{
		return FBS::GameEventType::GameEventType_EntityDestroy;
	}

	const char* EntityDestroy::Serialize(int* pLength)
	{
		FBS::GameEvent::EntityDestroyBuilder data_builder(m_Builder);
		data_builder.add_EntityID(m_nEntityID);
		auto data = data_builder.Finish();

		m_Builder.Finish(data);

		*pLength = m_Builder.GetSize();

		return (char*)m_Builder.GetBufferPointer();
	}
}