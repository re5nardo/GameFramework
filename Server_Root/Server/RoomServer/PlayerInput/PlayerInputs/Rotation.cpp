#include "stdafx.h"
#include "Rotation.h"
#include "../../../FBSFiles/PlayerInput/Rotation_generated.h"

namespace PlayerInput
{
	Rotation::Rotation()
	{
	}

	Rotation::~Rotation()
	{
	}

	FBS::PlayerInputType Rotation::GetType()
	{
		return FBS::PlayerInputType::PlayerInputType_Rotation;
	}

	IPlayerInput* Rotation::Clone()
	{
		Rotation* pClone = new Rotation();

		pClone->m_nEntityID = m_nEntityID;
		pClone->m_vec3Rotation = m_vec3Rotation;

		return pClone;
	}

	const char* Rotation::Serialize(int* pLength)
	{
		FBS::Data::Vector3 rotation(m_vec3Rotation.x(), m_vec3Rotation.y(), m_vec3Rotation.z());

		FBS::PlayerInput::RotationBuilder data_builder(m_Builder);
		data_builder.add_EntityID(m_nEntityID);
		data_builder.add_Rot(&rotation);
		auto data = data_builder.Finish();

		m_Builder.Finish(data);

		*pLength = m_Builder.GetSize();

		return (char*)m_Builder.GetBufferPointer();
	}

	bool Rotation::Deserialize(const char* pChar)
	{
		auto data = flatbuffers::GetRoot<FBS::PlayerInput::Rotation>((const void*)pChar);

		m_nEntityID = data->EntityID();
		m_vec3Rotation.setValue(data->Rot()->x(), data->Rot()->y(), data->Rot()->z());

		return true;
	}
}


