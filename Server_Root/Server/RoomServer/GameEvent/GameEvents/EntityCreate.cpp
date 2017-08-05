#include "stdafx.h"
#include "EntityCreate.h"
#include "../../../FBSFiles/EntityCreate_generated.h"

namespace GameEvent
{
	EntityCreate::EntityCreate()
	{
	}

	EntityCreate::~EntityCreate()
	{
	}

	FBS::GameEventType EntityCreate::GetType()
	{
		return FBS::GameEventType::GameEventType_EntityCreate;
	}

	const char* EntityCreate::Serialize(int* pLength)
	{
		FBS::Data::Vector3 position(m_vec3Position.x(), m_vec3Position.y(), m_vec3Position.z());

		FBS::GameEvent::EntityCreateBuilder data_builder(m_Builder);
		data_builder.add_EntityID(m_nEntityID);
		data_builder.add_MasterDataID(m_nMasterDataID);
		data_builder.add_Type(m_EntityType);
		data_builder.add_Pos(&position);
		auto data = data_builder.Finish();

		m_Builder.Finish(data);

		*pLength = m_Builder.GetSize();

		return (char*)m_Builder.GetBufferPointer();
	}
}