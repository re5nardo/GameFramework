#include "stdafx.h"
#include "Position.h"
#include "Position_generated.h"

namespace GameEvent
{
	Position::Position()
	{
	}

	Position::~Position()
	{
	}

	FBS::GameEventType Position::GetType()
	{
		return FBS::GameEventType::GameEventType_Position;
	}

	const char* Position::Serialize(int* pLength)
	{
		FBSData::Vector3 startPosition(m_vec3StartPosition.x(), m_vec3StartPosition.y(), m_vec3StartPosition.z());
		FBSData::Vector3 endPosition(m_vec3EndPosition.x(), m_vec3EndPosition.y(), m_vec3EndPosition.z());

		FBS::GameEvent::PositionBuilder data_builder(m_Builder);
		data_builder.add_EntityID(m_nEntityID);
		data_builder.add_StartTime(m_fStartTime);
		data_builder.add_EndTime(m_fEndTime);
		data_builder.add_StartPos(&startPosition);
		data_builder.add_EndPos(&endPosition);

		auto data = data_builder.Finish();

		m_Builder.Finish(data);

		*pLength = m_Builder.GetSize();

		return (char*)m_Builder.GetBufferPointer();
	}
}