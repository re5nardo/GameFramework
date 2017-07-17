#include "stdafx.h"
#include "Rotation.h"
#include "Rotation_generated.h"

namespace GameEvent
{
	Rotation::Rotation()
	{
	}

	Rotation::~Rotation()
	{
	}

	FBS::GameEventType Rotation::GetType()
	{
		return FBS::GameEventType::GameEventType_Rotation;
	}

	const char* Rotation::Serialize(int* pLength)
	{
		FBSData::Vector3 startRotation(m_vec3StartRotation.x(), m_vec3StartRotation.y(), m_vec3StartRotation.z());
		FBSData::Vector3 endRotation(m_vec3EndRotation.x(), m_vec3EndRotation.y(), m_vec3EndRotation.z());

		FBS::GameEvent::RotationBuilder data_builder(m_Builder);
		data_builder.add_EntityID(m_nEntityID);
		data_builder.add_StartTime(m_fStartTime);
		data_builder.add_EndTime(m_fEndTime);
		data_builder.add_StartRot(&startRotation);
		data_builder.add_EndRot(&endRotation);
		auto data = data_builder.Finish();

		m_Builder.Finish(data);

		*pLength = m_Builder.GetSize();

		return (char*)m_Builder.GetBufferPointer();
	}
}