#include "stdafx.h"
#include "Position.h"
#include "../../../FBSFiles/PlayerInput/Position_generated.h"

namespace PlayerInput
{
	Position::Position()
	{
	}

	Position::~Position()
	{
	}

	FBS::PlayerInputType Position::GetType()
	{
		return FBS::PlayerInputType::PlayerInputType_Position;
	}

	IPlayerInput* Position::Clone()
	{
		Position* pClone = new Position();

		pClone->m_nEntityID = m_nEntityID;
		pClone->m_vec3Position = m_vec3Position;

		return pClone;
	}

	const char* Position::Serialize(int* pLength)
	{
		FBS::Data::Vector3 position(m_vec3Position.x(), m_vec3Position.y(), m_vec3Position.z());

		FBS::PlayerInput::PositionBuilder data_builder(m_Builder);
		data_builder.add_EntityID(m_nEntityID);
		data_builder.add_Pos(&position);
		auto data = data_builder.Finish();

		m_Builder.Finish(data);

		*pLength = m_Builder.GetSize();

		return (char*)m_Builder.GetBufferPointer();
	}

	bool Position::Deserialize(const char* pChar)
	{
		auto data = flatbuffers::GetRoot<FBS::PlayerInput::Position>((const void*)pChar);

		m_nEntityID = data->EntityID();
		m_vec3Position.setValue(data->Pos()->x(), data->Pos()->y(), data->Pos()->z());

		return true;
	}
}


